using System;
using System.Threading.Tasks;
using Domain.Models.Users;
using Infrastructure.Users;
using NUnit.Framework;
using Test.Helpers;
using Test.Shared;
using UseCase.Shared;
using UseCase.Users.GetSignInInfo;

namespace Test.Infrastructure.Users
{
    public class UserGetSignInInfoQueryServiceTest : UseDbContextTestBase
    {
        private readonly IUserGetSignInInfoQueryService _userGetSignInInfoQueryService;
        private readonly IUserRepository _userRepository;

        public UserGetSignInInfoQueryServiceTest()
        {
            _userGetSignInInfoQueryService = new UserGetSignInInfoQueryService(TestDbContext);
            _userRepository = new UserRepository(TestDbContext);
        }

        [Test]
        public async Task 引数にセッション情報から取得したIDを渡すとサインイン情報を返す()
        {
            // 準備
            var user = UserGenerator.Generate();
            await _userRepository.SaveAsync(user);

            // 実行
            var command = new UserGetSignInInfoCommand(
                userSession: new UserSession(user.Id.Value));
            var result = await _userGetSignInInfoQueryService.ExecuteAsync(command);

            // 検証
            Assert.That(result.SignInInfo, Is.Not.Null);
            Assert.That(result.SignInInfo?.Id, Is.EqualTo(user.Id.Value));
            Assert.That(result.SignInInfo?.Name, Is.EqualTo(user.Name.Value));
            Assert.That(result.SignInInfo?.Nickname, Is.EqualTo(user.Profile.Nickname.Value));
        }

        [Test]
        public async Task 引数に不正なセッション情報が渡された場合nullを返す()
        {
            // 実行
            var command = new UserGetSignInInfoCommand(
                userSession: new UserSession(Guid.NewGuid().ToString("D")));
            var result = await _userGetSignInInfoQueryService.ExecuteAsync(command);

            // 検証
            Assert.That(result.SignInInfo, Is.Null);
        }
    }
}
