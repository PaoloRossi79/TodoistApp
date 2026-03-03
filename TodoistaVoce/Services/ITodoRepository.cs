using TodoistaVoce.Models;

namespace TodoistaVoce.Services;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken ct = default);
    Task<TodoItem?> GetAsync(Guid id, CancellationToken ct = default);
    Task CreateAsync(TodoItem item, CancellationToken ct = default);
    Task<bool> UpdateAsync(TodoItem item, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
