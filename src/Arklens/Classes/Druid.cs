namespace Arklens.Classes;

public record Druid : Class
{
    public override string Emoji => "🍀";
    public override string Name => "Друид";

    public override IReadOnlyCollection<Alignment> AllowedAlignments { get; } = 
        Alignment.OneStepFrom(Alignment.Neutral);

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeNature |
        ClassSkills.Survival |
        ClassSkills.Stealth |
        ClassSkills.FirstAid |
        ClassSkills.HorseRiding;
}
