using Arklens.Core;

namespace Arklens.Classes;

public record Rogue : Class, ISingleton<Rogue>
{
    public override string Emoji => "🗡️";
    public override string Name => "Плут";
    public override int SkillPoints => 6;

    public override ClassSkills ClassSkills =>
        ClassSkills.Mechanics |
        ClassSkills.KnowledgeWorld |
        ClassSkills.Diplomacy |
        ClassSkills.Acrobatics |
        ClassSkills.Stealth |
        ClassSkills.Climbing;

    public static Rogue Instance { get; } = new();
}
