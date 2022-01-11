namespace Domain.Models.Users
{
    public interface IUserRepository
    {
        Task SaveAsync(User user);
        
        Task<User?> FindAsync(UserId id);

        Task<bool> ExistsById(UserId id);
        Task<bool> ExistsByName(Username name);
        Task<bool> ExistsByEmail(UserEmailAddress email);
    }
}
