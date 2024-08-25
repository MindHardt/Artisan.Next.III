using Arklens.Core;

namespace Arklens.StatusEffects;

public record Stagger : StatusEffect, ISingleton<Stagger>
{
    public static Stagger Instance { get; } = new();
}