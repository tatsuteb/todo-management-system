using System.Text.RegularExpressions;
using Domain.Models.Shared;

namespace Domain.Models.Users
{
    public class Username : SingleValueObject<string>
    {
        public const int MaxUsernameLength = 30;

        public Username(string value) : base(value)
        {
            if (string.IsNullOrWhiteSpace(value) ||
                value.Length > MaxUsernameLength ||
                !Regex.IsMatch(value, "^[a-zA-Z]+[a-zA-Z0-9_\\-]*$"))
            {
                throw new DomainException(
                    $"ユーザー名は1～{MaxUsernameLength}文字の半角英数字、ハイフン、アンダーバーで設定してください。");
            }
        }
    }
}