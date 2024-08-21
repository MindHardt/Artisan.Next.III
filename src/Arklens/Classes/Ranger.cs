using Arklens.Core;

namespace Arklens.Classes;

public record Ranger : Class, ISingleton<Ranger>
{
    public override string Emoji => "🦅";
    public override string Name => "Рейнджер";
    public override int SkillPoints => 4;

    public override ClassSkills ClassSkills =>
        ClassSkills.Survival |
        ClassSkills.Swimming |
        ClassSkills.KnowledgeWorld |
        ClassSkills.Acrobatics |
        ClassSkills.KnowledgeNature |
        ClassSkills.HorseRiding |
        ClassSkills.FirstAid;

    public static Ranger Instance { get; } = new();
}
