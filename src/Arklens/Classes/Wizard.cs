namespace Arklens.Classes;

public record Wizard : Class
{
    public override string Emoji => "📚";
    public override string Name => "Волшебник";

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeMagic |
        ClassSkills.KnowledgeWorld |
        ClassSkills.KnowledgeReligion |
        ClassSkills.KnowledgeDungeons |
        ClassSkills.KnowledgeNature;

    public override IReadOnlyCollection<Subclass> Subclasses { get; } =
    [
        new Subclass("👻", "Аниматургия"),
        new Subclass("👁️", "Иллюзия"),
        new Subclass("🔮", "Прорицание"),
        new Subclass("💥", "Разрушение"),
        new Subclass("💫", "Релокация"),
        new Subclass("♻️", "Трансмутация"),
        new Subclass("🪄", "Универсализм")
    ];
}