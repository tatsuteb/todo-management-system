using Domain.Models.Todos;
using Domain.Models.Users;
using Infrastructure.DataModels;
using Infrastructure.DataModels.Todos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Todos
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _dbContext;

        public TodoRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region 更新系

        public async Task SaveAsync(Todo todo)
        {
            var exists = await _dbContext.Todos
                .AnyAsync(x => x.Id == todo.Id.Value);

            if (exists)
            {
                await UpdateAsync(_dbContext, todo);
            }
            else
            {
                await SaveNewAsync(_dbContext, todo);
            }
        }

        private static async Task UpdateAsync(AppDbContext dbContext, Todo todo)
        {
            var foundTodoDataModel = await dbContext.Todos
                .FindAsync(todo.Id.Value);

            if (foundTodoDataModel is null)
            {
                throw new Exception($"TODOデータ不明（ID: {todo.Id.Value}）");
            }

            var todoDataModel = Transfer(foundTodoDataModel, todo);

            dbContext.Todos
                .Update(todoDataModel);

            await dbContext.SaveChangesAsync();
        }

        private static async Task SaveNewAsync(AppDbContext dbContext, Todo todo)
        {
            var foundTodoDataModel = await dbContext.Todos
                .FindAsync(todo.Id.Value);

            if (foundTodoDataModel != null)
            {
                throw new Exception($"TODOデータ重複（ID: {todo.Id.Value}）");
            }

            var todoDataModel = ToDataModel(todo);

            await dbContext.Todos
                .AddAsync(todoDataModel);

            await dbContext.SaveChangesAsync();
        }

        private static TodoDataModel Transfer(
            TodoDataModel todoDataModel,
            Todo todo)
        {
            todoDataModel.Id = todo.Id.Value;
            todoDataModel.Title= todo.Title.Value;
            todoDataModel.Description = todo.Description?.Value;
            todoDataModel.BeginDateTime = todo.BeginDateTime;
            todoDataModel.DueDateTime = todo.DueDateTime;
            todoDataModel.OwnerId = todo.OwnerId.Value;
            todoDataModel.CreatedDateTime = todo.CreatedDateTime;
            todoDataModel.UpdatedDateTime = todo.UpdatedDateTime;
            todoDataModel.Status = (int)todo.Status;
            todoDataModel.IsDeleted = todo.IsDeleted;
            todoDataModel.DeletedDateTime = todo.DeletedDateTime;

            return todoDataModel;
        }

        private static TodoDataModel ToDataModel(Todo todo)
        {
            return new TodoDataModel
                {
                    Id = todo.Id.Value,
                    Title = todo.Title.Value,
                    Description = todo.Description?.Value,
                    BeginDateTime = todo.BeginDateTime,
                    DueDateTime = todo.DueDateTime,
                    OwnerId = todo.OwnerId.Value,
                    CreatedDateTime = todo.CreatedDateTime,
                    UpdatedDateTime = todo.UpdatedDateTime,
                    Status = (int)todo.Status,
                    IsDeleted = todo.IsDeleted,
                    DeletedDateTime = todo.DeletedDateTime
                };
        }

        #endregion

        #region 参照系

        public async Task<Todo?> FindAsync(TodoId id)
        {
            var todoDataModel = await _dbContext.Todos
                .FindAsync(id.Value);

            return todoDataModel != null
                ? ToModel(todoDataModel)
                : null;
        }

        private static Todo ToModel(TodoDataModel todoDataModel)
        {
            return Todo.CreateFromRepository(
                id: new TodoId(todoDataModel.Id),
                title: new TodoTitle(todoDataModel.Title),
                description: !string.IsNullOrWhiteSpace(todoDataModel.Description)
                    ? new TodoDescription(todoDataModel.Description)
                    : null,
                beginDateTime: todoDataModel.BeginDateTime,
                dueDateTime: todoDataModel.DueDateTime,
                ownerId: new UserId(todoDataModel.OwnerId),
                createdDateTime: todoDataModel.CreatedDateTime,
                updatedDateTime: todoDataModel.UpdatedDateTime,
                status: (TodoStatus)todoDataModel.Status,
                isDeleted: todoDataModel.IsDeleted,
                deletedDateTime: todoDataModel.DeletedDateTime);
        }

        #endregion
    }
}
