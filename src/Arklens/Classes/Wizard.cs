using Arklens.Core;

namespace Arklens.Classes;

public record Wizard : Class, ISingleton<Wizard>
{
    public override string Emoji => "📚";
    public override string Name => "Волшебник";
    public override int SkillPoints => 5;

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeMagic |
        ClassSkills.KnowledgeWorld |
        ClassSkills.KnowledgeReligion |
        ClassSkills.KnowledgeDungeons |
        ClassSkills.KnowledgeNature;

    public override IReadOnlyCollection<Subclass> Subclasses => WizardSchool.AllValues;
    
    public static Wizard Instance { get; } = new();
}