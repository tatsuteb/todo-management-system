namespace UseCase.Todos.Get
{
    public class TodoGetResult
    {
        public TodoData Todo { get; }

        public TodoGetResult(TodoData todo)
        {
            Todo = todo;
        }
    }
}