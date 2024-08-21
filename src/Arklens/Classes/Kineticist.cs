using Arklens.Core;

namespace Arklens.Classes;

public record Kineticist : Class, ISingleton<Kineticist>
{
    public override string Emoji => "☄️";
    public override string Name => "Кинетик";
    public override int SkillPoints => 3;

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeMagic |
        ClassSkills.Survival |
        ClassSkills.FirstAid |
        ClassSkills.KnowledgeWorld;

    public static Kineticist Instance { get; } = new();
}