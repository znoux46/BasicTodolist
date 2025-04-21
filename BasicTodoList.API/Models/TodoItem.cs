namespace BasicTodoList.API.Models
{
    public class TodoItem
    {
        // TodoItem Entity
        public int Id { get; set; }
        public string? Task { get; set; }
        public bool IsCompleted { get; set; }
    }
}
