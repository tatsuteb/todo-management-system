using UseCase.Shared;

namespace UseCase.Users.TempRegister
{
    public class UserTempRegisterCommand
    {
        public UserSession UserSession { get; }
        public string Name { get; }
        public string Email { get; }
        public string Nickname { get; }

        public UserTempRegisterCommand(
            UserSession userSession, 
            string name,
            string email,
            string nickname)
        {
            UserSession = userSession;
            Name = name;
            Email = email;
            Nickname = nickname;
        }
    }
}