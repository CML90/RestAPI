namespace TodoApi.Models.Todo;

/// <summary>
/// This class represents a Todo item in the application. It is more secure, as it 
/// hides the Secret value.
/// </summary>
public class TodoItemDTO
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}