using TodoApi.Models.Todo;

namespace TodoApi.Models.User;

/// <summary>
/// This class represents a User of the application. 
/// </summary>
public class UserItem
{
    public long Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public ICollection<TodoItem> Todos { get; set; } = new List<TodoItem>();
    public bool Banned { get; set; } = false;
}