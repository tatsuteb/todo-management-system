using UseCase.Shared;

namespace UseCase.Todos.Create
{
    public class TodoCreateCommand
    {
        public UserSession UserSession { get; }
        public string Title { get; }
        public string? Description { get; }

        public TodoCreateCommand(
            UserSession userSession,
            string title, 
            string? description)
        {
            UserSession = userSession;
            Title = title;
            Description = description;
        }
    }
}