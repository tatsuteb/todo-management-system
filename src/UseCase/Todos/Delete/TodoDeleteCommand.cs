using UseCase.Shared;

namespace UseCase.Todos.Delete
{
    public class TodoDeleteCommand
    {
        public UserSession UserSession { get; }
        public string Id { get; }

        public TodoDeleteCommand(UserSession userSession, string id)
        {
            UserSession = userSession;
            Id = id;
        }
    }
}