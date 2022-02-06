using System.Net;
using System.Transactions;
using Domain.Models.Todos;
using UseCase.Shared;

namespace UseCase.Todos.Delete
{
    public class TodoDeleteUseCase
    {
        private readonly ITodoRepository _todoRepository;

        public TodoDeleteUseCase(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task ExecuteAsync(TodoDeleteCommand command)
        {
            using var ts = new TransactionScope();

            try
            {
                var todo = await _todoRepository.FindAsync(new TodoId(command.Id));

                if (todo is null)
                {
                    throw new UseCaseException("指定されたTODOが見つかりません。");
                }

                // NOTE: ドメインルール？
                if (todo.OwnerId.Value != command.UserSession.Id)
                {
                    throw new UseCaseException("指定されたTODOを削除する権限がありません。");
                }

                todo.Delete();

                await _todoRepository.SaveAsync(todo);

                ts.Complete();
            }
            catch (Exception e)
            {
                ts.Dispose();

                Console.WriteLine(e);
                throw new UseCaseException("TODOの削除に失敗しました。", (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
