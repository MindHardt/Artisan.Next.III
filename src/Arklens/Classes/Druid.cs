using System.Runtime.CompilerServices;

namespace Arklens.Classes;

public record Druid : Class
{
    public override string Emoji => "🍀";
    public override string Name => "Друид";

    public override IReadOnlyCollection<Alignment> AllowedAlignments { get; } =
        Alignment.OneStepFrom(Alignment.Neutral);

    public override IReadOnlyCollection<Subclass> Subclasses => DruidCircle.AllValues;

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeNature |
        ClassSkills.Survival |
        ClassSkills.Stealth |
        ClassSkills.FirstAid |
        ClassSkills.HorseRiding;

    public Druid([CallerMemberName] string ownName = "") : base(ownName)
    { }
}
