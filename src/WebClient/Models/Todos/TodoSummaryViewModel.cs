using UseCase.Todos.Search;

namespace WebClient.Models.Todos
{
    public class TodoSummaryViewModel
    {
        public string Id { get; }
        public string Title { get; }
        public string StatusString { get; }

        public TodoSummaryViewModel(TodoSummaryData data)
        {
            Id = data.Id;
            Title = data.Title;
            StatusString = data.StatusString;
        }
    }
}
