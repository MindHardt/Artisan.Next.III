using Arklens.Alids;
using Arklens.Core;
using Arklens.Generators;

namespace Arklens.Classes;

[AlidDomain]
[GenerateEnumeration]
public abstract partial record Class : IAlidEntity, IArklensEntity
{
    public Alid Alid { get; }
    public abstract ClassSkills ClassSkills { get; }

    public virtual IReadOnlyCollection<Alignment>? AllowedAlignments => null;
    public virtual IReadOnlyCollection<Subclass> Subclasses { get; } = [];
    public abstract string Emoji { get; }
    public abstract string Name { get; }
    public abstract int SkillPoints { get; }

    protected Class()
        => Alid = Alid.CreateOwnFor<Class>(GetType().Name);
}