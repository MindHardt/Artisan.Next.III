namespace Client.Features.Maps;

/// <summary>
/// Represents a point on map.
/// </summary>
/// <param name="X">The horizontal coordinate.</param>
/// <param name="Y">The vertical coordinate.</param>
public readonly record struct Point(float X, float Y)
{
    public float DistanceTo(Point other)
    {
        var xOffset = X - other.X;
        var yOffset = Y - other.Y;
        return MathF.Sqrt(xOffset * xOffset + yOffset * yOffset);
    }

    public static implicit operator Point((float x, float y) tuple)
        => new(tuple.x, tuple.y);
}