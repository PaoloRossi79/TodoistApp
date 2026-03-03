using System.ComponentModel.DataAnnotations;

namespace TodoistaVoce.Models;

/// <summary>
/// Represents a task item.
/// </summary>
public sealed class TodoItem
{
    /// <summary>Unique identifier.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Short title.</summary>
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional details.</summary>
    [StringLength(2000)]
    public string? Description { get; set; }

    /// <summary>Whether the item is completed.</summary>
    public bool IsCompleted { get; set; }
}
