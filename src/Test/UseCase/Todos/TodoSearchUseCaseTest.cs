using Infrastructure.Todos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models.Todos;
using Domain.Models.Users;
using NUnit.Framework;
using Test.Helpers;
using Test.Shared;
using UseCase.Shared;
using UseCase.Todos.Search;

namespace Test.UseCase.Todos
{
    public class TodoSearchUseCaseTest : UseDbContextTestBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ITodoSearchQueryService _todoSearchQueryService;

        public TodoSearchUseCaseTest()
        {
            _todoRepository = new TodoRepository(TestDbContext);
            _todoSearchQueryService = new TodoSearchQueryService(TestDbContext);
        }

        [TestCase("", new [] { (int)TodoStatus.完了 }, ExpectedResult = 1, TestName = "完了したTODO一覧を取得する")]
        [TestCase("keyword", new int[] {}, ExpectedResult = 1, TestName = "キーワードを含むTODO一覧を取得する")]
        [TestCase("", new int[] { }, ExpectedResult = 3, TestName = "すべてのTODO一覧を取得する")]
        public async Task<int> 削除済み以外で条件に合ったTODOの一覧を取得する(string keyword, IEnumerable<int> statuses)
        {
            // 準備
            var userId = Guid.NewGuid().ToString();
            var todos = new Todo[]
            {
                TodoGenerator.Generate(
                    title: "タイトル1",
                    description: "説明文1",
                    ownerId: userId,
                    status: TodoStatus.未完了),
                TodoGenerator.Generate(
                    title: $"タイトル2_{keyword}",
                    description: "説明文2",
                    ownerId: userId,
                    status: TodoStatus.未完了),
                TodoGenerator.Generate(
                    title: "タイトル3",
                    description: "説明文3",
                    ownerId: userId,
                    status: TodoStatus.完了),
                TodoGenerator.Generate(
                    title: "タイトル4",
                    description: "説明文4",
                    ownerId: userId,
                    status: TodoStatus.完了,
                    isDeleted: true)
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

            return result.Total;
        }
    }
}
