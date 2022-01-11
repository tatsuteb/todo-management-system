using System.Net.Mail;
using Domain.Models.Shared;

namespace Domain.Models.Users
{
    public class UserEmailAddress : SingleValueObject<string>
    {
        public UserEmailAddress(string value) : base(value)
        {
            if (string.IsNullOrWhiteSpace(value) ||
                !CheckEmailFormat(value))
            {
                throw new DomainException("メールアドレスの形式が不正です。");
            }
        }

        private static bool CheckEmailFormat(string email)
        {
            try
            {
                _ = new MailAddress(email);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }
    }
}