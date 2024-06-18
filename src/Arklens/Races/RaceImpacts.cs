namespace Arklens.Races;

public readonly record struct RaceImpacts(
    RaceImpact Str = RaceImpact.Unaffected,
    RaceImpact Dex = RaceImpact.Unaffected,
    RaceImpact Con = RaceImpact.Unaffected,
    RaceImpact Int = RaceImpact.Unaffected,
    RaceImpact Wis = RaceImpact.Unaffected,
    RaceImpact Cha = RaceImpact.Unaffected)
{
    public RaceImpact GetByEmoji(string emoji) => emoji switch
    {
        "💪" => Str,
        "🏃" => Dex,
        "🧡" => Con,
        "🧠" => Int,
        "🦉" => Wis,
        "👄" => Cha,
        _ => throw new InvalidOperationException()
    };
}