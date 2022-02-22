
namespace UseCase.Todos.Get
{
    public class TodoData
    {
        public string Id { get; }
        public string Title { get; }
        public DateTime? BeginDateTime { get; }
        public DateTime? DueDateTime { get; }
        public string? Description { get; }
        public int Status { get; }
        public string StatusName { get; }
        public DateTime CreatedDateTime { get; }

        public TodoData(
            string id, 
            string title, 
            DateTime? beginDateTime,
            DateTime? dueDateTime,
            string? description, 
            int status, 
            string statusName, 
            DateTime createdDateTime)
        {
            Id = id;
            Title = title;
            BeginDateTime = beginDateTime;
            DueDateTime = dueDateTime;
            Description = description;
            Status = status;
            StatusName = statusName;
            CreatedDateTime = createdDateTime;
        }
    }
}