using Arklens.Core;

namespace Arklens.StatusEffects;

public record Exhaustion : StatusEffect, ISingleton<Exhaustion>
{
    public static Exhaustion Instance { get; } = new();
}