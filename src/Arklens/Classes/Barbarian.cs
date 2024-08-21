using Arklens.Core;

namespace Arklens.Classes;

public record Barbarian : Class, ISingleton<Barbarian>
{
    public override string Emoji => "😡";
    public override string Name => "Варвар";
    public override int SkillPoints => 2;

    public override IReadOnlyCollection<Alignment> AllowedAlignments { get; } =
        [..Alignment.AllValues.Where(x => x.Lawfulness is not Lawfulness.Lawful)];

    public override ClassSkills ClassSkills =>
        ClassSkills.Swimming |
        ClassSkills.Survival |
        ClassSkills.KnowledgeDungeons |
        ClassSkills.HorseRiding;

    public static Barbarian Instance { get; } = new();
}