using System.Runtime.CompilerServices;

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

    public override IReadOnlyCollection<Subclass> Subclasses => WizardSchool.AllValues;

    public Wizard([CallerMemberName] string ownName = "") : base(ownName)
    { }
}