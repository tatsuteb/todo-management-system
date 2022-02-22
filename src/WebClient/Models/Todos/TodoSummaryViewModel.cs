using UseCase.Todos.Search;

namespace WebClient.Models.Todos
{
    public class TodoSummaryViewModel
    {
        public string Id { get; }
        public string Title { get; }
        public DateTime? BeginDateTime { get; }
        public DateTime? DueDateTime { get; }
        public string StatusString { get; }

        public TodoSummaryViewModel(TodoSummaryData data)
        {
            Id = data.Id;
            Title = data.Title;
            BeginDateTime = data.BeginDateTime;
            DueDateTime = data.DueDateTime;
            StatusString = data.StatusString;
        }
    }
}
