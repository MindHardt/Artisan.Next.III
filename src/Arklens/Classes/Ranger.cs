using System.Runtime.CompilerServices;

namespace Arklens.Classes;

public record Ranger : Class
{
    public override string Emoji => "🦅";
    public override string Name => "Рейнджер";

    public override ClassSkills ClassSkills =>
        ClassSkills.Survival |
        ClassSkills.Swimming |
        ClassSkills.KnowledgeWorld |
        ClassSkills.Acrobatics |
        ClassSkills.KnowledgeNature |
        ClassSkills.HorseRiding |
        ClassSkills.FirstAid;
    
    public Ranger([CallerMemberName] string ownName = "") : base(ownName)
    { }
}
