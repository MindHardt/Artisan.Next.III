using Arklens.Alids;
using Arklens.Core;

namespace Arklens.Classes;

[AlidDomain]
public record CrusaderFaith(Deity Deity) : Subclass, IEnumeration<CrusaderFaith>
{
    public override Alid Alid { get; } = Alid.CreateOwnFor<CrusaderFaith>(Deity.Alid.Name);
    public override string Emoji => Deity.Emoji;
    public override string Name => Deity.Name;

    public override IReadOnlyCollection<Alignment> AllowedAlignments { get; } = [Deity.Alignment];

    public static IReadOnlyCollection<CrusaderFaith> AllValues { get; } =
        [..Deity.AllValues.Select(x => new CrusaderFaith(x))];
}