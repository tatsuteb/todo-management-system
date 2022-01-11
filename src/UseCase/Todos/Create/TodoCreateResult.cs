namespace UseCase.Todos.Create
{
    public class TodoCreateResult
    {
        public string Id { get; }

        public TodoCreateResult(string id)
        {
            Id = id;
        }
    }
}