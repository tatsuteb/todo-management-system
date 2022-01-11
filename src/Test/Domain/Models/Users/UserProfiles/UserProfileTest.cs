using Domain.Models.Users;
using Domain.Models.Users.UserProfiles;
using NUnit.Framework;
using System;

namespace Test.Domain.Models.Users.UserProfiles
{
    public class UserProfileTest
    {
        [Test]
        public void 新しくユーザープロフィールを作成すると引数の値でインスタンスが生成される()
        {
            var userId = new UserId(Guid.NewGuid().ToString("D"));
            var nickname = new UserNickname("ニックネーム");

            var userProfile = UserProfile.CreateNew(
                id: userId,
                nickname: nickname);

            Assert.That(userProfile.Id, Is.EqualTo(userId));
            Assert.That(userProfile.Nickname, Is.EqualTo(nickname));
        }

        [Test]
        public void リポジトリからユーザープロフィールを作成すると引数の値でインスタンスが生成される()
        {
            var userId = new UserId(Guid.NewGuid().ToString("D"));
            var nickname = new UserNickname("ニックネーム");

            var userProfile = UserProfile.CreateFromRepository(
                id: userId,
                nickname: nickname);

            Assert.That(userProfile.Id, Is.EqualTo(userId));
            Assert.That(userProfile.Nickname, Is.EqualTo(nickname));
        }
    }
}
