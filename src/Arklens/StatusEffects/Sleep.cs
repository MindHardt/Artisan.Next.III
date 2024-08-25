using Arklens.Core;

namespace Arklens.StatusEffects;

public record Sleep : StatusEffect, ISingleton<Sleep>
{
    public static Sleep Instance { get; } = new();
}