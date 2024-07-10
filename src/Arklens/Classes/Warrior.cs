using System.Runtime.CompilerServices;

namespace Arklens.Classes;

public record Warrior : Class
{
    public override string Emoji => "⚔️";
    public override string Name => "Воин";

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeDungeons |
        ClassSkills.HorseRiding |
        ClassSkills.Climbing |
        ClassSkills.Swimming;

    public Warrior([CallerMemberName] string ownName = "") : base(ownName)
    { }
}