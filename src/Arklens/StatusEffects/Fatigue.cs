using Arklens.Core;

namespace Arklens.StatusEffects;

public record Fatigue : StatusEffect, ISingleton<Fatigue>
{
    public static Fatigue Instance { get; } = new();
}