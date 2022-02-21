using Domain.Models.Shared;
using Domain.Models.Users.UserProfiles;
using NUnit.Framework;

namespace Test.Domain.Models.Users.UserProfiles
{
    public class UserNicknameTest
    {
        private const string UserNickname51Chars = "吾輩は猫である。名前はまだ無い。どこで生れたかとんと見当がつかぬ。何でも薄暗いじめじめした所でニャーニ";

        [Test]
        public void 引数に50文字以内の文字列を渡すとインスタンスが生成される()
        {
            const string userNicknameValue = "ニックネーム";
            var userNickname = new UserNickname(userNicknameValue);

            Assert.That(userNickname.Value, Is.EqualTo(userNicknameValue));
        }

        [TestCase(UserNickname51Chars, TestName = "引数に51文字以上の文字列を渡すと例外が発生する")]
        [TestCase("", TestName = "引数に空の文字列を渡すと例外が発生する")]
        [Test]
        public void 引数に不正な文字列を渡すと例外が発生する(string nickname)
        {
            Assert.That(
                () => new UserNickname(nickname),
                Throws.TypeOf<DomainException>());
        }
    }
}
