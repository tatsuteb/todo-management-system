using System.Threading.Tasks;
using Domain.Models.Users;
using Infrastructure.Users;
using NUnit.Framework;
using Test.Helpers;
using Test.Shared;
using UseCase.Shared;
using UseCase.Users.CompleteRegistration;

namespace Test.UseCase.Users
{
    public class UserCompleteRegistrationUseCaseTest : UseDbContextTestBase
    {
        private readonly UserCompleteRegistrationUseCase _userCompleteRegistrationUseCase;
        private readonly IUserRepository _userRepository;

        public UserCompleteRegistrationUseCaseTest()
        {
            _userRepository = new UserRepository(TestDbContext);
            _userCompleteRegistrationUseCase = new UserCompleteRegistrationUseCase(_userRepository);
        }

        [Test]
        public async Task 仮登録ユーザーの登録を完了して有効にする()
        {
            // 準備
            var user = UserGenerator.Generate(status: UserStatus.仮登録);
            await _userRepository.SaveAsync(user);

            // 実行
            var command = new UserCompleteRegistrationCommand(
                userSession: new UserSession(user.Id.Value));
            await _userCompleteRegistrationUseCase.ExecuteAsync(command);

            // 検証
            var foundUser = await _userRepository.FindAsync(user.Id);
            Assert.That(foundUser?.Status, Is.EqualTo(UserStatus.有効));
        }
    }
}
