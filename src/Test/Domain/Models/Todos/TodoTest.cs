using Domain.Models.Shared;
using Domain.Models.Todos;
using Domain.Models.Users;
using NUnit.Framework;
using System;
using Test.Helpers;

namespace Test.Domain.Models.Todos
{
    public class TodoTest
    {
        #region CreateNew, CreateFromRepository

        [Test]
        public void 新しくTODOを作成すると未完了状態でインスタンスが生成される()
        {
            // 準備
            var userId = new UserId(Guid.NewGuid().ToString("D"));
            var title = new TodoTitle("タイトル");
            var description = new TodoDescription("詳細");

            var operationDateTime = DateTime.Now;
            
            // 実行
            var todo = Todo.CreateNew(
                title: title,
                description: description,
                ownerId: userId);

            // 検証
            Assert.That(todo.Title, Is.EqualTo(title));
            Assert.That(todo.Description, Is.EqualTo(description));
            Assert.That(todo.OwnerId, Is.EqualTo(userId));
            Assert.That(todo.CreatedDateTime, Is.InRange(operationDateTime, operationDateTime.AddSeconds(10)));
            Assert.That(todo.UpdatedDateTime, Is.InRange(operationDateTime, operationDateTime.AddSeconds(10)));
            Assert.That(todo.Status, Is.EqualTo(TodoStatus.未完了));
            Assert.That(todo.IsDeleted, Is.False);
            Assert.That(todo.DeletedDateTime, Is.Null);
        }

        [Test]
        public void リポジトリからTODOを作成するとインスタンスが生成される()
        {
            // 準備
            var todoId = TodoId.Generate();
            var userId = new UserId(Guid.NewGuid().ToString("D"));
            var title = new TodoTitle("タイトル");
            var description = new TodoDescription("詳細");

            var createdDateTime = DateTime.Now;
            var updatedDateTime = DateTime.Now.AddDays(1);

            // 実行
            var todo = Todo.CreateFromRepository(
                id: todoId,
                title: title,
                description: description,
                ownerId: userId,
                createdDateTime: createdDateTime,
                updatedDateTime: updatedDateTime,
                status: TodoStatus.完了,
                isDeleted: false,
                deletedDateTime: null);

            // 検証
            Assert.That(todo.Title, Is.EqualTo(title));
            Assert.That(todo.Description, Is.EqualTo(description));
            Assert.That(todo.OwnerId, Is.EqualTo(userId));
            Assert.That(todo.CreatedDateTime, Is.EqualTo(createdDateTime));
            Assert.That(todo.UpdatedDateTime, Is.EqualTo(updatedDateTime));
            Assert.That(todo.Status, Is.EqualTo(TodoStatus.完了));
            Assert.That(todo.IsDeleted, Is.False);
            Assert.That(todo.DeletedDateTime, Is.Null);
        }

        #endregion

        #region UpdateStatus

        [Test]
        public void 引数にステータスを渡すとステータスが更新される()
        {
            // 準備
            var todo = TodoGenerator.Generate(
                status: TodoStatus.未完了,
                isDeleted: false);

            // 実行
            todo.UpdateStatus(TodoStatus.完了);

            // 検証
            Assert.That(todo.Status, Is.EqualTo(TodoStatus.完了));
        }

        [Test]
        public void 削除済みのTODOのステータスを更新すると例外が発生する()
        {
            // 準備
            var todo = TodoGenerator.Generate(
                isDeleted: true,
                deletedDateTime: DateTime.Now);

            // 実行・検証
            Assert.That(
                () => todo.UpdateStatus(TodoStatus.完了),
                Throws.TypeOf<DomainException>());
        }

        #endregion

        #region Delete

        [Test]
        public void TODOを削除すると削除フラグがたち削除した日付が入る()
        {
            // 準備
            var todo = TodoGenerator.Generate(
                isDeleted: false,
                deletedDateTime: null);

            // 実行
            todo.Delete();

            // 検証
            Assert.That(todo.IsDeleted, Is.True);
            Assert.That(todo.DeletedDateTime, Is.InRange(DateTime.Now.AddSeconds(-10), DateTime.Now.AddSeconds(10)));
        }

        #endregion

        #region Edit

        [Test]
        public void 引数にタイトルと詳細を渡して編集する()
        {
            // 準備
            var todo = TodoGenerator.Generate(
                title: "タイトル",
                description: "説明文");
            var newTitle = new TodoTitle("新しいタイトル");
            var newDescription = new TodoDescription("新しい説明文");

            // 実行
            todo.Edit(newTitle, newDescription);

            // 検証
            Assert.That(todo.Title, Is.EqualTo(newTitle));
            Assert.That(todo.Description, Is.EqualTo(newDescription));
        }

        [Test]
        public void 削除済みのTODOを編集しようとすると例外が発生する()
        {
            // 準備
            var todo = TodoGenerator.Generate(
                title: "タイトル",
                description: "説明文",
                isDeleted: true);
            var newTitle = new TodoTitle("新しいタイトル");
            var newDescription = new TodoDescription("新しい説明文");

            // 実行・検証
            Assert.That(
                () => todo.Edit(newTitle, newDescription),
                Throws.TypeOf<DomainException>());
        }

        #endregion
    }
}
