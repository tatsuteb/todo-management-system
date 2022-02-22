using UseCase.Shared;

namespace UseCase.Todos.Edit
{
    public class TodoEditCommand
    {
        public UserSession UserSession { get; }
        public string Id { get; }
        public string Title { get; }
        public string? Description { get; }
        public DateTime? BeginDateTime { get; }
        public DateTime? DueDateTime { get; }

        public TodoEditCommand(
            UserSession userSession, 
            string id, 
            string title, 
            string? description,
            DateTime? beginDateTime,
            DateTime? dueDateTime)
        {
            UserSession = userSession;
            Id = id;
            Title = title;
            Description = description;
            BeginDateTime = beginDateTime;
            DueDateTime = dueDateTime;
        }
    }
}
