using System.Net;
using System.Transactions;
using Domain.Models.Todos;
using Domain.Models.Users;
using UseCase.Shared;

namespace UseCase.Todos.Create
{
    public class TodoCreateUseCase
    {
        private readonly ITodoRepository _todoRepository;

        public TodoCreateUseCase(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<TodoCreateResult> ExecuteAsync(TodoCreateCommand command)
        {
            using var ts = new TransactionScope();

            try
            {
                var todo = Todo.CreateNew(
                    title: new TodoTitle(command.Title),
                    description: !string.IsNullOrWhiteSpace(command.Description)
                        ? new TodoDescription(command.Description)
                        : null,
                    ownerId: new UserId(command.UserSession.Id));

                await _todoRepository.SaveAsync(todo);

                ts.Complete();

                return new TodoCreateResult(todo.Id.Value);
            }
            catch (Exception e)
            {
                ts.Dispose();

                Console.WriteLine(e);
                throw new UseCaseException("TODOの作成に失敗しました。", (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
