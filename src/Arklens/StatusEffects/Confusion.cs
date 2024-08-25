using Arklens.Core;

namespace Arklens.StatusEffects;

public record Confusion : StatusEffect, ISingleton<Confusion>
{
    public static Confusion Instance { get; } = new();
}