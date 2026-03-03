using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoistaVoce.Models;
using Xunit;

namespace TodoistaVoceTests.Integration;

public class GetTodosTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public GetTodosTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task GetAll_ReturnsOk_And_Enumerable()
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync("/api/todos");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var list = await resp.Content.ReadFromJsonAsync<TodoItem[]>();
        Assert.NotNull(list);
    }

    [Fact]
    public async Task Get_NonExisting_ReturnsNotFound()
    {
        var client = _factory.CreateClient();
        var resp = await client.GetAsync($"/api/todos/{System.Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }
}
