using Domain.Models.Users;
using Domain.Models.Users.Specifications;
using Domain.Models.Users.UserProfiles;
using System.Transactions;
using UseCase.Shared;

namespace UseCase.Users.TempRegister
{
    public class UserTempRegisterUseCase
    {
        private readonly IUserRepository _userRepository;

        public UserTempRegisterUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserTempRegisterResult> ExecuteAsync(UserTempRegisterCommand command)
        {
            using var ts = new TransactionScope();

            var user = User.CreateNew(
                id: new UserId(command.UserSession.Id),
                name: new Username(command.Name),
                email: new UserEmailAddress(command.Email),
                nickname: new UserNickname(command.Nickname));

            var spec = new UserDuplicationSpecification(_userRepository);
            if (await spec.IsSatisfiedByAsync(user))
            {
                throw new UseCaseException("指定されたユーザーは既に存在しています。");
            }

            await _userRepository.SaveAsync(user);

            ts.Complete();

            return new UserTempRegisterResult(user.Id.Value);
        }
    }
}
