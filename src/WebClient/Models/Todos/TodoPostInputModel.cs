using System.ComponentModel.DataAnnotations;
using Domain.Models.Todos;

namespace WebClient.Models.Todos
{
    public class TodoPostInputModel
    {
        [StringLength(TodoTitle.MaxTodoTitleLength)]
        public string Title { get; set; } = "";

        [StringLength(TodoDescription.MaxTodoDescriptionLength)]

        public string Description { get; set; } = "";

        public bool IsComplete { get; set; }
    }
}
