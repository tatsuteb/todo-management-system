using Domain.Models.Shared;
using Domain.Models.Todos;
using NUnit.Framework;

namespace Test.Domain.Models.Todos
{
    public class TodoDescriptionTest
    {
        [Test]
        public void 引数に300文字以下の文字列を渡すとインスタンスが生成される()
        {
            const string value = "テスト";

            var description = new TodoDescription(value);

            Assert.That(description.Value, Is.EqualTo(value));
        }

        [Test]
        public void 引数に301文字以上の文字列を渡すと例外が発生する()
        {
            // 301文字
            const string value = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque finibus ante in arcu pellentesque, et semper purus rhoncus. Proin tincidunt lacus finibus gravida commodo. Vestibulum sollicitudin libero varius, facilisis purus aliquam, rhoncus est. Praesent placerat, turpis quis sagittis turpis.";

            Assert.That(
                () => new TodoDescription(value),
                Throws.TypeOf<DomainException>());
        }
    }
}
