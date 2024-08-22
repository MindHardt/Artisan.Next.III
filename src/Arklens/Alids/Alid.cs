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

    public AlidName Name { get; } = Name.Value.Length > 0 || IsGroup
        ? Name
        : throw new ArgumentException($"Non-group {nameof(Alid)}s cannot have empty name");

    public string Value { get; } = new(
    [
        ..Domains.SelectMany(x => $"{x}:"),
        ..IsGroup ? "#" : string.Empty,
        ..Name.Value,
        ..Modifiers.SelectMany(x => $"+{x}")
    ]);

    public static Alid CreateOwn(AlidNameCollection domains, AlidName name, AlidNameCollection modifiers = default) =>
        new(domains, name, modifiers, IsGroup: false);
    public static Alid CreateOwnFor<T>(AlidName name, AlidNameCollection modifiers = default) =>
        CreateOwn(Alids.Domains.Of<T>(), name, modifiers);

    public static Alid CreateGroup(AlidNameCollection domains, AlidName name, AlidNameCollection modifiers = default) =>
        new(domains, name, modifiers, IsGroup: true);
    public static Alid CreateGroupOf<T>(AlidName name, AlidNameCollection modifiers = default) =>
        CreateGroup(Alids.Domains.Of<T>(), name, modifiers);
    
    #region Overrides

    public virtual bool Equals(Alid? other) => Value == other?.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    #endregion

    #region Parse

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

    #endregion
}