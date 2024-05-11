namespace Client.Features.Maps;

public record CircularArea(Point Center, float Radius) : IArea
{
    public float Area { get; } = MathF.PI * Radius * Radius;

    public bool Contains(Point point)
        => Center.DistanceTo(point) <= Radius;

    public EllipsoidArea AsEllipsoid() => new(Center, Radius, Radius);

    public Point GetRandomPoint(Random? random = null)
    {
        random ??= Random.Shared;

        var angle = MathF.Tau * random.NextSingle();

        var xDelta = random.NextSingle() * Radius * MathF.Cos(angle);
        var yDelta = random.NextSingle() * Radius * MathF.Sin(angle);

        return (Center.X + xDelta, Center.Y + yDelta);
    }
}