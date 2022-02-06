using Domain.Models.Todos;
using System;
using Domain.Models.Users;

namespace Test.Helpers
{
    internal class TodoGenerator
    {
        public static Todo Generate(
            string? id = null,
            string? title = null,
            string? description = null,
            string? ownerId = null,
            DateTime? createdDateTime = null,
            DateTime? updatedDateTime = null,
            TodoStatus status = TodoStatus.未完了,
            bool isDeleted = false,
            DateTime? deletedDateTime = null)
        {
            return Todo.CreateFromRepository(
                id: id is null ? TodoId.Generate() : new TodoId(id),
                title: title is null ? new TodoTitle("タイトル") : new TodoTitle(title),
                description: description is null ? new TodoDescription("詳細") : new TodoDescription(description),
                ownerId: ownerId is null ? new UserId(Guid.NewGuid().ToString("D")) : new UserId(ownerId),
                createdDateTime: createdDateTime ?? DateTime.Now,
                updatedDateTime: updatedDateTime ?? DateTime.Now,
                status: status,
                isDeleted: isDeleted,
                deletedDateTime: isDeleted
                    ? deletedDateTime ?? DateTime.Now
                    : null);
        }
    }
}
