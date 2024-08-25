using Arklens.Core;

namespace Arklens.StatusEffects;

public record Paralysis : StatusEffect, ISingleton<Paralysis>
{
    public static Paralysis Instance { get; } = new();
}