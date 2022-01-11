namespace UseCase.Users.GetSignInInfo
{
    public interface IUserGetSignInInfoQueryService
    {
        Task<UserGetSignInInfoResult> ExecuteAsync(UserGetSignInInfoCommand command);
    }
}
