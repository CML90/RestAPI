using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models.Todo;

namespace TodoApi.Controllers.Todo;

[Route("api/[controller]")]
[ApiController]

/// <summary>
/// This Controller class contains the different endpoints for CRUD operations on the ToDo Items.
/// </summary>
public class TodoItemsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TodoItemsController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    // GET: api/TodoItems
    /// <summary>
    /// This endpoint reads all of the existing ToDo Items.
    /// </summary>
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
        return await _context.TodoItems
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }


    [HttpGet("{id}")]
    // GET: api/TodoItems/5
    /// <summary>
    /// This endpoint reads the ToDoDTO Item with a matching id.
    /// </summary>
    /// <param name="id">Todo's primary key</param>
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(todoItem);
    }


    [HttpPut("{id}")]
    // PUT: api/TodoItems/5
    /// <summary>
    /// This endpoint edits the whole ToDoDTO item with a matching id.
    /// </summary>
    /// <param name="id">Todo's primary key</param>
    /// <param name="todoDTO">Todo's DTO</param>
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
    {
        if (id != todoDTO.Id)
        {
            return BadRequest();
        }

        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.Name = todoDTO.Name;
        todoItem.IsComplete = todoDTO.IsComplete;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }


    [HttpPost]
    // POST: api/TodoItems
    /// <summary>
    /// This endpoint adds a ToDoDTO item if the user exists.
    /// </summary>
    /// <param name="todoDTO">Todo's DTO</param>
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
    {
        var user = await _context.UserItems.FindAsync(todoDTO.UserId);
        if (user == null)
        {
            return NotFound( new {message = "user does not exist"});
        }
        
        var todoItem = new TodoItem
        {
            UserId = todoDTO.UserId,
            Name = todoDTO.Name,
            IsComplete = todoDTO.IsComplete
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = todoItem.Id },
            ItemToDTO(todoItem));
    }


    [HttpDelete("{id}")]
    // DELETE: api/TodoItems/5
    /// <summary>
    /// This endpoint deletes the ToDoDTO item with a matching id.
    /// </summary>
    /// <param name="id">Todo's primary key</param>
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// This method checks if the given todo exists.
    /// </summary>
    /// <param name="id">Todo's primary key</param>
    private bool TodoItemExists(long id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }

    /// <summary>
    /// This method maps the ToDoItem to the DTO version.
    /// </summary>
    /// <param name="todoItem">TodoDTO's Superset</param>
    private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
        new TodoItemDTO
        {
            Id = todoItem.Id,
            UserId = todoItem.UserId,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };
}

