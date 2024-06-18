using System.Runtime.CompilerServices;

namespace Arklens.Classes;

public record Barbarian : Class
{
    public override string Emoji => "😡";
    public override string Name => "Варвар";

    public override IReadOnlyCollection<Alignment> AllowedAlignments { get; } =
        [..Alignment.AllValues.Where(x => x.Lawfulness is not Lawfulness.Lawful)];

    public override ClassSkills ClassSkills =>
        ClassSkills.Swimming |
        ClassSkills.Survival |
        ClassSkills.KnowledgeDungeons |
        ClassSkills.HorseRiding;

    public Barbarian([CallerMemberName] string ownName = "") : base(ownName)
    { }
}