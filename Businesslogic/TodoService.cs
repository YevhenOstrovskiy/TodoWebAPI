using BusinessLogic;
using DataAccess;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Businesslogic
{
    internal class TodoService : ITodoService
    {
        private readonly ITodoRepository todoRepository;
        private readonly Guid? accountId;

        public TodoService(ITodoRepository todoRepository, IAccountService accountService)
        {
            this.todoRepository = todoRepository;
            accountId = accountService.GetAccountId();
        }
        public async Task<List<Todo>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await todoRepository.GetAllAsync(accountId);
        }

        public async Task<List<Todo>> GetCompletedAsync(CancellationToken cancellationToken = default)
        {
            return await todoRepository.GetCompletedAsync(accountId, cancellationToken);
        }

        public async Task<List<Todo>> GetUncompletedAsync(CancellationToken cancellationToken = default)
        {
            return await todoRepository.GetUncompletedAsync(accountId, cancellationToken);
        }

        public async Task<Todo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await todoRepository.GetByIdAsync(accountId, id, cancellationToken);
        }

        public async Task<List<Todo>?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await todoRepository.GetByNameAsync(accountId, name, cancellationToken);
        }

        public async Task CreateAsync(string name, CancellationToken cancellationToken = default)
        {
            if (accountId is not null)
            {
                var todo = new Todo
                {
                    Name = name,
                    AccountId = (Guid)accountId,

                };

                await todoRepository.CreateAsync(todo, cancellationToken);
            }

        }

        public async Task CreateBulkAsync(List<string> newTodoNames, CancellationToken cancellationToken = default)
        {
            var todos = newTodoNames.Select(name => new Todo
            {
                Name = name,
                AccountId= (Guid)accountId,
            }).ToList();

            await todoRepository.CreateBulkAsync(todos, cancellationToken);
        }

        public async Task UpdateByIdAsync(Guid id, Todo inputTodo, CancellationToken cancellationToken = default)
        {
            var todo = await todoRepository.GetByIdAsync(accountId, id, cancellationToken);
            if (todo is null)
            {
                throw new ArgumentNullException($"Todo with {id} wasn`t found");
            }

            todo.Name = inputTodo.Name;
            todo.IsComplete = inputTodo.IsComplete;

            await todoRepository.UpdateByIdAsync(accountId, inputTodo, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var todo = await todoRepository.GetByIdAsync(accountId, id, cancellationToken);
            if (todo is null)
            {
                throw new ArgumentNullException($"Todo with {id} wasn`t found");
            }

            await todoRepository.DeleteAsync(accountId, todo, cancellationToken);
        }

        public async Task DeleteBulkAsync(List<Todo> todos, CancellationToken cancellationToken = default)
        {
            if (todos.IsNullOrEmpty())
            {
                throw new ArgumentNullException($"Todo list is null or empty.Value of todos: {todos}");
            }

            await todoRepository.DeleteBulkAsync(accountId, todos, cancellationToken);
        }
    }
}
