namespace UseCase.Todos.Search
{
    public class TodoSearchResult
    {
        public TodoSummaryData[] Summaries { get; }
        public int Total { get; }
        public int PageNum { get; }
        public int PageSize { get; }

        public TodoSearchResult(
            IEnumerable<TodoSummaryData>? summaries, 
            int total, 
            int pageNum, 
            int pageSize)
        {
            Summaries = summaries?.ToArray() ?? Array.Empty<TodoSummaryData>();
            Total = total;
            PageNum = pageNum;
            PageSize = pageSize;
        }
    }
}