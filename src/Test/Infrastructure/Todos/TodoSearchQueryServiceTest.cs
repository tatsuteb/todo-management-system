using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models.Todos;
using Infrastructure.Todos;
using NUnit.Framework;
using Test.Helpers;
using Test.Shared;
using UseCase.Shared;
using UseCase.Todos.Search;

namespace Test.Infrastructure.Todos
{
    public class TodoSearchQueryServiceTest : UseDbContextTestBase
    {
        private readonly ITodoSearchQueryService _todoSearchQueryService;
        private readonly ITodoRepository _todoRepository;

        public TodoSearchQueryServiceTest()
        {
            _todoSearchQueryService = new TodoSearchQueryService(TestDbContext);
            _todoRepository = new TodoRepository(TestDbContext);
        }

        [Test]
        public async Task オプションを指定しないと削除済み以外のすべてのTODOが返される()
        {
            // 準備
            var userId = Guid.NewGuid().ToString("D");
            var todo1 = TodoGenerator.Generate(
                ownerId: userId);
            var todo2 = TodoGenerator.Generate(
                ownerId: userId,
                status: TodoStatus.完了);
            // 削除済みTODO
            var todo3 = TodoGenerator.Generate(
                ownerId: userId,
                isDeleted: true,
                deletedDateTime: DateTime.Now);

            await _todoRepository.SaveAsync(todo1);
            await _todoRepository.SaveAsync(todo2);
            await _todoRepository.SaveAsync(todo3);

            // 実行
            var command = new TodoSearchCommand(
                userSession: new UserSession(userId));
            var result = await _todoSearchQueryService.ExecuteAsync(command);

            // 検証
            Assert.That(result.Total, Is.EqualTo(2));
        }

        [Test]
        public async Task オプションでキーワードを指定すると削除済み以外でタイトルまたは詳細にキーワードを含むTODOが返される()
        {
            // 準備
            var userId = Guid.NewGuid().ToString("D");
            var todo1 = TodoGenerator.Generate(
                title: "keyword",
                ownerId: userId);
            var todo2 = TodoGenerator.Generate(
                description: "keyword",
                ownerId: userId,
                status: TodoStatus.完了);
            // 削除済みTODO
            var todo3 = TodoGenerator.Generate(
                title: "keyword",
                ownerId: userId,
                isDeleted: true,
                deletedDateTime: DateTime.Now);

            await _todoRepository.SaveAsync(todo1);
            await _todoRepository.SaveAsync(todo2);
            await _todoRepository.SaveAsync(todo3);

            // 実行
            var command = new TodoSearchCommand(
                userSession: new UserSession(userId),
                keyword: "keyword");
            var result = await _todoSearchQueryService.ExecuteAsync(command);

            // 検証
            Assert.That(result.Total, Is.EqualTo(2));
            Assert.That(result.Summaries.Any(x => x.Id == todo1.Id.Value), Is.True);
            Assert.That(result.Summaries.Any(x => x.Id == todo2.Id.Value), Is.True);
            Assert.That(result.Summaries.Any(x => x.Id == todo3.Id.Value), Is.False);
        }

        [Test]
        public async Task オプションでステータスを指定すると削除済み以外で同じステータスのTODOが返される()
        {
            // 準備
            var userId = Guid.NewGuid().ToString("D");
            var todo1 = TodoGenerator.Generate(
                ownerId: userId,
                status: TodoStatus.未完了);
            var todo2 = TodoGenerator.Generate(
                ownerId: userId,
                status: TodoStatus.完了);
            // 削除済みTODO
            var todo3 = TodoGenerator.Generate(
                ownerId: userId,
                status: TodoStatus.未完了,
                isDeleted: true,
                deletedDateTime: DateTime.Now);

            await _todoRepository.SaveAsync(todo1);
            await _todoRepository.SaveAsync(todo2);
            await _todoRepository.SaveAsync(todo3);

            // 実行
            var command = new TodoSearchCommand(
                userSession: new UserSession(userId),
                statuses: new []{ (int) TodoStatus.未完了 });
            var result = await _todoSearchQueryService.ExecuteAsync(command);

            // 検証
            Assert.That(result.Total, Is.EqualTo(1));
        }

        [Test]
        public async Task オプションでキーワードとステータスを指定すると削除済み以外ですべての条件を満たすTODOが返される()
        {
            // 準備
            var userId = Guid.NewGuid().ToString("D");
            var todo1 = TodoGenerator.Generate(
                title: "keyword",
                ownerId: userId,
                status: TodoStatus.未完了);
            var todo2 = TodoGenerator.Generate(
                title: "keyword",
                ownerId: userId,
                status: TodoStatus.完了);
            // 削除済みTODO
            var todo3 = TodoGenerator.Generate(
                ownerId: userId,
                status: TodoStatus.未完了,
                isDeleted: true,
                deletedDateTime: DateTime.Now);

            await _todoRepository.SaveAsync(todo1);
            await _todoRepository.SaveAsync(todo2);
            await _todoRepository.SaveAsync(todo3);

            // 実行
            var command = new TodoSearchCommand(
                userSession: new UserSession(userId),
                keyword: "keyword",
                statuses: new[] { (int)TodoStatus.未完了 });
            var result = await _todoSearchQueryService.ExecuteAsync(command);

            // 検証
            Assert.That(result.Total, Is.EqualTo(1));
        }
    }
}
