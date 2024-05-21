namespace Arklens.Classes;

public record Crusader : Class
{
    public override string Emoji => "✡️";
    public override string Name => "Крестоносец";

    public override IReadOnlyCollection<Subclass> Subclasses { get; } =
        [..Deity.AllValues.Select(deity => new Subclass(deity.Emoji, deity.Name, [deity.Alignment]))];

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeReligion |
        ClassSkills.KnowledgeDungeons |
        ClassSkills.Diplomacy;
}