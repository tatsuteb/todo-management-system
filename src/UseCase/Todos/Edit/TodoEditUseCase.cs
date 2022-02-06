﻿using System.Net;
using System.Transactions;
using Domain.Models.Todos;
using UseCase.Shared;

namespace UseCase.Todos.Edit
{
    public class TodoEditUseCase
    {
        private readonly ITodoRepository _todoRepository;

        public TodoEditUseCase(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task ExecuteAsync(TodoEditCommand command)
        {
            using var ts = new TransactionScope();

            try
            {
                var todo = await _todoRepository.FindAsync(new TodoId(command.Id));

                if (todo is null)
                {
                    throw new UseCaseException("指定されたTODOが見つかりません。");
                }

                if (todo.OwnerId.Value != command.UserSession.Id)
                {
                    throw new UseCaseException("指定されたTODOを編集する権限がありません。");
                }

                todo.Edit(
                    title: new TodoTitle(command.Title),
                    description: !string.IsNullOrWhiteSpace(command.Description)
                        ? new TodoDescription(command.Description)
                        : null);

                await _todoRepository.SaveAsync(todo);

                ts.Complete();
            }
            catch (Exception e)
            {
                ts.Dispose();

                Console.WriteLine(e);
                throw new UseCaseException("TODOの編集に失敗しました。", (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
