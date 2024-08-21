using Arklens.Core;

namespace Arklens.Classes;

public record Crusader : Class, ISingleton<Crusader>
{
    public override string Emoji => "✡️";
    public override string Name => "Крестоносец";
    public override int SkillPoints => 2;

    public override IReadOnlyCollection<Subclass> Subclasses => CrusaderFaith.AllValues;

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeReligion |
        ClassSkills.KnowledgeDungeons |
        ClassSkills.Diplomacy;

    public static Crusader Instance { get; } = new();
}