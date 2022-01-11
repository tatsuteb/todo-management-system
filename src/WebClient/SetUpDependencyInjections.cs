using Domain.Models.Todos;
using Domain.Models.Users;
using Infrastructure.Todos;
using Infrastructure.Users;
using UseCase.Todos.Create;
using UseCase.Todos.Delete;
using UseCase.Todos.Edit;
using UseCase.Todos.Get;
using UseCase.Todos.Search;
using UseCase.Todos.UpdateStatus;
using UseCase.Users.CompleteRegistration;
using UseCase.Users.GetSignInInfo;
using UseCase.Users.TempRegister;

namespace WebClient
{
    public static class SetUpDependencyInjections
    {
        public static WebApplicationBuilder SetUp(WebApplicationBuilder builder)
        {
            builder.Services
                // UseCase
                .AddTransient<UserTempRegisterUseCase>()
                .AddTransient<UserCompleteRegistrationUseCase>()
                .AddTransient<TodoCreateUseCase>()
                .AddTransient<TodoEditUseCase>()
                .AddTransient<TodoUpdateStatusUseCase>()
                .AddTransient<TodoDeleteUseCase>()
                .AddTransient<TodoGetUseCase>()
                // QueryService
                .AddTransient<IUserGetSignInInfoQueryService, UserGetSignInInfoQueryService>()
                .AddTransient<ITodoSearchQueryService, TodoSearchQueryService>()
                // Repository
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<ITodoRepository, TodoRepository>()
                ;

            return builder;
        }
    }
}
