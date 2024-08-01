namespace Client.Features.Notion;

public record StatusEffectModel(
    string? CoverUrl,
    string Name,
    string Icon,
    string Description,
    string? PageUrl);