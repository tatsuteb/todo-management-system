using Domain.Models.Shared;

namespace Domain.Models.Users.UserProfiles
{
    public class UserNickname : SingleValueObject<string>
    {
        public const int MaxNicknameLength = 30;

        public UserNickname(string value) : base(value)
        {
            if (string.IsNullOrWhiteSpace(value) ||
                value.Length > MaxNicknameLength)
            {
                throw new DomainException($"ニックネームは1～{MaxNicknameLength}文字以内で設定してください。");
            }
        }
    }
}