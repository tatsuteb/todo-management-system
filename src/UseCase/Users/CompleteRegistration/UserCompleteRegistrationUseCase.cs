using Domain.Models.Users;
using UseCase.Shared;

namespace UseCase.Users.CompleteRegistration
{
    public class UserCompleteRegistrationUseCase
    {
        private readonly IUserRepository _userRepository;

        public UserCompleteRegistrationUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ExecuteAsync(UserCompleteRegistrationCommand command)
        {
            var user = await _userRepository.FindAsync(new UserId(command.UserSession.Id));

            if (user is null)
            {
                throw new UseCaseException("指定されたユーザーが見つかりません。");
            }

            user.ChangeStatus(UserStatus.有効);

            await _userRepository.SaveAsync(user);
        }
    }
}
