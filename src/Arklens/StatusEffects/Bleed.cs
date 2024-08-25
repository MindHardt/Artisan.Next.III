using Arklens.Core;

namespace Arklens.StatusEffects;

public record Bleed : StatusEffect, ISingleton<Bleed>
{
    public static Bleed Instance { get; } = new();
}