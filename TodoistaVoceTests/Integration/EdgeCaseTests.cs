using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoistaVoce.Models;
using Xunit;

namespace TodoistaVoceTests.Integration;

public class EdgeCaseTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public EdgeCaseTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Post_LargePayload_ReturnsCreatedOrBadRequest()
    {
        var client = _factory.CreateClient();
        var sb = new StringBuilder();
        for (int i = 0; i < 10000; i++) sb.Append('x');
        var item = new TodoItem { Title = sb.ToString(), Description = sb.ToString() };
        var resp = await client.PostAsJsonAsync("/api/todos", item);
        Assert.True(resp.StatusCode == HttpStatusCode.Created || resp.StatusCode == HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Concurrency_MultipleCreates_NoException()
    {
        var client = _factory.CreateClient();
        var tasks = new Task[20];
        for (int i = 0; i < 20; i++)
        {
            var it = new TodoItem { Title = $"c{i}" };
            tasks[i] = client.PostAsJsonAsync("/api/todos", it);
        }
        await Task.WhenAll(tasks);
        foreach (var t in tasks)
        {
            var r = ((Task<System.Net.Http.HttpResponseMessage>)t).Result;
            Assert.True(r.IsSuccessStatusCode || r.StatusCode == HttpStatusCode.BadRequest);
        }
    }
}
