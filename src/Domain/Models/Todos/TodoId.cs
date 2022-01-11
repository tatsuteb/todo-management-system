using Domain.Models.Shared;
using Domain.Models.Users;

namespace Domain.Models.Todos
{
    public class TodoId : SingleValueObject<string>
    {
        private const int TodoIdLength = 16;

        public TodoId(string value) : base(value)
        {
            if (string.IsNullOrWhiteSpace(value) ||
                value.Length != TodoIdLength)
            {
                throw new DomainException("TODO IDの形式が不正です。");
            }
        }

        public static TodoId Generate()
        {
            return new TodoId(Guid.NewGuid().ToString("N")[..TodoIdLength]);
        }
    }
}