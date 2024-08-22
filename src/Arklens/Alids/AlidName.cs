using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace Arklens.Alids;

public readonly partial struct AlidName([NotNull] string? value) : IEquatable<AlidName>
{
    [StringSyntax(StringSyntaxAttribute.Regex)]
    public const string Regex = "[A-Za-z0-9_]+";
    
    [GeneratedRegex($"^{Regex}$")]
    public static partial Regex ValidationRegex();

    public string Value { get; } = TryNormalize(value, out var result)
        ? result
        : throw new ArgumentException($"'{value}' is not a valid {nameof(AlidName)}");

    #region Overrides

    public override string ToString() => Value;
    public bool Equals(AlidName other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is AlidName other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(AlidName left, AlidName right) => left.Equals(right);
    public static bool operator !=(AlidName left, AlidName right) => left.Equals(right) is false;

    #endregion

    public static implicit operator AlidName(string s) => new(s);

    private static bool TryNormalize(string? value, [NotNullWhen(true)] out string? result)
    {
        if (string.IsNullOrEmpty(value) || ValidationRegex().IsMatch(value) is false)
        {
            result = null;
            return false;
        }

        var sb = new StringBuilder(value.Length);
        sb.Append(char.ToLower(value[0]));
        for (var i = 1; i < value.Length; i++)
        {
            var character = value[i];
            if (char.IsUpper(character))
            {
                sb.Append($"_{char.ToLower(character)}");
            }
            else
            {
                sb.Append(character);
            }
        }

        result = sb.ToString();
        return true;
    }
}