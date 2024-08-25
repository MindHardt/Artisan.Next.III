using Arklens.Core;

namespace Arklens.StatusEffects;

public record Entanglement : StatusEffect, ISingleton<Entanglement>
{
    public static Entanglement Instance { get; } = new();
}