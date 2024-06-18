using System.Runtime.CompilerServices;

namespace Arklens.Classes;

public record Bard : Class
{
    public override string Emoji => "🎵";
    public override string Name => "Бард";

    public override ClassSkills ClassSkills { get; } = Enum
        .GetValues<ClassSkills>()
        .Aggregate((l, r) => l | r);

    public Bard([CallerMemberName] string ownName = "") : base(ownName)
    { }
}