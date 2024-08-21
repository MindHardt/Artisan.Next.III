using Arklens.Core;

namespace Arklens.Classes;

public record Bookworm : Class, ISingleton<Bookworm>
{
    public override string Emoji => "🎓";
    public override string Name => "Книгочей";
    public override int SkillPoints => 6;

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeReligion |
        ClassSkills.KnowledgeDungeons |
        ClassSkills.KnowledgeMagic |
        ClassSkills.KnowledgeNature |
        ClassSkills.KnowledgeWorld |
        ClassSkills.Diplomacy |
        ClassSkills.Mechanics |
        ClassSkills.FirstAid;

    public static Bookworm Instance { get; } = new();
}