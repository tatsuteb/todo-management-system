using UseCase.Shared;

namespace UseCase.Users.GetSignInInfo
{
    public class UserGetSignInInfoCommand
    {
        public UserSession UserSession { get; }

        public UserGetSignInInfoCommand(UserSession userSession)
        {
            UserSession = userSession;
        }
    }
}