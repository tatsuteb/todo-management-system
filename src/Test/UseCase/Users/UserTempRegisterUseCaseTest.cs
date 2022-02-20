using System;
using System.Threading.Tasks;
using System.Transactions;
using Domain.Models.Users;
using Infrastructure.Users;
using NUnit.Framework;
using Test.Helpers;
using Test.Shared;
using UseCase.Shared;
using UseCase.Users.TempRegister;

namespace Test.UseCase.Users
{
    public class UserTempRegisterUseCaseTest : UseDbContextTestBase
    {
        private readonly UserTempRegisterUseCase _userTempRegisterUseCase;
        private readonly IUserRepository _userRepository;

        public UserTempRegisterUseCaseTest()
        {
            _userRepository = new UserRepository(TestDbContext);
            _userTempRegisterUseCase = new UserTempRegisterUseCase(_userRepository);
        }

        [Test]
        public async Task 引数にユーザー名などを渡すとユーザー情報が保存される()
        {
            // 準備
            var userId = Guid.NewGuid().ToString("D");
            const string username = "username";
            const string email = "username@test.com";
            const string nickname = "ニックネーム";

            // 実行
            var command = new UserTempRegisterCommand(
                userSession: new UserSession(userId),
                name: username,
                email: email,
                nickname: nickname);
            var result = await _userTempRegisterUseCase.ExecuteAsync(command);

            // 検証
            Assert.That(result.Id, Is.EqualTo(userId));
            var exists= await _userRepository.ExistsById(new UserId(result.Id));
            Assert.That(exists, Is.True);
        }

        [Test]
        [TestCase("username1", "unique_mail@test.com", TestName = "既存のユーザーとユーザー名が重複すると例外が発生する")]
        [TestCase("uniqueUsername", "username2@test.com", TestName = "既存のユーザーとメールアドレスが重複すると例外が発生する")]
        public async Task 既存のユーザーと重複すると例外が発生する(string testUsername, string testEmail)
        {
            using var ts = new TransactionScope(TransactionScopeOption.Suppress);
            
            // 準備
            var existUser1 = UserGenerator.Generate(
                name: "username1",
                email: "username1@test.com");
            var existUser2 = UserGenerator.Generate(
                name: "username2",
                email: "username2@test.com");
            await _userRepository.SaveAsync(existUser1);
            await _userRepository.SaveAsync(existUser2);

            // 実行・検証
            var command = new UserTempRegisterCommand(
                userSession: new UserSession(Guid.NewGuid().ToString("D")),
                name: testUsername,
                email: testEmail,
                nickname: "ニックネーム");
            Assert.That(
                async () => await _userTempRegisterUseCase.ExecuteAsync(command),
                Throws.TypeOf<UseCaseException>());
        }
    }
}
