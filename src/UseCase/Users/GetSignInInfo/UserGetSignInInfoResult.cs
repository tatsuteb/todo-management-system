namespace UseCase.Users.GetSignInInfo
{
    public class UserGetSignInInfoResult
    {
        public UserSignInInfoData? SignInInfo { get; }

        public UserGetSignInInfoResult(UserSignInInfoData? signInInfo)
        {
            SignInInfo = signInInfo;
        }
    }
}