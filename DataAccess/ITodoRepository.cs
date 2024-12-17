namespace DataAccess
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetAllAsync(Guid? accountId, CancellationToken cancellationToken = default);
        Task<List<Todo>> GetCompletedAsync(Guid? accountId, CancellationToken cancellationToken = default);
        Task<List<Todo>> GetUncompletedAsync(Guid? accountId, CancellationToken cancellationToken = default);
        Task<Todo?> GetByIdAsync(Guid? accountId, Guid id, CancellationToken cancellationToken = default);
        Task<List<Todo>?> GetByNameAsync(Guid? accountId, string name, CancellationToken cancellationToken = default);
        Task CreateAsync(Todo todo, CancellationToken cancellationToken = default);
        Task CreateBulkAsync(List<Todo> newTodos, CancellationToken cancellationToken = default);
        Task UpdateByIdAsync(Guid? accountId, Todo todo, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid? accountId, Todo todo, CancellationToken cancellationToken = default);
        Task DeleteBulkAsync(Guid? accountId, List<Todo> todos, CancellationToken cancellationToken = default);

    }
}
