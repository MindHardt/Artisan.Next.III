using System.Runtime.CompilerServices;

namespace Arklens.Classes;

public record Crusader : Class
{
    public override string Emoji => "✡️";
    public override string Name => "Крестоносец";

    public override IReadOnlyCollection<Subclass> Subclasses => CrusaderFaith.AllValues;

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeReligion |
        ClassSkills.KnowledgeDungeons |
        ClassSkills.Diplomacy;
    
    public Crusader([CallerMemberName] string ownName = "") : base(ownName)
    { }
}