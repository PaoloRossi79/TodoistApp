using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoistaVoce.Models;
using Xunit;

namespace TodoistaVoceTests.Integration;

public class DeleteTodosTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public DeleteTodosTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Delete_Existing_ReturnsNoContent_And_Removes()
    {
        var client = _factory.CreateClient();
        var item = new TodoItem { Title = "delete test" };
        var create = await client.PostAsJsonAsync("/api/todos", item);
        var created = await create.Content.ReadFromJsonAsync<TodoItem>();

        var del = await client.DeleteAsync($"/api/todos/{created!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, del.StatusCode);

        var get = await client.GetAsync($"/api/todos/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
    }

    [Fact]
    public async Task Delete_NonExisting_ReturnsNotFound()
    {
        var client = _factory.CreateClient();
        var del = await client.DeleteAsync($"/api/todos/{System.Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, del.StatusCode);
    }
}
