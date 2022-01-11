using System.Threading.Tasks;
using Domain.Models.Users;
using Domain.Models.Users.Specifications;
using Infrastructure.Users;
using NUnit.Framework;
using Test.Helpers;
using Test.Shared;

namespace Test.Domain.Models.Users.Specifications
{
    public class UserDuplicationSpecificationTest : UseDbContextTestBase
    {
        private readonly IUserRepository _userRepository;

        public UserDuplicationSpecificationTest()
        {
            _userRepository = new UserRepository(TestDbContext);
        }

        [Test]
        public async Task 既存のユーザーが存在するかどうか判定する()
        {
            // 準備
            var user1 = UserGenerator.Generate(name: "user1");
            var user2 = UserGenerator.Generate(name: "user2");
            await _userRepository.SaveAsync(user1);
            await _userRepository.SaveAsync(user2);
            var spec = new UserDuplicationSpecification(_userRepository);

            // 実行
            var exists1 = await spec.IsSatisfiedByAsync(user1);
            var exists2 = await spec.IsSatisfiedByAsync(user2);

            // 検証
            Assert.That(exists1, Is.True);
            Assert.That(exists2, Is.True);
        }
    }
}