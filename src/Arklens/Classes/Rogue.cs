namespace Arklens.Classes;

public record Rogue : Class
{
    public override string Emoji => "🗡️";
    public override string Name => "Плут";

    public override ClassSkills ClassSkills =>
        ClassSkills.Mechanics |
        ClassSkills.KnowledgeWorld |
        ClassSkills.Diplomacy |
        ClassSkills.Acrobatics |
        ClassSkills.Stealth |
        ClassSkills.Climbing;
}
