using Arklens.Core;

namespace Arklens.StatusEffects;

public record Daze : StatusEffect, ISingleton<Daze>
{
    public static Daze Instance { get; } = new();
}