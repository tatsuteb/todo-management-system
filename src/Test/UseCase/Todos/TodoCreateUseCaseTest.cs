using System;
using System.Threading.Tasks;
using Domain.Models.Todos;
using Infrastructure.Todos;
using NUnit.Framework;
using Test.Shared;
using UseCase.Shared;
using UseCase.Todos.Create;

namespace Test.UseCase.Todos
{
    public class TodoCreateUseCaseTest : UseDbContextTestBase
    {
        private readonly TodoCreateUseCase _todoCreateUseCase;
        private readonly ITodoRepository _todoRepository;

        public TodoCreateUseCaseTest()
        {
            _todoRepository = new TodoRepository(TestDbContext);
            _todoCreateUseCase = new TodoCreateUseCase(_todoRepository);
        }

        [Test]
        public async Task 引数にタイトルなどを渡すと未完了状態でTODOが保存される()
        {
            // 準備
            var userId = Guid.NewGuid().ToString("D");
            const string title = "タイトル";
            const string description = "詳細";
            var beginDateTime = DateTime.Now;
            var dueDateTime = DateTime.Now.AddDays(7);

            // 実行
            var command = new TodoCreateCommand(
                userSession: new UserSession(userId),
                title: title,
                description: description,
                beginDateTime: beginDateTime,
                dueDateTime: dueDateTime);
            var result = await _todoCreateUseCase.ExecuteAsync(command);

            // 検証
            var todo = await _todoRepository.FindAsync(new TodoId(result.Id));
            Assert.That(todo, Is.Not.Null);
            Assert.That(todo?.Status, Is.EqualTo(TodoStatus.未完了));
        }
    }
}
