using TodoistaVoce.Models;

namespace TodoistaVoce.Services;

/// <summary>
/// Thread-safe in-memory repository for demo/testing purposes.
/// Not intended for production use.
/// </summary>
public sealed class InMemoryTodoRepository : ITodoRepository
{
    private readonly Dictionary<Guid, TodoItem> _items = new();
    private readonly object _lock = new();

    public Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken ct = default)
    {
        lock (_lock)
        {
            return Task.FromResult(_items.Values.Select(i => Clone(i)).AsEnumerable());
        }
    }

    public Task<TodoItem?> GetAsync(Guid id, CancellationToken ct = default)
    {
        lock (_lock)
        {
            return Task.FromResult(_items.TryGetValue(id, out var v) ? Clone(v) : null);
        }
    }

    public Task CreateAsync(TodoItem item, CancellationToken ct = default)
    {
        lock (_lock)
        {
            if (_items.ContainsKey(item.Id)) throw new InvalidOperationException("Item with the same id already exists.");
            _items[item.Id] = Clone(item);
        }
        return Task.CompletedTask;
    }

    public Task<bool> UpdateAsync(TodoItem item, CancellationToken ct = default)
    {
        lock (_lock)
        {
            if (!_items.ContainsKey(item.Id)) return Task.FromResult(false);
            _items[item.Id] = Clone(item);
            return Task.FromResult(true);
        }
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        lock (_lock)
        {
            return Task.FromResult(_items.Remove(id));
        }
    }

    private static TodoItem Clone(TodoItem src) => new()
    {
        Id = src.Id,
        Title = src.Title,
        Description = src.Description,
        IsCompleted = src.IsCompleted
    };
}
