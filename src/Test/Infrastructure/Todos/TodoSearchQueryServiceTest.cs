using Domain.Models.Todos;
using Infrastructure.Todos;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
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

        [TestCase("keyword", new [] { (int) TodoStatus.未完了 }, ExpectedResult = new object[] { 1, true }, TestName = "キーワードとステータスを指定すると削除済み以外ですべての条件を満たすTODOが返される")]
        [TestCase(null, new [] { (int) TodoStatus.未完了 }, ExpectedResult = new object[] { 2, false }, TestName = "ステータスのみ指定すると削除済み以外で同じステータスのTODOが返される")]
        [TestCase("keyword", null, ExpectedResult = new object[] { 2, true }, TestName = "キーワードのみ指定すると削除済み以外でタイトルまたは詳細にキーワードを含むTODOが返される")]
        [TestCase(null, null, ExpectedResult = new object[] { 4, false }, TestName = "キーワード、ステータス指定なしだと削除済み以外のすべてのTODOが返される")]
        public async Task<object[]> 条件を満たすTODOが返される(string keyword, int[]? statuses)
        {
            // 準備
            var userId = Guid.NewGuid().ToString("D");
            var todos = new []
            {
                TodoGenerator.Generate(
                    ownerId: userId),
                TodoGenerator.Generate(
                    ownerId: userId,
                    status: TodoStatus.完了),
                TodoGenerator.Generate(
                    title: "keyword",
                    ownerId: userId),
                TodoGenerator.Generate(
                    description: "keyword",
                    ownerId: userId,
                    status: TodoStatus.完了),
                // 削除済みTODO
                TodoGenerator.Generate(
                    ownerId: userId,
                    isDeleted: true,
                    deletedDateTime: DateTime.Now)
            };

            foreach (var todo in todos)
            {
                await _todoRepository.SaveAsync(todo);
            }

            // 実行
            var command = new TodoSearchCommand(
                userSession: new UserSession(userId),
                keyword: keyword,
                statuses: statuses);
            var result = await _todoSearchQueryService.ExecuteAsync(command);

            // 検証
            var hasKeyword = !string.IsNullOrWhiteSpace(keyword) && 
                             result.Summaries
                                 .Join(todos, 
                                     x => x.Id, y => y.Id.Value,
                                    (x, y) => new { Title = y.Title.Value, Description = y.Description?.Value ?? "" })
                                .Any(x =>
                                    string.IsNullOrWhiteSpace(keyword) ||
                                    x.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                    x.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));

            return new object[] { result.Total, hasKeyword };
        }
    }
}
