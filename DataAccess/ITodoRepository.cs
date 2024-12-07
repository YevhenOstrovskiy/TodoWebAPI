namespace DataAccess
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<Todo>> GetCompletedAsync(CancellationToken cancellationToken = default);
        Task<List<Todo>> GetUncompletedAsync(CancellationToken cancellationToken = default);
        Task<Todo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Todo>?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task CreateAsync(Todo todo, CancellationToken cancellationToken = default);
        Task CreateBulkAsync(List<Todo> newTodos, CancellationToken cancellationToken = default);
        Task UpdateByIdAsync(Todo todo, CancellationToken cancellationToken = default);
        Task DeleteAsync(Todo todo, CancellationToken cancellationToken = default);
        Task DeleteBulkAsync(List<Todo> todos, CancellationToken cancellationToken = default);

    }
}
