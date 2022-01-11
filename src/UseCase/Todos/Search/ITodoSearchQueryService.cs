namespace UseCase.Todos.Search
{
    public interface ITodoSearchQueryService
    {
        Task<TodoSearchResult> ExecuteAsync(TodoSearchCommand command);
    }
}
