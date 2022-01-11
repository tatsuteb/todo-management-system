using UseCase.Shared;

namespace UseCase.Todos.UpdateStatus
{
    public class TodoUpdateStatusCommand
    {
        public UserSession UserSession { get; }
        public string Id { get; }
        public int Status { get; }

        public TodoUpdateStatusCommand(
            UserSession userSession,
            string id, 
            int status)
        {
            UserSession = userSession;
            Id = id;
            Status = status;
        }
    }
}