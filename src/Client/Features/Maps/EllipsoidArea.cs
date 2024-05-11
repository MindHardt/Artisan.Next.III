namespace Client.Features.Maps;

public record EllipsoidArea(Point Center, float Width, float Height) : IArea
{
    public float Area { get; } = MathF.PI * Width * Height / 4;
    public bool Contains(Point point)
    {
        var (a, b) = (Width / 2, Height / 2);
        var (cx, cy) = (Center.X, Center.Y);

        return (point.X - cx) * (point.X - cx) / (a * a) +
            (point.Y - cy) * (point.Y - cy) / (b * b) <= 1;
    }

    public Point GetRandomPoint(Random? random = null)
    {
        random ??= Random.Shared;

        var angle = MathF.Tau * random.NextSingle();

        var xOffset = random.NextSingle() * Width / 2 * MathF.Cos(angle);
        var yOffset = random.NextSingle() * Height / 2 * MathF.Sin(angle);

        return (Center.X + xOffset, Center.Y + yOffset);
    }
}