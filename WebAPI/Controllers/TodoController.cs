using Businesslogic;
using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI
{

    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService, ILogger<TodoController> logger)
        {
            _logger = logger;
            _todoService = todoService;
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

                var todos = await _todoService.GetAllAsync();
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

                var completedTodos = await _todoService.GetCompletedAsync();
                if (completedTodos is null || !completedTodos.Any())
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

                var uncompletedTodos = await _todoService.GetUncompletedAsync();

                if (uncompletedTodos is null || !uncompletedTodos.Any())
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
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching Todo item with ID {Id}", id);

                var todo = await _todoService.GetByIdAsync(id);

                if (todo is null)
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

                var todos = await _todoService.GetByNameAsync(query);

                if (todos is null || !todos.Any())
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
        public async Task<IActionResult> CreateTodo([FromBody] string newTodoName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newTodoName))
                {
                    return BadRequest("Todo name is required.");
                }

                _logger.LogInformation("Creating new Todo item with Name {Name}", newTodoName);

                await _todoService.CreateAsync(newTodoName);

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex, "An error occurred while creating a new Todo item.");
            }
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddTodos([FromBody] List<string> newTodoNames)
        {
            try
            {
                if (newTodoNames is null || !newTodoNames.Any())
                {
                    _logger.LogWarning("Attempted to add an empty or null list of todos.");
                    return BadRequest("The list of new todo names cannot be null or empty.");
                }

                await _todoService.CreateBulkAsync(newTodoNames);
                _logger.LogInformation("Adding {Count} Todo items in bulk.", newTodoNames.Count);

                return NoContent();
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
                var todo = await _todoService.GetByIdAsync(id);

                if (todo is null)
                {
                    _logger.LogWarning("Todo item with ID {Id} not found", id);
                    return NotFound($"Todo with ID {id} not found.");
                }

                await _todoService.UpdateByIdAsync(id, inputTodo);

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
                var todo = await _todoService.GetByIdAsync(id);

                if (todo is null)
                {
                    _logger.LogWarning("Todo item with ID {Id} not found", id);
                    return NotFound($"Todo with ID {id} not found.");
                }

                await _todoService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex, $"An error occurred while deleting Todo item with ID {id}");
            }
        }

        [HttpDelete("bulk")]
        public async Task<IActionResult> DeleteTodos([FromBody] IEnumerable<Todo> todosToDelete)
        {
            try
            {
                if (todosToDelete is null || !todosToDelete.Any())
                {
                    _logger.LogWarning("Attempted to delete an empty or null list of todos.");
                    return BadRequest("The list of todos to delete cannot be null or empty.");
                }

                var ids = todosToDelete.Select(t => t.Id).ToList();
                var todos = await _todoService.GetAllAsync();
                var existingTodos = todos.Where(t => ids.Contains(t.Id)).ToList();

                if (!existingTodos.Any())
                {
                    _logger.LogWarning("No matching Todo items found for deletion.");
                    return NotFound("No matching Todo items found in the database.");
                }

                await _todoService.DeleteBulkAsync(existingTodos);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex, "An error occurred while deleting Todo items.");
            }
        }
    }
}