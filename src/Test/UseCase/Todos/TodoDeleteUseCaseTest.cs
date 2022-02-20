using System;
using System.Threading.Tasks;
using Domain.Models.Todos;
using Infrastructure.Todos;
using NUnit.Framework;
using Test.Helpers;
using Test.Shared;
using UseCase.Shared;
using UseCase.Todos.Delete;

namespace Test.UseCase.Todos
{
    public class TodoDeleteUseCaseTest : UseDbContextTestBase
    {
        private readonly TodoDeleteUseCase _todoDeleteUseCase;
        private readonly ITodoRepository _todoRepository;

        public TodoDeleteUseCaseTest()
        {
            _todoRepository = new TodoRepository(TestDbContext);
            _todoDeleteUseCase = new TodoDeleteUseCase(_todoRepository);
        }

        [Test]
        public async Task IDを指定して削除すると削除フラグを立てて保存する()
        {
            // 準備
            var todo = TodoGenerator.Generate();
            await _todoRepository.SaveAsync(todo);

            // 実行
            var command = new TodoDeleteCommand(
                userSession: new UserSession(todo.OwnerId.Value),
                id: todo.Id.Value);
            await _todoDeleteUseCase.ExecuteAsync(command);

            // 検証
            var deletedTodo = await _todoRepository.FindAsync(todo.Id);
            Assert.That(deletedTodo?.IsDeleted, Is.True);
            Assert.That(deletedTodo?.DeletedDateTime, Is.Not.Null);
        }

        [Test]
        public void 存在しないTODOを指定して削除すると例外が発生する()
        {
            // 準備
            var todo = TodoGenerator.Generate();

            // 実行・検証
            var command = new TodoDeleteCommand(
                userSession: new UserSession(todo.OwnerId.Value),
                id: todo.Id.Value);
            Assert.That(
                async () => await _todoDeleteUseCase.ExecuteAsync(command),
                Throws.TypeOf<UseCaseException>());
        }

        [Test]
        public async Task 他人が所有するTODOを指定して削除しようとすると例外が発生する()
        {
            // 準備
            var todo = TodoGenerator.Generate();
            await _todoRepository.SaveAsync(todo);

            var userId = Guid.NewGuid().ToString("D");

            // 実行・検証
            var command = new TodoDeleteCommand(
                userSession: new UserSession(userId),
                id: todo.Id.Value);
            Assert.That(
                async () => await _todoDeleteUseCase.ExecuteAsync(command),
                Throws.TypeOf<UseCaseException>());
        }
    }
}
