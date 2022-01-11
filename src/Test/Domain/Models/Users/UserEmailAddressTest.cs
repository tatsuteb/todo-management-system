using Domain.Models.Shared;
using Domain.Models.Users;
using NUnit.Framework;

namespace Test.Domain.Models.Users
{
    public class UserEmailAddressTest
    {
        [Test]
        public void 引数にメールアドレス形式の文字列を渡すとインスタンスが生成される()
        {
            const string emailValue = "test-test_test123@example.com";
            
            var email = new UserEmailAddress(emailValue);

            Assert.That(email.Value, Is.EqualTo(emailValue));
        }

        [Test]
        public void 引数にメールアドレス形式の以外の文字列を渡すと例外が発生する()
        {
            const string emailValue = "invalid email address";

            Assert.That(
                () => new UserEmailAddress(emailValue),
                Throws.TypeOf<DomainException>());
        }
    }
}
