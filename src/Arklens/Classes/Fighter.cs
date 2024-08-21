using Arklens.Core;

namespace Arklens.Classes;

public record Fighter : Class, ISingleton<Fighter>
{
    public override string Emoji => "⚔️";
    public override string Name => "Воин";
    public override int SkillPoints => 3;

    public override ClassSkills ClassSkills =>
        ClassSkills.KnowledgeDungeons |
        ClassSkills.HorseRiding |
        ClassSkills.Climbing |
        ClassSkills.Swimming;

    public static Fighter Instance { get; } = new();
}