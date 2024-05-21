namespace Arklens.Classes;

public record Priest : Class
{
    public override string Emoji => "📜";
    public override string Name => "Жрец";

    public override IReadOnlyCollection<Subclass> Subclasses { get; } =
    [
        ..Deity.AllValues.Select(deity => new Subclass(
            deity.Emoji,
            deity.Name, 
            Alignment.OneStepFrom(deity.Alignment)))
    ];

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeReligion |
        ClassSkills.FirstAid |
        ClassSkills.KnowledgeMagic |
        ClassSkills.Diplomacy |
        ClassSkills.KnowledgeDungeons;
}