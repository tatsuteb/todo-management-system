namespace Domain.Models.Users.Specifications
{
    public class UserDuplicationSpecification
    {
        private readonly IUserRepository _userRepository;

        public UserDuplicationSpecification(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> IsSatisfiedByAsync(User user)
        {
            return await _userRepository.ExistsById(user.Id) ||
                   await _userRepository.ExistsByName(user.Name) ||
                   await _userRepository.ExistsByEmail(user.Email);
        }
    }
}