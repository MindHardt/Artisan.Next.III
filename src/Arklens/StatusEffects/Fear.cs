using Arklens.Core;

namespace Arklens.StatusEffects;

public record Fear : StatusEffect, ISingleton<Fear>
{
    public static Fear Instance { get; } = new();
}