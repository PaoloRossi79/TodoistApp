using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoistaVoce.Models;
using Xunit;

namespace TodoistaVoceTests.Integration;

public class PostTodosTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public PostTodosTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Post_ValidItem_ReturnsCreated()
    {
        var client = _factory.CreateClient();
        var item = new TodoItem { Title = "post test", Description = "integration" };
        var resp = await client.PostAsJsonAsync("/api/todos", item);
        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
        var created = await resp.Content.ReadFromJsonAsync<TodoItem>();
        Assert.NotNull(created);
        Assert.Equal(item.Title, created!.Title);
    }

    [Fact]
    public async Task Post_InvalidItem_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();
        var item = new TodoItem { Title = "" }; // Title required
        var resp = await client.PostAsJsonAsync("/api/todos", item);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
    }
}
