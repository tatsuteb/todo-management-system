using Domain.Models.Shared;
using Domain.Models.Users.UserProfiles;
using NUnit.Framework;

namespace Test.Domain.Models.Users.UserProfiles
{
    public class UserNicknameTest
    {
        [Test]
        public void 引数に50文字以内の文字列を渡すとインスタンスが生成される()
        {
            const string userNicknameValue = "ニックネーム";
            var userNickname = new UserNickname(userNicknameValue);

            Assert.That(userNickname.Value, Is.EqualTo(userNicknameValue));
        }

        [Test]
        public void 引数に空の文字列を渡すと例外が発生する()
        {
            Assert.That(
                () => new UserNickname(""),
                Throws.TypeOf<DomainException>());
        }

        [Test]
        public void 引数に51文字以上の文字列を渡すと例外が発生する()
        {
            // 51文字
            const string userIntroductionValue = "吾輩は猫である。名前はまだ無い。どこで生れたかとんと見当がつかぬ。何でも薄暗いじめじめした所でニャーニ";
            Assert.That(
                () => new UserNickname(userIntroductionValue),
                Throws.TypeOf<DomainException>());
        }
    }
}
