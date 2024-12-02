using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI
{

    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoDb _db;
        private readonly ILogger<TodoController> _logger;

        public TodoController(TodoDb db, ILogger<TodoController> logger)
        {
            _db = db;
            _logger = logger;
        }

        private ActionResult HandleError(Exception ex, string errorMessage)
        {
            _logger.LogError(ex, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
        }

        [HttpGet]
        [HttpGet("todoitems")]
        public async Task<ActionResult<IEnumerable<Todo>>> Get()
        {
            try
            {
                _logger.LogInformation("Fetching Todo items");

                var todos = await _db.Todos.ToListAsync();
                if (todos == null || !todos.Any())
                {
                    _logger.LogWarning("No Todo items found in the database.");
                    return NoContent();
                }

                _logger.LogInformation("Successfully fetched {Count} Todo items", todos.Count);
                return Ok(todos);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "An error occurred while fetching Todo items.");
            }
        }

        [HttpGet("completed")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetCompleted()
        {
            try
            {
                _logger.LogInformation("Fetching completed Todo items");

                var completedTodos = await _db.Todos.Where(t => t.IsComplete).ToListAsync();
                if (completedTodos == null || !completedTodos.Any())
                {
                    _logger.LogInformation("No completed Todo items found.");
                    return NoContent();
                }

                _logger.LogInformation("Successfully fetched {Count} completed Todo items", completedTodos.Count);
                return Ok(completedTodos);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "An error occurred while fetching completed Todo items.");
            }
        }

        [HttpGet("uncompleted")]
        public async Task<IActionResult> GetUncompleted()
        {
            try
            {
                _logger.LogInformation("Fetching uncompleted Todo items");

                var uncompletedTodos = await _db.Todos.Where(t => !t.IsComplete).ToListAsync();

                if (uncompletedTodos == null || !uncompletedTodos.Any())
                {
                    _logger.LogInformation("No uncompleted Todo items found.");
                    return NoContent();
                }

                _logger.LogInformation("Successfully fetched {Count} uncompleted Todo items", uncompletedTodos.Count);
                return Ok(uncompletedTodos);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "An error occurred while fetching uncompleted Todo items.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching Todo item with ID {Id}", id);

                var todo = await _db.Todos.FindAsync(id);

                if (todo == null)
                {
                    _logger.LogWarning("Todo with ID {Id} not found.", id);
                    return NotFound($"Todo with ID {id} was not found.");
                }

                _logger.LogInformation("Successfully fetched Todo item with ID {Id}", id);
                return Ok(todo);
            }
            catch (Exception ex)
            {
                return HandleError(ex, $"An error occurred while fetching Todo item with ID {id}");
            }

        }

        [HttpGet("search")]
        public async Task<IActionResult> FindByName([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest("Query string is empty.");
                }

                _logger.LogInformation("Searching Todo items contains Name {Name}", query);

                var todos = await _db.Todos
                    .Where(t => t.Name != null && t.Name.ToLower().Contains(query.ToLower()))
                    .ToListAsync();

                if (todos == null || !todos.Any())
                {
                    _logger.LogInformation("No Todo items contains Name {Name} found.", query);
                    return NoContent();
                }

                _logger.LogInformation("Successfully fetched {Count} Todo items", todos.Count);
                return Ok(todos);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "An error occurred while serching Todo item.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTodo([FromBody] string newTodoName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newTodoName))
                {
                    return BadRequest("Todo name is required.");
                }

                var newTodo = new Todo
                {
                    Id = Guid.NewGuid(),
                    Name = newTodoName,
                    IsComplete = false
                };

                _logger.LogInformation("Creating Todo item with ID {Id}", newTodo.Id);

                _db.Todos.Add(newTodo);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = newTodo.Id }, newTodo);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "An error occurred while creating a new Todo item.");
            }
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddTodos([FromBody] IEnumerable<string> newTodoNames)
        {
            try
            {
                if (newTodoNames == null || !newTodoNames.Any())
                {
                    _logger.LogWarning("Attempted to add an empty or null list of todos.");
                    return BadRequest("The list of new todo names cannot be null or empty.");
                }

                var todos = newTodoNames.Select(name => new Todo
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    IsComplete = false
                });

                _logger.LogInformation("Adding {Count} Todo items in bulk.", todos.Count());

                _db.Todos.AddRange(todos);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), null, todos);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "An error occurred while adding todos in bulk.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeTodo(Guid id, [FromBody] Todo inputTodo)
        {
            try
            {
                var todo = await _db.Todos.FindAsync(id);

                if (todo == null)
                {
                    _logger.LogWarning("Todo item with ID {Id} not found", id);
                    return NotFound($"Todo with ID {id} not found.");
                }

                todo.Name = inputTodo.Name;
                todo.IsComplete = inputTodo.IsComplete;

                await _db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex, $"An error occurred while updating Todo item with ID {id}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(Guid id)
        {
            try
            {
                var todo = await _db.Todos.FindAsync(id);

                if (todo == null)
                {
                    _logger.LogWarning("Todo item with ID {Id} not found", id);
                    return NotFound($"Todo with ID {id} not found.");
                }

                _db.Todos.Remove(todo);
                await _db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex, $"An error occurred while deleting Todo item with ID {id}");
            }
        }

        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteTodos([FromBody] IEnumerable<Todo> todos)
        {
            try
            {
                if (todos == null || !todos.Any())
                {
                    _logger.LogWarning("Attempted to delete an empty or null list of todos.");
                    return BadRequest("The list of todos to delete cannot be null or empty.");
                }

                var ids = todos.Select(t => t.Id).ToList();
                var existingTodos = await _db.Todos.Where(t => ids.Contains(t.Id)).ToListAsync();

                if (!existingTodos.Any())
                {
                    _logger.LogWarning("No matching Todo items found for deletion.");
                    return NotFound("No matching Todo items found in the database.");
                }

                _db.Todos.RemoveRange(existingTodos);
                await _db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex, "An error occurred while deleting Todo items.");
            }
        }
    }
}