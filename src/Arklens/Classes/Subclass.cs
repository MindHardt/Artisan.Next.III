using Arklens.Core;

namespace Arklens.Classes;

public record Subclass(
    string Emoji,
    string Name,
    IReadOnlyCollection<Alignment>? AllowedAlignments = null) : IArklensEntity;