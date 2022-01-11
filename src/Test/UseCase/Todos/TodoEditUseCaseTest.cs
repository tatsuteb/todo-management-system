using System;
using System.Threading.Tasks;
using Domain.Models.Todos;
using Infrastructure.Todos;
using NUnit.Framework;
using Test.Helpers;
using Test.Shared;
using UseCase.Shared;
using UseCase.Todos.Edit;

namespace Test.UseCase.Todos
{
    public class TodoEditUseCaseTest : UseDbContextTestBase
    {
        private readonly TodoEditUseCase _todoEditUseCase;
        private readonly ITodoRepository _todoRepository;

        public TodoEditUseCaseTest()
        {
            _todoRepository = new TodoRepository(TestDbContext);
            _todoEditUseCase = new TodoEditUseCase(_todoRepository);
        }

        [Test]
        public async Task 引数にタイトルと詳細を渡すと更新して保存する()
        {
            // 準備
            var todo = TodoGenerator.Generate(
                title: "タイトル",
                description: "説明文");
            await _todoRepository.SaveAsync(todo);

            const string newTitle = "新しいタイトル";
            const string newDescription = "新しい説明文";

            // 実行
            var command = new TodoEditCommand(
                userSession: new UserSession(todo.OwnerId.Value),
                id: todo.Id.Value,
                title: newTitle,
                description: newDescription);
            await _todoEditUseCase.ExecuteAsync(command);

            // 検証
            var editedTodo = await _todoRepository.FindAsync(todo.Id);
            Assert.That(editedTodo?.Title.Value, Is.EqualTo(newTitle));
            Assert.That(editedTodo?.Description?.Value, Is.EqualTo(newDescription));
        }

        [Test]
        public async Task 存在しないTODOを編集すると例外が発生する()
        {
            // 準備
            var todo = TodoGenerator.Generate(
                title: "タイトル",
                description: "説明文");

            const string newTitle = "新しいタイトル";
            const string newDescription = "新しい説明文";

            // 実行・検証
            var command = new TodoEditCommand(
                userSession: new UserSession(todo.OwnerId.Value),
                id: todo.Id.Value,
                title: newTitle,
                description: newDescription);

            Assert.That(
                async () => await _todoEditUseCase.ExecuteAsync(command),
                Throws.TypeOf<UseCaseException>());
        }

        [Test]
        public async Task 他のユーザーのTODOを編集しようとすると例外が発生する()
        {
            // 準備
            var todo = TodoGenerator.Generate(
                title: "タイトル",
                description: "説明文",
                isDeleted: true);
            await _todoRepository.SaveAsync(todo);

            const string newTitle = "新しいタイトル";
            const string newDescription = "新しい説明文";

            var userId = Guid.NewGuid().ToString();

            // 実行・検証
            var command = new TodoEditCommand(
                userSession: new UserSession(userId),
                id: todo.Id.Value,
                title: newTitle,
                description: newDescription);

            Assert.That(
                async () => await _todoEditUseCase.ExecuteAsync(command),
                Throws.TypeOf<UseCaseException>());
        }
    }
}
