using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models.User;
using TodoApi.Models.Todo;

namespace TodoApi.Controllers.User;

[Route("api/[controller]")]
[ApiController]

/// <summary>
/// This Controller class contains the different endpoints for CRUD operations on the Users.
/// </summary>
public class UserItemController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserItemController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    /// <summary>
    /// This endpoint reads all of the existing Users.
    /// </summary>
    public async Task<ActionResult<IEnumerable<UserItemDTO>>> GetUsers()
    {

        return await _context.UserItems
            .Include(u => u.Todos)
            .Select(x => UserItemsDTO(x))
            .ToListAsync();
    }

    [HttpGet("{id}")]
    /// <summary>
    /// This endpoint finds and gets one user.
    /// </summary>
    /// <param name="id">User's primary key</param>
    public async Task<ActionResult<UserItemDTO>> GetIndivUser(long id)
    {
        var userItem = await _context.UserItems.FindAsync(id);

        if (userItem == null)
        {
            return NotFound();
        }

        return UserItemsDTO(userItem);
    }

    [HttpPut("{id}")]
    /// <summary>
    /// This endpoint updates the whole user object.
    /// </summary>
    /// <param name="id">User's primary key</param>
    /// <param name="userDTO">A new user</param>
    public async Task<IActionResult> PutUser(long id, UserItemDTO userDTO)
    {
        if (id != userDTO.Id)
        {
            return BadRequest();
        }

        var userItem = await _context.UserItems.FindAsync(id);
        if (userItem == null)
        {
            return NotFound();
        }

        userItem.LastName = userDTO.LastName;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!UserItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    /// <summary>
    /// This endpoint inserts one user.
    /// </summary>
    /// <param name="userDTO">A new user</param>
    public async Task<ActionResult<UserItemDTO>> PostUser(UserItemDTO userDTO)
    {
        var user = new UserItem
        {
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName,
            Todos = new List<TodoItem>()
        };

        _context.UserItems.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetIndivUser),
            new { id = user.Id },
            UserItemsDTO(user));
    }

    [HttpDelete("{id}")]
    /// <summary>
    /// This endpoint deletes a user.
    /// </summary>
    /// <param name="id">User's primary key</param>
    public async Task<IActionResult> DeleteUser(long id)
    {
        var user = await _context.UserItems.FindAsync();
        if (user == null)
        {
            return NotFound();
        }

        _context.UserItems.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// This method checks if the given user exists.
    /// </summary>
    /// <param name="id">User's primary key</param>
    private bool UserItemExists(long id)
    {
        return _context.UserItems.Any(e => e.Id == id);
    }

    /// <summary>
    /// This method maps the User to the DTO version.
    /// </summary>
    /// <param name="userItem">DTO superset</param>
    private static UserItemDTO UserItemsDTO(UserItem userItem) =>
        new UserItemDTO
        {
            Id = userItem.Id,
            FirstName = userItem.FirstName,
            LastName = userItem.LastName,
            Todo = userItem.Todos.Select(t => ItemToDTO(t)).ToList()
        };

    private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
                new TodoItemDTO
                {
                    Id = todoItem.Id,
                    Name = todoItem.Name,
                    IsComplete = todoItem.IsComplete
                };

}
