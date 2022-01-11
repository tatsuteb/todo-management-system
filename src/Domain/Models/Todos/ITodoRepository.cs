namespace Domain.Models.Todos
{
    public interface ITodoRepository
    {
        Task SaveAsync(Todo todo);
        Task<Todo?> FindAsync(TodoId id);
    }
}
