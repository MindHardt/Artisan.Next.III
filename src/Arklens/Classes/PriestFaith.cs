using Arklens.Alids;
using Arklens.Core;

namespace Arklens.Classes;

[AlidDomain]
public record PriestFaith(Deity Deity) : Subclass, IEnumeration<PriestFaith>
{
    public override Alid Alid { get; } = Alid.OfType<PriestFaith>(Deity.Alid.Name);
    public override string Emoji => Deity.Emoji;
    public override string Name => Deity.Name;

    public override IReadOnlyCollection<Alignment> AllowedAlignments { get; } = Alignment.OneStepFrom(Deity.Alignment);

    public static IReadOnlyCollection<PriestFaith> AllValues { get; } =
        [..Deity.AllValues.Select(x => new PriestFaith(x))];
}