using UseCase.Shared;

namespace UseCase.Todos.Create
{
    public class TodoCreateCommand
    {
        public UserSession UserSession { get; }
        public string Title { get; }
        public string? Description { get; }
        public DateTime? BeginDateTime { get; }
        public DateTime? DueDateTime { get; }

        public TodoCreateCommand(
            UserSession userSession,
            string title, 
            string? description,
            DateTime? beginDateTime,
            DateTime? dueDateTime)
        {
            UserSession = userSession;
            Title = title;
            Description = description;
            BeginDateTime = beginDateTime;
            DueDateTime = dueDateTime;
        }
    }
}