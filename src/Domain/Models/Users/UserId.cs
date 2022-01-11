using Domain.Models.Shared;

namespace Domain.Models.Users
{
    public class UserId : SingleValueObject<string>
    {
        public UserId(string value) : base(value)
        {
            if (!Guid.TryParseExact(value, "D", out _))
            {
                throw new DomainException("ユーザーIDの形式が不正です。");
            }
        }
    }
}