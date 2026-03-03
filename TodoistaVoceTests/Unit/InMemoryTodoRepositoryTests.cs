using System;
using System.Linq;
using System.Threading.Tasks;
using TodoistaVoce.Models;
using TodoistaVoce.Services;
using Xunit;

namespace TodoistaVoceTests.Unit;

public class InMemoryTodoRepositoryTests
{
    [Fact]
    public async Task CreateGetUpdateDeleteLifecycle()
    {
        var repo = new InMemoryTodoRepository();

        var item = new TodoItem { Title = "unit test", Description = "desc", IsCompleted = false };
        await repo.CreateAsync(item);

        var fetched = await repo.GetAsync(item.Id);
        Assert.NotNull(fetched);
        Assert.Equal(item.Title, fetched!.Title);

        // Update
        fetched.IsCompleted = true;
        var updated = await repo.UpdateAsync(fetched);
        Assert.True(updated);

        var afterUpdate = await repo.GetAsync(item.Id);
        Assert.True(afterUpdate!.IsCompleted);

        // Delete
        var deleted = await repo.DeleteAsync(item.Id);
        Assert.True(deleted);

        var afterDelete = await repo.GetAsync(item.Id);
        Assert.Null(afterDelete);
    }
}
