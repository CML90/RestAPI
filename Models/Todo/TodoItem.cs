using TodoApi.Models.User;

namespace TodoApi.Models.Todo;

/// <summary>
/// This class represents a Todo item in the application.
/// </summary>
public class TodoItem
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public string? Secret { get; set; }
    public UserItem? User { get; set; }
}