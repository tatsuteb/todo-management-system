namespace UseCase.Todos.Search
{
    public class TodoSummaryData
    {
        public string Id { get; }
        public string Title { get; }
        public DateTime? BeginDateTime { get; }
        public DateTime? DueDateTime { get; }
        public DateTime CreatedDateTime { get; }
        public DateTime UpdatedDateTime { get; }
        public string StatusString { get; }

        public TodoSummaryData(
            string id, 
            string title,
            DateTime? beginDateTime,
            DateTime? dueDateTime,
            DateTime createdDateTime, 
            DateTime updatedDateTime, 
            string statusString)
        {
            Id = id;
            Title = title;
            BeginDateTime = beginDateTime;
            DueDateTime = dueDateTime;
            CreatedDateTime = createdDateTime;
            UpdatedDateTime = updatedDateTime;
            StatusString = statusString;
        }
    }
}