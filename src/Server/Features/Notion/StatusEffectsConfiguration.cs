using System.ComponentModel.DataAnnotations;

namespace Server.Features.Notion;

public record StatusEffectsConfiguration
{
    [Required]
    public string DatabaseId { get; set; } = null!;
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
}