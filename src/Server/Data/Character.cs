using Arklens;
using Arklens.Classes;
using Arklens.Races;

namespace Server.Data;

public record Character
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required Alignment Alignment { get; set; }
    public required Race Race { get; set; }
    public required Class Class { get; set; }
}