using TodoApi.Models.Todo;
namespace TodoApi.Models.User;

/// <summary>
/// This class represents a User in the application. 
/// It hides the User's firstname and lastname.
/// <summary>
public class UserItemDTO
{
    public long Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<TodoItemDTO> Todo { get; set; } = new List<TodoItemDTO>();
}