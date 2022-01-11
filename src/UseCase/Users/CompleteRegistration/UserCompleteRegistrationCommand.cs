using UseCase.Shared;

namespace UseCase.Users.CompleteRegistration
{
    public class UserCompleteRegistrationCommand
    {
        public UserSession UserSession { get; }

        public UserCompleteRegistrationCommand(UserSession userSession)
        {
            UserSession = userSession;
        }
    }
}