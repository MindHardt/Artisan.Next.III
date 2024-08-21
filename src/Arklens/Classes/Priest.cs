using Arklens.Core;

namespace Arklens.Classes;

public record Priest : Class, ISingleton<Priest>
{
    public override string Emoji => "📜";
    public override string Name => "Жрец";
    public override int SkillPoints => 3;

    public override IReadOnlyCollection<Subclass> Subclasses => PriestFaith.AllValues;

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeReligion |
        ClassSkills.FirstAid |
        ClassSkills.KnowledgeMagic |
        ClassSkills.Diplomacy |
        ClassSkills.KnowledgeDungeons;

    public static Priest Instance { get; } = new();
}