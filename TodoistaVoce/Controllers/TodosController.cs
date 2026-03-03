using Microsoft.AspNetCore.Mvc;
using TodoistaVoce.Models;
using TodoistaVoce.Services;

namespace TodoistaVoce.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoRepository _repo;

    public TodosController(ITodoRepository repo) => _repo = repo;

    /// <summary>
    /// Get all todo items.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll(CancellationToken ct)
    {
        try
        {
            var items = await _repo.GetAllAsync(ct);
            return Ok(items);
        }
        catch (Exception ex)
        {
            var pd = new ProblemDetails { Title = "Failed to get todos", Detail = ex.Message, Status = StatusCodes.Status500InternalServerError };
            return StatusCode(pd.Status.Value, pd);
        }
    }

    /// <summary>
    /// Get a single todo item by id.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoItem>> Get(Guid id, CancellationToken ct)
    {
        try
        {
            var item = await _repo.GetAsync(id, ct);
            if (item is null) return NotFound(new { message = "Todo item not found." });
            return Ok(item);
        }
        catch (Exception ex)
        {
            var pd = new ProblemDetails { Title = "Failed to get todo", Detail = ex.Message, Status = StatusCodes.Status500InternalServerError };
            return StatusCode(pd.Status.Value, pd);
        }
    }

    /// <summary>
    /// Create a new todo item.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TodoItem>> Create([FromBody] TodoItem input, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            input.Id = Guid.NewGuid();
            await _repo.CreateAsync(input, ct);
            return CreatedAtAction(nameof(Get), new { id = input.Id }, input);
        }
        catch (Exception ex)
        {
            var pd = new ProblemDetails { Title = "Failed to create todo", Detail = ex.Message, Status = StatusCodes.Status500InternalServerError };
            return StatusCode(pd.Status.Value, pd);
        }
    }

    /// <summary>
    /// Update an existing todo item.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] TodoItem input, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (id != input.Id) return BadRequest(new { message = "Route id and body id do not match." });

        try
        {
            var updated = await _repo.UpdateAsync(input, ct);
            if (!updated) return NotFound(new { message = "Todo item not found." });
            return NoContent();
        }
        catch (Exception ex)
        {
            var pd = new ProblemDetails { Title = "Failed to update todo", Detail = ex.Message, Status = StatusCodes.Status500InternalServerError };
            return StatusCode(pd.Status.Value, pd);
        }
    }

    /// <summary>
    /// Delete a todo item.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            var removed = await _repo.DeleteAsync(id, ct);
            if (!removed) return NotFound(new { message = "Todo item not found." });
            return NoContent();
        }
        catch (Exception ex)
        {
            var pd = new ProblemDetails { Title = "Failed to delete todo", Detail = ex.Message, Status = StatusCodes.Status500InternalServerError };
            return StatusCode(pd.Status.Value, pd);
        }
    }
}
