using Domain.Models.Users;
using Domain.Models.Users.UserProfiles;
using System;

namespace Test.Helpers
{
    internal class UserGenerator
    {
        public static User Generate(
            string? id = null,
            string? name = null,
            string? email = null,
            string? nickname = null,
            DateTime? registeredDateTime = null,
            DateTime? updatedDateTime = null,
            UserStatus status = UserStatus.有効)
        {
            return User.CreateFromRepository(
                id: id is null ? new UserId(Guid.NewGuid().ToString("D")) : new UserId(id),
                name: name is null ? new Username("username") : new Username(name),
                email: email is null ? new UserEmailAddress("user@test.com") : new UserEmailAddress(email),
                nickname: nickname is null ? new UserNickname("ニックネーム") : new UserNickname(nickname),
                registeredDateTime: registeredDateTime ?? DateTime.Now,
                updatedDateTime: updatedDateTime ?? DateTime.Now,
                status: status);
        }
    }
}
