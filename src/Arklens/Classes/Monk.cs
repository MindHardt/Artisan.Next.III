using System.Runtime.CompilerServices;

namespace Arklens.Classes;

public record Monk : Class
{
    public override string Emoji => "🧘";
    public override string Name => "Монах";

    public override IReadOnlyCollection<Alignment> AllowedAlignments =>
        [..Alignment.AllValues.Where(x => x.Lawfulness is Lawfulness.Lawful)];

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeMagic |
        ClassSkills.Swimming |
        ClassSkills.HorseRiding |
        ClassSkills.Climbing |
        ClassSkills.Acrobatics |
        ClassSkills.Stealth;
    
    public Monk([CallerMemberName] string ownName = "") : base(ownName)
    { }
} 