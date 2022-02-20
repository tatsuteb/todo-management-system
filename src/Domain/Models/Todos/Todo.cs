using Domain.Models.Shared;
using Domain.Models.Users;

namespace Domain.Models.Todos
{
    public class Todo
    {
        public TodoId Id { get; }
        public TodoTitle Title { get; private set; }
        public TodoDescription? Description { get; private set; }
        public UserId OwnerId { get; }
        public DateTime CreatedDateTime { get; }
        public DateTime UpdatedDateTime { get; private set; }
        public TodoStatus Status { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedDateTime { get; private set; }

        private Todo(
            TodoId id, 
            TodoTitle title, 
            TodoDescription? description, 
            UserId ownerId, 
            DateTime createdDateTime, 
            DateTime updatedDateTime, 
            TodoStatus status,
            bool isDeleted,
            DateTime? deletedDateTime)
        {
            Id = id;
            Title = title;
            Description = description;
            OwnerId = ownerId;
            CreatedDateTime = createdDateTime;
            UpdatedDateTime = updatedDateTime;
            Status = status;
            IsDeleted = isDeleted;
            DeletedDateTime = deletedDateTime;
        }

        public static Todo CreateNew(
            TodoTitle title,
            TodoDescription? description,
            UserId ownerId)
        {
            var operationDateTime = DateTime.Now;
            
            return new Todo(
                id: TodoId.Generate(),
                title: title ?? throw new DomainException("タイトルを設定してください。"),
                description: description,
                ownerId: ownerId,
                createdDateTime: operationDateTime,
                updatedDateTime: operationDateTime,
                status: TodoStatus.未完了,
                isDeleted: false,
                deletedDateTime: null);
        }

        public static Todo CreateFromRepository(
            TodoId id,
            TodoTitle title,
            TodoDescription? description,
            UserId ownerId,
            DateTime createdDateTime,
            DateTime updatedDateTime,
            TodoStatus status,
            bool isDeleted,
            DateTime? deletedDateTime)
        {
            return new Todo(
                id: id,
                title: title,
                description: description,
                ownerId: ownerId,
                createdDateTime: createdDateTime,
                updatedDateTime: updatedDateTime,
                status: status,
                isDeleted: isDeleted,
                deletedDateTime: deletedDateTime);
        }

        public void UpdateStatus(TodoStatus status)
        {
            if (Status == status)
            {
                return;
            }

            if (IsDeleted)
            {
                throw new DomainException("削除したTODOのステータスを更新することはできません。");
            }

            Status = status;
            UpdatedDateTime = DateTime.Now;
        }

        public void Delete()
        {
            if (IsDeleted)
            {
                return;
            }

            IsDeleted = true;
            DeletedDateTime = DateTime.Now;
        }

        public void Edit(TodoTitle title, TodoDescription? description)
        {
            if (IsDeleted)
            {
                throw new DomainException("削除したTODOを編集することはできません。");
            }

            Title = title ?? throw new DomainException("タイトルを設定してください。");
            Description = description;
            UpdatedDateTime = DateTime.Now;
        }
    }
}
