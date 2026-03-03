using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoistaVoce.Models;
using Xunit;

namespace TodoistaVoceTests.Integration;

public class PutTodosTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public PutTodosTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Put_ExistingItem_ReturnsNoContent()
    {
        var client = _factory.CreateClient();
        var item = new TodoItem { Title = "put test" };
        var create = await client.PostAsJsonAsync("/api/todos", item);
        var created = await create.Content.ReadFromJsonAsync<TodoItem>();

        created!.Title = "updated";
        var put = await client.PutAsJsonAsync($"/api/todos/{created.Id}", created);
        Assert.Equal(HttpStatusCode.NoContent, put.StatusCode);
    }

    [Fact]
    public async Task Put_IdMismatch_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();
        var item = new TodoItem { Id = System.Guid.NewGuid(), Title = "x" };
        var put = await client.PutAsJsonAsync($"/api/todos/{System.Guid.NewGuid()}", item);
        Assert.Equal(HttpStatusCode.BadRequest, put.StatusCode);
    }
}
