namespace WebClient.Models.Todos
{
    public class TodoDetailsViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime? BeginDateTime { get; set; }
        public DateTime? DueDateTime { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
