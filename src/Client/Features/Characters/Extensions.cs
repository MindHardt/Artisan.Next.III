using System.Numerics;

namespace Client.Features.Characters;

public static class Extensions
{
    public static string AsModifier<T>(this T number)
        where T: ISignedNumber<T> => T.IsNegative(number)
        ? $"{number}"
        : $"+{number}";
}