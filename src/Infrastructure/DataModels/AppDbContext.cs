using Infrastructure.DataModels.Todos;
using Infrastructure.DataModels.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataModels
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserDataModel> Users { get; set; }
        public DbSet<UserProfileDataModel> UserProfiles { get; set; }
        public DbSet<TodoDataModel> Todos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
