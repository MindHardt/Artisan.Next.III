using System.Runtime.CompilerServices;

namespace Arklens.Classes;

public record Priest : Class
{
    public override string Emoji => "📜";
    public override string Name => "Жрец";

    public override IReadOnlyCollection<Subclass> Subclasses => PriestFaith.AllValues;

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeReligion |
        ClassSkills.FirstAid |
        ClassSkills.KnowledgeMagic |
        ClassSkills.Diplomacy |
        ClassSkills.KnowledgeDungeons;

    public Priest([CallerMemberName] string ownName = "") : base(ownName)
    { }
}