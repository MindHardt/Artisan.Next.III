using Arklens.Core;

namespace Arklens.StatusEffects;

public record Terror : StatusEffect, ISingleton<Terror>
{
    public static Terror Instance { get; } = new();
}