namespace Client.Features.Maps;

public record RectangularArea(Point BottomLeft, Point TopRight) : IArea
{
    public RectangularArea(float bottom, float left, float top, float right)
        : this((left, bottom), (right, top))
    { }

    public RectangularArea(Point center, float width, float height)
        : this(
            center.Y - height / 2,
            center.X - width / 2,
            center.Y + height / 2,
            center.X + height / 2)
    { }

    public float Width => TopRight.X - BottomLeft.X;
    public float Height => TopRight.Y - BottomLeft.Y;
    public Point Center => (BottomLeft.X + Width / 2, BottomLeft.Y + Height / 2);

    public float Area { get; } =
        MathF.Abs(TopRight.X - BottomLeft.X) *
        MathF.Abs(TopRight.Y - BottomLeft.Y);

    public bool Contains(Point point) =>
        BottomLeft.X <= point.X && point.X <= TopRight.X &&
        BottomLeft.Y <= point.Y && point.Y <= TopRight.Y;

    public Point GetRandomPoint(Random? random = null)
    {
        random ??= Random.Shared;

        var xSpan = TopRight.X - BottomLeft.X;
        var ySpan = TopRight.Y - BottomLeft.Y;

        var xDelta = xSpan * random.NextSingle();
        var yDelta = ySpan * random.NextSingle();

        return (BottomLeft.X + xDelta, BottomLeft.Y + yDelta);
    }
}