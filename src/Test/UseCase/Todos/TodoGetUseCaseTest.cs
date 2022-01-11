using System;
using Domain.Models.Todos;
using Infrastructure.Todos;
using NUnit.Framework;
using System.Threading.Tasks;
using Test.Helpers;
using Test.Shared;
using UseCase.Shared;
using UseCase.Todos.Get;

namespace Test.UseCase.Todos
{
    public class TodoGetUseCaseTest : UseDbContextTestBase
    {
        private readonly TodoGetUseCase _todoGetUseCase;
        private readonly ITodoRepository _todoRepository;

        public TodoGetUseCaseTest()
        {
            _todoRepository = new TodoRepository(TestDbContext);
            _todoGetUseCase = new TodoGetUseCase(_todoRepository);
        }


        [Test]
        public async Task TODOのIDを指定して詳細を取得する()
        {
            // 準備
            var todo = TodoGenerator.Generate();
            await _todoRepository.SaveAsync(todo);

            // 実行
            var command = new TodoGetCommand(
                userSession: new UserSession(todo.OwnerId.Value),
                id: todo.Id.Value);
            var result = await _todoGetUseCase.ExecuteAsync(command);

            // 検証
            Assert.That(result.Todo.Id, Is.EqualTo(todo.Id.Value));
            Assert.That(result.Todo.Title, Is.EqualTo(todo.Title.Value));
            Assert.That(result.Todo.Description, Is.EqualTo(todo.Description?.Value));
            Assert.That(result.Todo.CreatedDateTime, Is.EqualTo(todo.CreatedDateTime));
            Assert.That(result.Todo.Status, Is.EqualTo((int) todo.Status));
            Assert.That(result.Todo.StatusName, Is.EqualTo(todo.Status.ToString()));
        }

        [Test]
        public async Task 存在しないTODOのIDを指定すると例外が発生する()
        {
            // 準備
            var userId = Guid.NewGuid().ToString();
            var todoId = TodoId.Generate().Value;

            // 実行・検証
            var command = new TodoGetCommand(
                userSession: new UserSession(userId),
                id: todoId);
            
            Assert.That(
                async () => await _todoGetUseCase.ExecuteAsync(command),
                Throws.TypeOf<UseCaseException>());
        }

        [Test]
        public async Task 他のユーザー所有するTODOのIDを指定すると例外が発生する()
        {
            // 準備
            var userId = Guid.NewGuid().ToString();

            var todo = TodoGenerator.Generate();
            await _todoRepository.SaveAsync(todo);

            // 実行・検証
            var command = new TodoGetCommand(
                userSession: new UserSession(userId),
                id: todo.Id.Value);

            Assert.That(
                async () => await _todoGetUseCase.ExecuteAsync(command),
                Throws.TypeOf<UseCaseException>());
        }
    }
}
