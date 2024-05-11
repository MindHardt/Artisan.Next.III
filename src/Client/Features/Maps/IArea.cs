using System.Text.Json.Serialization;

namespace Client.Features.Maps;

[JsonPolymorphic]
[JsonDerivedType(typeof(CircularArea))]
[JsonDerivedType(typeof(RectangularArea))]
[JsonDerivedType(typeof(EllipsoidArea))]
public interface IArea
{
    /// <summary>
    /// Gets the area of this <see cref="IArea"/>.
    /// </summary>
    /// <returns></returns>
    public float Area { get; }

    /// <summary>
    /// Gets the center of this <see cref="IArea"/>.
    /// </summary>
    public Point Center { get; }

    /// <summary>
    /// Checks whether this <see cref="IArea"/>
    /// contains <paramref name="point"/> inside.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    /// <remarks>Points on the border are considered contained.</remarks>
    public bool Contains(Point point);

    /// <summary>
    /// Gets random <see cref="Point"/> inside this <see cref="IArea"/>.
    /// </summary>
    /// <param name="random">
    /// A <see cref="Random"/> instance to used to choose a point.
    /// If emitted the result can be unpredictable.
    /// </param>
    /// <returns></returns>
    public Point GetRandomPoint(Random? random = null);
}