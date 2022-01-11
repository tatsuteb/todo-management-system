using Domain.Models.Shared;

namespace Domain.Models.Users.UserProfiles
{
    public class UserProfile
    {
        public UserId Id { get; }
        public UserNickname Nickname { get; }

        private UserProfile(
            UserId id,
            UserNickname nickname)
        {
            Id = id;
            Nickname = nickname;
        }

        public static UserProfile CreateNew(
            UserId id,
            UserNickname nickname)
        {
            return new UserProfile(
                id: id ?? throw new DomainException("ユーザーIDを設定してください。"),
                nickname: nickname ?? throw new DomainException("ニックネームを設定してください。"));
        }

        public static UserProfile CreateFromRepository(
            UserId id,
            UserNickname nickname)
        {
            return new UserProfile(
                id: id,
                nickname: nickname);
        }
    }
}