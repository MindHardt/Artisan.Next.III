using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Arklens.Alids;

public partial record Alid(
    AlidNameCollection Domains, 
    AlidName Name, 
    AlidNameCollection Modifiers = default, 
    bool IsGroup = false)
    : IParsable<Alid>
{
    public const int MaxLength = 128;
    
    [StringSyntax(StringSyntaxAttribute.Regex)]
    public const string Regex = @$"(?<Domains>({AlidName.Regex}:)+)(?<Name>#?{AlidName.Regex})(?<Modifiers>(\+{AlidName.Regex})*)";
    [GeneratedRegex($"^{Regex}$")]
    public static partial Regex ValidationRegex();
    

    public AlidNameCollection Domains { get; } = Domains.Count > 0
        ? Domains
        : throw new ArgumentException($"{nameof(Domains)} cannot be empty");

    public string Value { get; } =
        string.Concat(Domains.Select(x => $"{x}:")) +
        (IsGroup ? "#" : string.Empty) +
        Name +
        string.Concat(Modifiers.Select(x => $"+{x}"));

    #region Overrides

    public virtual bool Equals(Alid? other) => Value == other?.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    #endregion

    public static Alid OfType<T>(string name) => OfType<T>(new AlidName(name));
    public static Alid OfType<T>(AlidName name) => new(Alids.Domains.Of<T>(), name);
    public static Alid Parse(string s, IFormatProvider? provider = null)
    {
        ArgumentNullException.ThrowIfNull(s);
        if (s.Length > MaxLength)
        {
            throw new FormatException($"Input string is too long for {nameof(Alid)}");
        }
        
        if (ValidationRegex().Match(s) is not { Success: true } match)
        {
            throw new FormatException($"String {s} was not in a correct {nameof(Alid)} format");
        }

        return FromMatch(match);
    }

    public static bool TryParse(string? s, [MaybeNullWhen(false)] out Alid result) => 
        TryParse(s, null, out result);
    
    public static bool TryParse(string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Alid result)
    {
        if (s?.Length <= MaxLength && ValidationRegex().Match(s) is { Success: true } match)
        {
            result = FromMatch(match);
            return true;
        }

        result = null;
        return false;
    }

    private static Alid FromMatch(Match match)
    {
        var domains = match.Groups["Domains"].Value
            .Split(":", StringSplitOptions.RemoveEmptyEntries)
            .ToAlidNameCollection();
        
        var modifiers = match.Groups["Modifiers"].Value
            .Split("+", StringSplitOptions.RemoveEmptyEntries)
            .ToAlidNameCollection();

        var nameValue = match.Groups["Name"].Value;
        
        var (isGroup, name) = nameValue.StartsWith('#')
            ? (true, new AlidName(nameValue[1..]))
            : (false, new AlidName(nameValue));

        return new Alid(domains, name, modifiers, isGroup);
    }
}