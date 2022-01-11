using System;
using System.Threading.Tasks;
using Domain.Models.Todos;
using Infrastructure.Todos;
using NUnit.Framework;
using Test.Helpers;
using Test.Shared;
using UseCase.Shared;
using UseCase.Todos.UpdateStatus;

namespace Test.UseCase.Todos
{
    public class TodoUpdateStatusUseCaseTest : UseDbContextTestBase
    {
        private readonly TodoUpdateStatusUseCase _todoUpdateStatusUseCase;
        private readonly ITodoRepository _todoRepository;

        public TodoUpdateStatusUseCaseTest()
        {
            _todoRepository = new TodoRepository(TestDbContext);
            _todoUpdateStatusUseCase = new TodoUpdateStatusUseCase(_todoRepository);
        }

        [Test]
        public async Task 引数に状態を渡すとTODOの状態を更新して保存する()
        {
            // 準備
            var todo = TodoGenerator.Generate(status: TodoStatus.未完了);
            await _todoRepository.SaveAsync(todo);

            // 実行
            var command = new TodoUpdateStatusCommand(
                userSession: new UserSession(todo.OwnerId.Value),
                id: todo.Id.Value,
                status: (int)TodoStatus.完了);
            await _todoUpdateStatusUseCase.ExecuteAsync(command);

            // 検証
            var updatedTodo = await _todoRepository.FindAsync(todo.Id);
            Assert.That(updatedTodo?.Status, Is.EqualTo(TodoStatus.完了));
        }

        [Test]
        public void 存在しないTODOの状態を更新しようとすると例外が発生する()
        {
            // 準備
            var todo = TodoGenerator.Generate(status: TodoStatus.未完了);

            // 実行・検証
            var command = new TodoUpdateStatusCommand(
                userSession: new UserSession(todo.OwnerId.Value),
                id: todo.Id.Value,
                status: (int)TodoStatus.完了);
            Assert.That(
                async () => await _todoUpdateStatusUseCase.ExecuteAsync(command),
                Throws.TypeOf<UseCaseException>());
        }


        [Test]
        public async Task 他人が所有しているTODOの状態を更新しようとすると例外が発生する()
        {
            // 準備
            var todo = TodoGenerator.Generate(status: TodoStatus.未完了);
            await _todoRepository.SaveAsync(todo);

            var userId = Guid.NewGuid().ToString("D");

            // 実行・検証
            var command = new TodoUpdateStatusCommand(
                userSession: new UserSession(userId),
                id: todo.Id.Value,
                status: (int)TodoStatus.完了);
            Assert.That(
                async () => await _todoUpdateStatusUseCase.ExecuteAsync(command),
                Throws.TypeOf<UseCaseException>());
        }
    }
}
