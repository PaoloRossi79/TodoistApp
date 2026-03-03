using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoistaVoce.Controllers;
using TodoistaVoce.Models;
using TodoistaVoce.Services;
using Xunit;

namespace TodoistaVoceTests.Unit;

public class TodosControllerUnitTests
{
    [Fact]
    public async Task CreateAndGet_ReturnsCreatedAndOk()
    {
        var repo = new InMemoryTodoRepository();
        var controller = new TodosController(repo);

        var input = new TodoItem { Title = "ctrl test", Description = "desc" };
        var createResult = await controller.Create(input, default);
        var createdAt = Assert.IsType<CreatedAtActionResult>(createResult.Result);
        var created = Assert.IsType<TodoItem>(createdAt.Value!);

        var getResult = await controller.Get(created.Id, default);
        var ok = Assert.IsType<OkObjectResult>(getResult.Result);
        var got = Assert.IsType<TodoItem>(ok.Value!);
        Assert.Equal(created.Id, got.Id);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_And_NotFoundWhenMissing()
    {
        var repo = new InMemoryTodoRepository();
        var controller = new TodosController(repo);

        var item = new TodoItem { Title = "u1" };
        await repo.CreateAsync(item);

        item.Title = "updated";
        var updateResult = await controller.Update(item.Id, item, default);
        Assert.IsType<NoContentResult>(updateResult);

        var missingId = Guid.NewGuid();
        var missing = await controller.Update(missingId, new TodoItem { Id = missingId, Title = "x" }, default);
        var notFound = Assert.IsType<NotFoundObjectResult>(missing);
    }
}
