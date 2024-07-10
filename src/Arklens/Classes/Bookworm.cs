using System.Runtime.CompilerServices;

namespace Arklens.Classes;

public record Bookworm : Class
{
    public override string Emoji => "🎓";
    public override string Name => "Книгочей";

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeReligion |
        ClassSkills.KnowledgeDungeons |
        ClassSkills.KnowledgeMagic |
        ClassSkills.KnowledgeNature |
        ClassSkills.KnowledgeWorld |
        ClassSkills.Diplomacy |
        ClassSkills.Mechanics |
        ClassSkills.FirstAid;

    public Bookworm([CallerMemberName] string ownName = "") : base(ownName)
    { }
}