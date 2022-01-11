namespace UseCase.Users.TempRegister
{
    public class UserTempRegisterResult
    {
        public string Id { get; }

        public UserTempRegisterResult(string id)
        {
            Id = id;
        }
    }
}