using System.Net;
using System.Transactions;
using Domain.Models.Todos;
using UseCase.Shared;

namespace UseCase.Todos.UpdateStatus
{
    public class TodoUpdateStatusUseCase
    {
        private readonly ITodoRepository _todoRepository;

        public TodoUpdateStatusUseCase(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task ExecuteAsync(TodoUpdateStatusCommand command)
        {
            using var ts = new TransactionScope();

            try
            {
                var todo = await _todoRepository.FindAsync(new TodoId(command.Id));

                if (todo == null)
                {
                    throw new UseCaseException("指定されたTODOが見つかりません。");
                }

                if (todo.OwnerId.Value != command.UserSession.Id)
                {
                    throw new UseCaseException("指定されたTODOのステータスを更新する権限がありません。");
                }

                todo.UpdateStatus((TodoStatus) command.Status);

                await _todoRepository.SaveAsync(todo);

                ts.Complete();
            }
            catch (Exception e)
            {
                ts.Dispose();

                Console.WriteLine(e);
                throw new UseCaseException("TODOステータスの更新に失敗しました。", (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
