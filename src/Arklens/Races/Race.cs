using Arklens.Core;
using Arklens.Generators;

namespace Arklens.Races;

[GenerateEnumeration]
public partial record Race(
    string Name,
    string Emoji,
    RaceImpacts? Impacts) 
    : IArklensEntity
{
    public static Race Human { get; } = new(
        "Человек",
        "🧑",
        null);

    public static Race Elf { get; } = new(
        "Эльф",
        "🧝",
        new RaceImpacts(
            Dex: RaceImpact.Increased,
            Int: RaceImpact.Increased,
            Con: RaceImpact.Decreased));

    public static Race Dwarf { get; } = new(
        "Дварф",
        "🧔",
        new RaceImpacts(
            Con: RaceImpact.Increased,
            Wis: RaceImpact.Increased,
            Cha: RaceImpact.Decreased));

    public static Race Kitsune { get; } = new(
        "Кицуне",
        "🦊",
        new RaceImpacts(
            Dex: RaceImpact.Increased,
            Cha: RaceImpact.Increased,
            Str: RaceImpact.Decreased));

    public static Race Minas { get; } = new(
        "Минас",
        "♉",
        new RaceImpacts(
            Str: RaceImpact.Increased,
            Con: RaceImpact.Increased,
            Int: RaceImpact.Decreased));

    public static Race Serpent { get; } = new(
        "Серпент",
        "🦎",
        new RaceImpacts(
            Con: RaceImpact.Increased,
            Int: RaceImpact.Increased,
            Wis: RaceImpact.Decreased));

}