using System.Runtime.CompilerServices;

namespace Arklens.Classes;

public record Kineticist : Class
{
    public override string Emoji => "☄️";
    public override string Name => "Кинетик";

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeMagic |
        ClassSkills.Survival |
        ClassSkills.FirstAid |
        ClassSkills.KnowledgeWorld;

    public Kineticist([CallerMemberName] string ownName = "") : base(ownName)
    { }
}