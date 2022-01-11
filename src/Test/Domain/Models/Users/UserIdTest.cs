using System;
using Domain.Models.Shared;
using Domain.Models.Users;
using NUnit.Framework;

namespace Test.Domain.Models.Users
{
    internal class UserIdTest
    {
        [Test]
        public void 引数にハイフン付きGUIDを渡すとユーザーIDインスタンスが生成される()
        {
            var guid = Guid.NewGuid().ToString("D");

            var userId = new UserId(guid);

            Assert.That(userId.Value, Is.EqualTo(guid));
        }

        [Test]
        public void 引数にハイフン付きGUID以外の文字列を渡すと例外が発生する()
        {
            var guid = Guid.NewGuid().ToString("N");

            Assert.That(
                () => new UserId(guid),
                Throws.TypeOf<DomainException>());
        }
    }
}
