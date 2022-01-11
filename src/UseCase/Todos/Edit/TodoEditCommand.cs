using UseCase.Shared;

namespace UseCase.Todos.Edit
{
    public class TodoEditCommand
    {
        public UserSession UserSession { get; }
        public string Id { get; }
        public string Title { get; }
        public string? Description { get; }

        public TodoEditCommand(
            UserSession userSession, 
            string id, 
            string title, 
            string? description)
        {
            UserSession = userSession;
            Id = id;
            Title = title;
            Description = description;
        }
    }
}
