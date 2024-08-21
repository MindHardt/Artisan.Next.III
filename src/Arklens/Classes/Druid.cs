using Arklens.Core;

namespace Arklens.Classes;

public record Druid : Class, ISingleton<Druid>
{
    public override string Emoji => "🍀";
    public override string Name => "Друид";
    public override int SkillPoints => 4;

    public override IReadOnlyCollection<Alignment> AllowedAlignments { get; } =
        Alignment.OneStepFrom(Alignment.Neutral);

    public override IReadOnlyCollection<Subclass> Subclasses => DruidCircle.AllValues;

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeNature |
        ClassSkills.Survival |
        ClassSkills.Stealth |
        ClassSkills.FirstAid |
        ClassSkills.HorseRiding;

    public static Druid Instance { get; } = new();
}
