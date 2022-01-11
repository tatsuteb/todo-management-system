namespace WebClient.Models.Todos
{
    public class TodoCompleteInputModel
    {
        public string TodoId { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
        public int Status { get; set; }
    }
}
