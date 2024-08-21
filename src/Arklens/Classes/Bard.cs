using Arklens.Core;

namespace Arklens.Classes;

public record Bard : Class, ISingleton<Bard>
{
    public override string Emoji => "🎵";
    public override string Name => "Бард";
    public override int SkillPoints => 5;

    public override ClassSkills ClassSkills { get; } = Enum
        .GetValues<ClassSkills>()
        .Aggregate((l, r) => l | r);

    public static Bard Instance { get; } = new();
}