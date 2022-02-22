using Domain.Models.Todos;
using Infrastructure.Todos;
using NUnit.Framework;
using System.Threading.Tasks;
using Test.Helpers;
using Test.Shared;

namespace Test.Infrastructure.Todos
{
    public class TodoRepositoryTest : UseDbContextTestBase
    {
        private readonly ITodoRepository _todoRepository;

        public TodoRepositoryTest()
        {
            _todoRepository = new TodoRepository(TestDbContext);
        }

        [Test]
        public async Task 引数にTODOモデルを渡すとDBに保存される()
        {
            // 準備
            var todo = TodoGenerator.Generate();

            // 実行
            await _todoRepository.SaveAsync(todo);

            // 検証
            var todoDataModel = await TestDbContext.Todos
                .FindAsync(todo.Id.Value);
            Assert.That(todoDataModel, Is.Not.Null);
            Assert.That(todoDataModel?.Id, Is.EqualTo(todo.Id.Value));
            Assert.That(todoDataModel?.Title, Is.EqualTo(todo.Title.Value));
            Assert.That(todoDataModel?.Description, Is.EqualTo(todo.Description?.Value));
            Assert.That(todoDataModel?.BeginDateTime, Is.EqualTo(todo.BeginDateTime));
            Assert.That(todoDataModel?.DueDateTime, Is.EqualTo(todo.DueDateTime));
            Assert.That(todoDataModel?.OwnerId, Is.EqualTo(todo.OwnerId.Value));
            Assert.That(todoDataModel?.Status, Is.EqualTo((int)todo.Status));
            Assert.That(todoDataModel?.CreatedDateTime, Is.EqualTo(todo.CreatedDateTime));
            Assert.That(todoDataModel?.UpdatedDateTime, Is.EqualTo(todo.UpdatedDateTime));
            Assert.That(todoDataModel?.IsDeleted, Is.EqualTo(todo.IsDeleted));
            Assert.That(todoDataModel?.DeletedDateTime, Is.EqualTo(todo.DeletedDateTime));
        }

        [Test]
        public async Task 引数にTODOのIDを渡すとDBからTODOを生成する()
        {
            // 準備
            var todo = TodoGenerator.Generate();
            await _todoRepository.SaveAsync(todo);

            // 実行
            var foundTodo = await _todoRepository.FindAsync(todo.Id);

            // 検証
            Assert.That(foundTodo, Is.Not.Null);
            Assert.That(foundTodo?.Id, Is.EqualTo(todo.Id));
            Assert.That(foundTodo?.Title, Is.EqualTo(todo.Title));
            Assert.That(foundTodo?.Description, Is.EqualTo(todo.Description));
            Assert.That(foundTodo?.BeginDateTime, Is.EqualTo(todo.BeginDateTime));
            Assert.That(foundTodo?.DueDateTime, Is.EqualTo(todo.DueDateTime));
            Assert.That(foundTodo?.OwnerId, Is.EqualTo(todo.OwnerId));
            Assert.That(foundTodo?.Status, Is.EqualTo(todo.Status));
            Assert.That(foundTodo?.CreatedDateTime, Is.EqualTo(todo.CreatedDateTime));
            Assert.That(foundTodo?.UpdatedDateTime, Is.EqualTo(todo.UpdatedDateTime));
            Assert.That(foundTodo?.IsDeleted, Is.EqualTo(todo.IsDeleted));
            Assert.That(foundTodo?.DeletedDateTime, Is.EqualTo(todo.DeletedDateTime));
        }

        [Test]
        public async Task 引数に存在しないTODOのIDを渡すとnullが返る()
        {
            // 準備
            var todo = TodoGenerator.Generate();

            // 実行
            var foundTodo = await _todoRepository.FindAsync(todo.Id);

            // 検証
            Assert.That(foundTodo, Is.Null);
        }
    }
}
