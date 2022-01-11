using UseCase.Shared;

namespace UseCase.Todos.Get
{
    public class TodoGetCommand
    {
        public UserSession UserSession { get; }
        public string Id { get; }

        public TodoGetCommand(UserSession userSession, string id)
        {
            UserSession = userSession;
            Id = id;
        }
    }
}