using Domain.Models.Users;
using Domain.Models.Users.UserProfiles;
using NUnit.Framework;
using System;
using Domain.Models.Shared;
using Test.Helpers;

namespace Test.Domain.Models.Users
{
    public class UserTest
    {
        [Test]
        public void 新しくユーザーを作成すると一般ユーザーの仮登録状態でインスタンスが生成される()
        {
            var userId = new UserId(Guid.NewGuid().ToString("D"));
            var userName = new Username("username");
            var userEmail = new UserEmailAddress("user@text.com");
            var nickname = new UserNickname("ニックネーム");

            var user = User.CreateNew(
                id: userId,
                name: userName,
                email: userEmail,
                nickname: nickname);

            Assert.That(user.Id, Is.EqualTo(userId));
            Assert.That(user.Name, Is.EqualTo(userName));
            Assert.That(user.Email, Is.EqualTo(userEmail));
            Assert.That(user.Profile.Id, Is.EqualTo(userId));
            Assert.That(user.Profile.Nickname, Is.EqualTo(nickname));
            Assert.That(user.Status, Is.EqualTo(UserStatus.仮登録));
        }

        [Test]
        public void リポジトリからユーザーを作成すると引数の値でインスタンスが生成される()
        {
            var userId = new UserId(Guid.NewGuid().ToString("D"));
            var userName = new Username("username");
            var userEmail = new UserEmailAddress("user@text.com");
            var nickname = new UserNickname("ニックネーム");
            const UserStatus userStatus = UserStatus.有効;
            var now = DateTime.Now;

            var user = User.CreateFromRepository(
                id: userId,
                name: userName,
                email: userEmail,
                nickname: nickname,
                status: userStatus,
                registeredDateTime: now,
                updatedDateTime: now);

            Assert.That(user.Id, Is.EqualTo(userId));
            Assert.That(user.Name, Is.EqualTo(userName));
            Assert.That(user.Email, Is.EqualTo(userEmail));
            Assert.That(user.Profile.Id, Is.EqualTo(userId));
            Assert.That(user.Profile.Nickname, Is.EqualTo(nickname));
            Assert.That(user.Status, Is.EqualTo(userStatus));
            Assert.That(user.RegisteredDateTime, Is.EqualTo(now));
            Assert.That(user.UpdatedDateTime, Is.EqualTo(now));
        }

        [Test]
        public void 引数にステータスを渡すとユーザーのステータスが変更される()
        {
            var user = UserGenerator.Generate(status: UserStatus.仮登録);

            user.ChangeStatus(UserStatus.有効);

            Assert.That(user.Status, Is.EqualTo(UserStatus.有効));
        }

        [Test]
        public void 削除されたユーザーのステータスを変更しようとすると例外が発生する()
        {
            var user = UserGenerator.Generate(status: UserStatus.退会);

            Assert.That(
                () => user.ChangeStatus(UserStatus.有効),
                Throws.TypeOf<DomainException>());
        }
    }
}
