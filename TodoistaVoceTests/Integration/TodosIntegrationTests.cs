using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoistaVoce.Models;
using Xunit;

namespace TodoistaVoceTests.Integration;

public class TodosIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TodosIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CrudEndpoints_WorkEndToEnd()
    {
        var client = _factory.CreateClient();

        // Create
        var create = new TodoItem { Title = "integration", Description = "i test" };
        var createResp = await client.PostAsJsonAsync("/api/todos", create);
        Assert.Equal(HttpStatusCode.Created, createResp.StatusCode);
        var created = await createResp.Content.ReadFromJsonAsync<TodoItem>();
        Assert.NotNull(created);

        // Get
        var getResp = await client.GetAsync($"/api/todos/{created!.Id}");
        Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);

        // Update
        created.Title = "integration updated";
        var putResp = await client.PutAsJsonAsync($"/api/todos/{created.Id}", created);
        Assert.Equal(HttpStatusCode.NoContent, putResp.StatusCode);

        // Delete
        var delResp = await client.DeleteAsync($"/api/todos/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, delResp.StatusCode);
    }
}
