using Domain.Models.Shared;
using Domain.Models.Users;

namespace Domain.Models.Todos
{
    public class Todo
    {
        public TodoId Id { get; }
        public TodoTitle Title { get; private set; }
        public TodoDescription? Description { get; private set; }
        public DateTime? BeginDateTime { get; private set; }
        public DateTime? DueDateTime { get; private set; }
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
            DateTime? beginDateTime,
            DateTime? dueDateTime,
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
            BeginDateTime = beginDateTime;
            DueDateTime = dueDateTime;
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
            DateTime? beginDateTime,
            DateTime? dueDateTime,
            UserId ownerId)
        {
            var operationDateTime = DateTime.Now;
            
            if (beginDateTime > dueDateTime)
            {
                throw new DomainException("開始日は期日より前になるように設定してください。");
            }

            return new Todo(
                id: TodoId.Generate(),
                title: title ?? throw new DomainException("タイトルを設定してください。"),
                description: description,
                beginDateTime: beginDateTime,
                dueDateTime: dueDateTime,
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
            DateTime? beginDateTime,
            DateTime? dueDateTime,
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
                beginDateTime: beginDateTime,
                dueDateTime: dueDateTime,
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

        public void Edit(
            TodoTitle title,
            TodoDescription? description,
            DateTime? beginDateTime,
            DateTime? dueDateTime)
        {
            if (IsDeleted)
            {
                throw new DomainException("削除したTODOを編集することはできません。");
            }

            if (beginDateTime > dueDateTime)
            {
                throw new DomainException("開始日は期日より前になるように設定してください。");
            }

            Title = title ?? throw new DomainException("タイトルを設定してください。");
            Description = description;
            BeginDateTime = beginDateTime;
            DueDateTime = dueDateTime;
            UpdatedDateTime = DateTime.Now;
        }
    }
}
