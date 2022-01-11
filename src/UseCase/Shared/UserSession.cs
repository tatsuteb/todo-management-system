namespace UseCase.Shared
{
    public class UserSession
    {
        public string Id { get; }

        public UserSession(string id)
        {
            Id = id;
        }
    }
}