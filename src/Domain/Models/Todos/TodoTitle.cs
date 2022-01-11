using Domain.Models.Shared;
using Domain.Models.Users;

namespace Domain.Models.Todos
{
    public class TodoTitle : SingleValueObject<string>
    {
        public const int MaxTodoTitleLength = 50;

        public TodoTitle(string value) : base(value)
        {
            if (string.IsNullOrWhiteSpace(value) ||
                value.Length > MaxTodoTitleLength)
            {
                throw new DomainException($"タイトルは1～{MaxTodoTitleLength}文字以内で設定してください。");
            }
        }
    }
}