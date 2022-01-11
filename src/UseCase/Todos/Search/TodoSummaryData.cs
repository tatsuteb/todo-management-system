namespace UseCase.Todos.Search
{
    public class TodoSummaryData
    {
        public string Id { get; }
        public string Title { get; }
        public DateTime CreatedDateTime { get; }
        public DateTime UpdatedDateTime { get; }
        public string StatusString { get; }

        public TodoSummaryData(
            string id, 
            string title, 
            DateTime createdDateTime, 
            DateTime updatedDateTime, 
            string statusString)
        {
            Id = id;
            Title = title;
            CreatedDateTime = createdDateTime;
            UpdatedDateTime = updatedDateTime;
            StatusString = statusString;
        }
    }
}