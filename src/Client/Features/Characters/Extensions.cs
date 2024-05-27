using System.Numerics;

namespace Client.Features.Characters;

public static class Extensions
{
    public static string AsModifier<T>(this T number)
        where T: ISignedNumber<T> => T.IsNegative(number)
        ? $"{number}"
        : $"+{number}";

    /// <summary>
    /// Calculates base64 string for a provided <see cref="Stream"/>.
    /// </summary>
    public static async Task<string> CalculateBase64(this Stream stream, CancellationToken ct = default)
    {
        var destination = new MemoryStream();
        await stream.CopyToAsync(destination, ct);
        return Convert.ToBase64String(destination.ToArray());
    }
}