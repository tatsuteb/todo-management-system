using Domain.Models.Shared;
using Domain.Models.Todos;
using NUnit.Framework;
using System;

namespace Test.Domain.Models.Todos
{
    public class TodoIdTest
    {
        [Test]
        public void 引数に16桁の半角英数字を渡すとインスタンスが生成される()
        {
            var value = Guid.NewGuid().ToString("N")[..16];

            var todoId = new TodoId(value);

            Assert.That(todoId.Value, Is.EqualTo(value));
        }

        [Test]
        public void 引数に16桁以外の半角英数字を渡すと例外が発生する()
        {
            var underValue = Guid.NewGuid().ToString("N")[..15];
            var overValue = Guid.NewGuid().ToString("N")[..17];

            Assert.That(
                () => new TodoId(underValue),
                Throws.TypeOf<DomainException>());
            Assert.That(
                () => new TodoId(overValue),
                Throws.TypeOf<DomainException>());
        }
    }
}
