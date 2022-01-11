using Domain.Models.Users;
using Domain.Models.Users.UserProfiles;
using Infrastructure.DataModels;
using Infrastructure.DataModels.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region 更新系

        public async Task SaveAsync(User user)
        {
            var exists = await _dbContext.Users
                .AnyAsync(x => x.Id == user.Id.Value);

            if (exists)
            {
                await UpdateAsync(_dbContext, user);
            }
            else
            {
                await SaveNewAsync(_dbContext, user);
            }
        }

        private static async Task UpdateAsync(AppDbContext dbContext, User user)
        {
            var foundUserDataModel = await dbContext.Users
                .FindAsync(user.Id.Value);
            
            if (foundUserDataModel is null)
            {
                throw new Exception($"ユーザーデータ不明（ID: {user.Id.Value}）");
            }

            var foundUserProfileDataModel = await dbContext.UserProfiles
                .FindAsync(user.Id.Value);

            if (foundUserProfileDataModel is null)
            {
                throw new Exception($"プロフィールデータ不明（ID: {user.Id.Value}）");
            }

            var (userDataModel, 
                userProfileDataModel) = Transfer(foundUserDataModel, foundUserProfileDataModel, user);

            dbContext.Users
                .Update(userDataModel);
            dbContext.UserProfiles
                .Update(userProfileDataModel);

            await dbContext.SaveChangesAsync();
        }

        private static async Task SaveNewAsync(AppDbContext dbContext, User user)
        {
            var foundUserDataModel = await dbContext.Users
                .FindAsync(user.Id.Value);

            if (foundUserDataModel != null)
            {
                throw new Exception($"ユーザーデータ重複（ID: {user.Id.Value}）");
            }

            var foundUserProfileDataModel = await dbContext.UserProfiles
                .FindAsync(user.Id.Value);

            if (foundUserProfileDataModel != null)
            {
                throw new Exception($"プロフィールデータ重複（ID: {user.Id.Value}）");
            }

            var (userDataModel,
                userProfileDataModel) = ToDataModel(user);

            await dbContext.Users
                .AddAsync(userDataModel);
            await dbContext.UserProfiles
                .AddAsync(userProfileDataModel);

            await dbContext.SaveChangesAsync();
        }

        private static (UserDataModel, UserProfileDataModel) Transfer(
            UserDataModel userDataModel, 
            UserProfileDataModel userProfileDataModel, 
            User user)
        {
            userDataModel.Id = user.Id.Value;
            userDataModel.Name = user.Name.Value;
            userDataModel.Email = user.Email.Value;
            userDataModel.RegisteredDateTime = user.RegisteredDateTime;
            userDataModel.UpdatedDateTime = user.UpdatedDateTime;
            userDataModel.Status = (int)user.Status;

            userProfileDataModel.UserId = user.Id.Value;
            userProfileDataModel.Nickname = user.Profile.Nickname.Value;

            return (userDataModel, userProfileDataModel);
        }

        private static (UserDataModel, UserProfileDataModel) ToDataModel(User user)
        {
            return (
                new UserDataModel
                {
                    Id = user.Id.Value,
                    Name = user.Name.Value,
                    Email = user.Email.Value,
                    RegisteredDateTime = user.RegisteredDateTime,
                    UpdatedDateTime = user.UpdatedDateTime,
                    Status = (int)user.Status
                },
                new UserProfileDataModel
                {
                    UserId = user.Id.Value,
                    Nickname = user.Profile.Nickname.Value
                });
        }

        #endregion


        #region 参照系

        public async Task<User?> FindAsync(UserId id)
        {
            var userDataModel = await _dbContext.Users
                .FindAsync(id.Value);

            if (userDataModel is null)
            {
                return null;
            }

            var userProfileDataModel = await _dbContext.UserProfiles
                .FindAsync(id.Value);

            if (userProfileDataModel is null)
            {
                return null;
            }

            return ToModel(userDataModel, userProfileDataModel);
        }

        private static User ToModel(UserDataModel userDataModel, UserProfileDataModel userProfileDataModel)
        {
            return User.CreateFromRepository(
                id: new UserId(userDataModel.Id),
                name: new Username(userDataModel.Name),
                email: new UserEmailAddress(userDataModel.Email),
                nickname: new UserNickname(userProfileDataModel.Nickname),
                registeredDateTime: userDataModel.RegisteredDateTime,
                updatedDateTime: userDataModel.UpdatedDateTime,
                status: (UserStatus) userDataModel.Status);
        }

        public async Task<bool> ExistsById(UserId id)
        {
            return await _dbContext.Users
                .AnyAsync(x => x.Id == id.Value);
        }

        public async Task<bool> ExistsByName(Username name)
        {
            return await _dbContext.Users
                .AnyAsync(x => x.Name == name.Value);
        }

        public async Task<bool> ExistsByEmail(UserEmailAddress email)
        {
            return await _dbContext.Users
                .AnyAsync(x => x.Email == email.Value);
        }

        #endregion
    }
}
