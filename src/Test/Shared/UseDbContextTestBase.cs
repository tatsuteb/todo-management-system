using System.Threading.Tasks;
using Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Test.Shared
{
    public abstract class UseDbContextTestBase
    {
        protected AppDbContext TestDbContext;

        protected UseDbContextTestBase()
        {
            TestDbContext = new AppDbContext(
                options: new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase("todo_management_system_test_db")
                    .Options);

            TestDbContext.Database.EnsureCreated();
        }

        [SetUp]
        public virtual async Task SetupAsync()
        {
            // Users
            TestDbContext.Users.RemoveRange(TestDbContext.Users);
            TestDbContext.UserProfiles.RemoveRange(TestDbContext.UserProfiles);
            // Todos
            TestDbContext.Todos.RemoveRange(TestDbContext.Todos);

            await TestDbContext.SaveChangesAsync();
        }
    }
}
