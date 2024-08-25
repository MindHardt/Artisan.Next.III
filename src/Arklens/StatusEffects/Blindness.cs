using Arklens.Core;

namespace Arklens.StatusEffects;

public record Blindness : StatusEffect, ISingleton<Blindness>
{
    public static Blindness Instance { get; } = new();
}