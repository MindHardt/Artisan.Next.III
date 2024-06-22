using Arklens;

namespace Contracts;

public static class MonsterEndpoints
{
    public const string Path = "monsters";
    public const string FullPath = $"{WikiEndpoints.FullPath}/{Path}";
}

public record MonsterModel(
    string Name,
    Alignment Alignment,
    FileIdentifier CoverImage,
    FileIdentifier MinifigureImage);