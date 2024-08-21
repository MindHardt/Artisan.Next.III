using Arklens.Core;

namespace Arklens.Classes;

public record Monk : Class, ISingleton<Monk>
{
    public override string Emoji => "🧘";
    public override string Name => "Монах";
    public override int SkillPoints => 2;

    public override IReadOnlyCollection<Alignment> AllowedAlignments =>
        [..Alignment.AllValues.Where(x => x.Lawfulness is Lawfulness.Lawful)];

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeMagic |
        ClassSkills.Swimming |
        ClassSkills.HorseRiding |
        ClassSkills.Climbing |
        ClassSkills.Acrobatics |
        ClassSkills.Stealth;

    public static Monk Instance { get; } = new();
} 