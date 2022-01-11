using UseCase.Shared;

namespace UseCase.Todos.Search
{
    public class TodoSearchCommand
    {
        public UserSession UserSession { get; }
        public string? Keyword { get; }
        public int[] Statuses { get; }
        public int PageNum { get; }
        public int PageSize { get; }

        public TodoSearchCommand(
            UserSession userSession,
            string? keyword = null,
            IEnumerable<int>? statuses = null,
            int pageNum = 1, 
            int pageSize = 30)
        {
            UserSession = userSession;
            Keyword = keyword;
            Statuses = statuses?.ToArray() ?? Array.Empty<int>();
            PageNum = pageNum;
            PageSize = pageSize;
        }
    }
}