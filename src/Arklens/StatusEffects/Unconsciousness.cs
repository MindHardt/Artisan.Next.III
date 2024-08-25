using Arklens.Core;

namespace Arklens.StatusEffects;

public record Unconsciousness : StatusEffect, ISingleton<Unconsciousness>
{
    public static Unconsciousness Instance { get; } = new();
}