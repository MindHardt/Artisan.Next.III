using Arklens.Core;

namespace Arklens.StatusEffects;

public record Fallen : StatusEffect, ISingleton<Fallen>
{
    public static Fallen Instance { get; } = new();
}