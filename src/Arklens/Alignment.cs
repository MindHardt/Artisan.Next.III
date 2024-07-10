using System.Diagnostics;
using Arklens.Alids;
using Arklens.Generators;

namespace Arklens;

[AlidDomain]
[GenerateEnumeration]
public partial record Alignment(Lawfulness Lawfulness, Goodness Goodness) : IAlidEntity
{
    public Alid Alid { get; } = Alid.OfType<Alignment>($"{Lawfulness}{Goodness}");

    public int DistanceTo(Alignment other) =>
        int.Abs(Goodness - other.Goodness) +
        int.Abs(Lawfulness - other.Lawfulness);

    public string Emoji { get; } =
        Lawfulness switch
        {
            Lawfulness.Lawful => "⚖️",
            Lawfulness.Neutral => null,
            Lawfulness.Chaotic => "🌬️",
            _ => throw new UnreachableException()
        } +
        Goodness switch
        {
            Goodness.Good => "🙂",
            Goodness.Neutral => "😐",
            Goodness.Evil => "😠",
            _ => throw new UnreachableException()
        };

    public string Name { get; } =
        Lawfulness switch
        {
            Lawfulness.Lawful => "Законное ",
            Lawfulness.Neutral => null,
            Lawfulness.Chaotic => "Хаотичное ",
            _ => throw new UnreachableException()
        } +
        Goodness switch
        {
            Goodness.Good => "Доброе",
            Goodness.Neutral => "Нейтральное",
            Goodness.Evil => "Злое",
            _ => throw new UnreachableException()
        };

    public static Alignment LawfulGood { get; } = new(Lawfulness.Lawful, Goodness.Good);
    public static Alignment NeutralGood { get; } = new(Lawfulness.Neutral, Goodness.Good);
    public static Alignment ChaoticGood { get; } = new(Lawfulness.Chaotic, Goodness.Good);
    public static Alignment LawfulNeutral { get; } = new(Lawfulness.Lawful, Goodness.Neutral);
    public static Alignment Neutral { get; } = new(Lawfulness.Neutral, Goodness.Neutral);
    public static Alignment ChaoticNeutral { get; } = new(Lawfulness.Chaotic, Goodness.Neutral);
    public static Alignment LawfulEvil { get; } = new(Lawfulness.Lawful, Goodness.Evil);
    public static Alignment NeutralEvil { get; } = new(Lawfulness.Neutral, Goodness.Evil);
    public static Alignment ChaoticEvil { get; } = new(Lawfulness.Chaotic, Goodness.Evil);

    public static IReadOnlyCollection<Alignment> OneStepFrom(Alignment anchor)
        => [..AllValues.Where(x => x.DistanceTo(anchor) <= 1)];
}

public enum Goodness : sbyte
{
    Good = 1,
    Neutral = 2,
    Evil = 3,
}

public enum Lawfulness : sbyte
{
    Lawful = 1,
    Neutral = 0,
    Chaotic = -1
}