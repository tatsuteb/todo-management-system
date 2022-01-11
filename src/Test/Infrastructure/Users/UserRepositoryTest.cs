using System.Threading.Tasks;
using Domain.Models.Users;
using Infrastructure.Users;
using NUnit.Framework;
using Test.Helpers;
using Test.Shared;

namespace Test.Infrastructure.Users
{
    public class UserRepositoryTest : UseDbContextTestBase
    {
        private readonly IUserRepository _userRepository;

        public UserRepositoryTest()
        {
            _userRepository = new UserRepository(TestDbContext);
        }

        [Test]
        public async Task 引数にユーザーモデルを渡すとDBに保存される()
        {
            // 準備
            var user = UserGenerator.Generate();
            
            // 実行
            await _userRepository.SaveAsync(user);

            // 検証
            var userDataModel = await TestDbContext.Users
                .FindAsync(user.Id.Value);
            Assert.That(userDataModel, Is.Not.Null);
            Assert.That(userDataModel?.Id, Is.EqualTo(user.Id.Value));
            Assert.That(userDataModel?.Name, Is.EqualTo(user.Name.Value));
            Assert.That(userDataModel?.Email, Is.EqualTo(user.Email.Value));
            Assert.That(userDataModel?.Status, Is.EqualTo((int) user.Status));
            Assert.That(userDataModel?.RegisteredDateTime, Is.EqualTo(user.RegisteredDateTime));
            Assert.That(userDataModel?.UpdatedDateTime, Is.EqualTo(user.UpdatedDateTime));

            var userProfileDataModel = await TestDbContext.UserProfiles
                .FindAsync(user.Id.Value);
            Assert.That(userProfileDataModel, Is.Not.Null);
            Assert.That(userProfileDataModel?.UserId, Is.EqualTo(user.Id.Value));
            Assert.That(userProfileDataModel?.Nickname, Is.EqualTo(user.Profile.Nickname.Value));
        }

        [Test]
        public async Task 引数に既存のユーザーモデルを更新して渡すとDBの情報が更新される()
        {
            // 準備
            var user = UserGenerator.Generate(status: UserStatus.有効);
            await _userRepository.SaveAsync(user);

            // 実行
            user.ChangeStatus(UserStatus.退会);
            await _userRepository.SaveAsync(user);

            // 検証
            var foundUser = await _userRepository.FindAsync(user.Id);
            Assert.That(foundUser?.Status, Is.EqualTo(UserStatus.退会));
        }

        [Test]
        public async Task 引数にユーザーIDを渡すとDBの情報からユーザーモデルが作成されて返される()
        {
            // 準備
            var user = UserGenerator.Generate();
            await _userRepository.SaveAsync(user);

            // 実行
            var foundUser = await _userRepository.FindAsync(user.Id);

            // 検証
            Assert.That(foundUser, Is.Not.Null);
            Assert.That(foundUser?.Id, Is.EqualTo(user.Id));
            Assert.That(foundUser?.Name, Is.EqualTo(user.Name));
            Assert.That(foundUser?.Email, Is.EqualTo(user.Email));
            Assert.That(foundUser?.Profile.Nickname, Is.EqualTo(user.Profile.Nickname));
            Assert.That(foundUser?.RegisteredDateTime, Is.EqualTo(user.RegisteredDateTime));
            Assert.That(foundUser?.UpdatedDateTime, Is.EqualTo(user.UpdatedDateTime));
            Assert.That(foundUser?.Status, Is.EqualTo(user.Status));
        }

        [Test]
        public async Task 引数にユーザーIDを渡すと存在しているかどうか判定する()
        {
            // 準備
            var existUser = UserGenerator.Generate(name: "ExistUser");
            await _userRepository.SaveAsync(existUser);
            
            var newUser = UserGenerator.Generate(name: "NewUser");

            // 実行
            var exist = await _userRepository.ExistsById(existUser.Id);
            var exist2 = await _userRepository.ExistsById(newUser.Id);

            // 検証
            Assert.That(exist, Is.True);
            Assert.That(exist2, Is.False);
        }

        [Test]
        public async Task 引数にユーザー名を渡すと存在しているかどうか判定する()
        {
            // 準備
            var existUser = UserGenerator.Generate(name: "ExistUser");
            await _userRepository.SaveAsync(existUser);

            var newUser = UserGenerator.Generate(name: "NewUser");

            // 実行
            var exist = await _userRepository.ExistsByName(existUser.Name);
            var exist2 = await _userRepository.ExistsByName(newUser.Name);

            // 検証
            Assert.That(exist, Is.True);
            Assert.That(exist2, Is.False);
        }

        [Test]
        public async Task 引数にメールアドレスを渡すと存在しているかどうか判定する()
        {
            // 準備
            var existUser = UserGenerator.Generate(
                name: "ExistUser",
                email: "exist_user@test.com");
            await _userRepository.SaveAsync(existUser);

            var newUser = UserGenerator.Generate(
                name: "NewUser",
                email: "new_user@test.com");

            // 実行
            var exist = await _userRepository.ExistsByEmail(existUser.Email);
            var exist2 = await _userRepository.ExistsByEmail(newUser.Email);

            // 検証
            Assert.That(exist, Is.True);
            Assert.That(exist2, Is.False);
        }
    }
}
