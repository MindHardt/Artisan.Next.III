using Arklens.Alids;
using Arklens.Core;

namespace Arklens.Classes;

[AlidDomain]
public abstract record Subclass : IArklensEntity, IAlidEntity
{
    public abstract Alid Alid { get; }
    public abstract string Emoji { get; }
    public abstract string Name { get; }
    
    public virtual IReadOnlyCollection<Alignment>? AllowedAlignments => null;
}