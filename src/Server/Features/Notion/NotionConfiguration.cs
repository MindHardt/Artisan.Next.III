namespace Server.Features.Notion;

public record NotionConfiguration
{
    public StatusEffectsConfiguration StatusEffects { get; set; } = null!;
}