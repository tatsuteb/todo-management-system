using Microsoft.AspNetCore.Mvc;

namespace WebClient.Models.Todos
{
    public class TodoViewModel
    {
        public TodoSummaryViewModel[] Summaries { get; set; }
        public int Total { get; set; }
        public bool ShowDetails { get; set; }

        #region QueryParameters

        public string Id { get; set; } = string.Empty;
        public string Keyword { get; set; } = string.Empty;
        public bool IsIncompleted { get; set; } = false;
        public bool IsCompleted { get; set; } = false;
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 30;

        #endregion
        
        #region InputModels

        public TodoPostInputModel PostInputModel { get; set; } = new();
        public TodoCompleteInputModel CompleteInputModel { get; set; } = new();
        public TodoDetailsViewModel TodoDetailsViewModel { get; set; } = new();

        #endregion

        [TempData]
        public string ErrorMessage { get; set; } = string.Empty;


        public TodoViewModel()
        {
            Summaries = Array.Empty<TodoSummaryViewModel>();
        }
    }
}
