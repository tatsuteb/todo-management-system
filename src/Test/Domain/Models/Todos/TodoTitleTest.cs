using Domain.Models.Shared;
using Domain.Models.Todos;
using NUnit.Framework;

namespace Test.Domain.Models.Todos
{
    public class TodoTitleTest
    {
        [Test]
        public void 引数に50文字以下の文字列を渡すとインスタンスが生成される()
        {
            const string value = "テスト";

            var title = new TodoTitle(value);

            Assert.That(title.Value, Is.EqualTo(value));
        }

        [Test]
        public void 引数に51文字以上の文字列を渡すと例外が発生する()
        {
            // 51文字
            const string value = "Lorem ipsum dolor sit amet, consectetur erat curae.";

            Assert.That(
                () => new TodoTitle(value),
                Throws.TypeOf<DomainException>());
        }
    }
}
