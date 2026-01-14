using System.Globalization;

namespace Client.Features.Maps;

public class YandexMapFramesProvider : IMapFramesProvider
{
    private const string AreaDisplayBaseUri =
        "https://yandex.ru/map-widget/v1/?ll={0}%2C{1}&rl={2}%2C{3}{4}&z={5}&rlt=area";
    private const string DeltaUriPart =
        "~{0}%2C{1}";
    public Uri GetAreaDisplayUri(IArea area) => area switch
    {
        RectangularArea rect => GetRectangularAreaUri(rect),
        EllipsoidArea ellipsoid => GetEllipsoidAreaUri(ellipsoid),
        CircularArea circle => GetEllipsoidAreaUri(circle.AsEllipsoid()),
        _ => throw new NotSupportedException($"{area.GetType()} is not supported")
    };

    private const string ExternalStreetViewBaseUri =
        "https://yandex.ru/maps?l=stv%2Csta&ll={0}%2C{1}&panorama%5Bdirection%5D={2}%2C0.000000&panorama%5Bfull%5D=true&panorama%5Bpoint%5D={3}%2C{4}&panorama%5Bspan%5D=0.000000%2C0.000000&z=100";
    public Uri GetExternalStreetViewUri(Point point)
    {
        var direction = 360f * Random.Shared.NextSingle();
        return new Uri(string.Format(ExternalStreetViewBaseUri,
            Format(point.X), Format(point.Y), Format(direction), Format(point.X), Format(point.Y)));
    }

    private static Uri GetRectangularAreaUri(RectangularArea area)
    {
        var center = area.Center;
        Point[] deltas =
        [
            (area.Width / 2, area.Height / 2),
            (-area.Width, 0),
            (0, -area.Height),
            (area.Width, 0),
            (0, area.Height - 0.000001f)
        ];
        return new Uri(string.Format(AreaDisplayBaseUri,
            Format(center.X), Format(center.Y), Format(center.X), Format(center.Y),
            string.Concat(deltas.Select(x => string.Format(DeltaUriPart, Format(x.X), Format(x.Y)))), GetZoom(area.Height)));
    }

    private static Uri GetEllipsoidAreaUri(EllipsoidArea area)
    {
        const int pointsCount = 24;
        var center = area.Center;

        var points = Enumerable.Range(0, pointsCount)
            .Select(i => MathF.Tau * i / pointsCount)
            .Select(angle => new Point(
                area.Center.X + area.Width / 2 * MathF.Cos(angle),
                area.Center.Y + area.Height / 2 * MathF.Sin(angle)))
            .ToArray();
        var deltas = new List<Point>(pointsCount);
        for (var i = 1; i < points.Length; i++)
        {
            var (currentPoint, previousPoint) = (points[i], points[i - 1]);
            var xDelta = currentPoint.X - previousPoint.X;
            var yDelta = currentPoint.Y - previousPoint.Y;

            deltas.Add((xDelta, yDelta));
        }
        Console.WriteLine(area.Height);
        
        return new Uri(string.Format(AreaDisplayBaseUri,
            Format(center.X), Format(center.Y), Format(center.X + area.Width / 2), Format(center.Y),
            string.Concat(deltas.Select(x => string.Format(DeltaUriPart, Format(x.X), Format(x.Y)))), GetZoom(area.Height)));
    }

    private const string StreetViewUrl =
        "https://yandex.ru/map-widget/v1/?ll={0}%2C{1}&panorama%5Bdirection%5D={2}%2C0.000000&panorama%5Bpoint%5D={3}%2C{4}&z=100";
    public Uri GetStreetViewUri(Point point)
        => new(string.Format(StreetViewUrl,
            Format(point.X), Format(point.Y), 
            Format(Random.Shared.NextSingle() * 360),
            Format(point.X), Format(point.Y)));

    public Uri GetPointDescriptionUri(Point point)
    {
        throw new NotImplementedException();
    }

    private static string Format(float coordinate) => coordinate.ToString("0.000000", CultureInfo.InvariantCulture);
    
    private static float GetZoom(float height) => 10.75f - MathF.Log(height / 0.2f, 2f);
}

public static class DependencyInjection
{
    public static IServiceCollection AddYandexFrames(this IServiceCollection services) => services
        .AddScoped<IMapFramesProvider, YandexMapFramesProvider>()
        .AddScoped<YandexMapFramesProvider>();
}