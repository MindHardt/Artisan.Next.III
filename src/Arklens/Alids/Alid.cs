namespace Arklens.Alids;

public record Alid(AlidNameCollection Domains, AlidName Name, AlidNameCollection Modifiers = default)
{
    public const int MaxLength = 128;
    
    public AlidNameCollection Domains { get; } = Domains.Count > 0
        ? Domains
        : throw new ArgumentException($"{nameof(Domains)} cannot be empty");

    public string Value { get; } =
        string.Concat(Domains.Select(x => $"{x}:")) +
        Name +
        string.Concat(Modifiers.Select(x => $"+{x}"));
    
    #region Overrides

    public virtual bool Equals(Alid? other) => Value == other?.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    #endregion

    /// <summary>
    /// Creates a new <see cref="Alid"/> from <see cref="string"/> representation.
    /// </summary>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is not valid.</exception>
    public static Alid Create(string value)
    {
        if (value.Length > MaxLength)
        {
            throw new ArgumentException($"Value {value} is too long to be {nameof(Alid)}");
        }
        
        var domainsEndIndex = value.LastIndexOf(':');
        var domains = AlidNameCollection.Create(value[..domainsEndIndex].TrimEnd(':').Split(':'));

        var modifiersStartIndex = value.IndexOf('+');
        var modifiers = modifiersStartIndex > 0
            ? AlidNameCollection.Create(value[modifiersStartIndex..].TrimStart('+').Split('+'))
            : AlidNameCollection.Empty;

        var name = modifiersStartIndex > 0
            ? new AlidName(value[(domainsEndIndex + 1)..(modifiersStartIndex - 1)])
            : new AlidName(value[(domainsEndIndex + 1)..]);

        return new Alid(domains, name, modifiers);
    }
    
    public static Alid OfType<T>(string name) => OfType<T>(new AlidName(name));
    public static Alid OfType<T>(AlidName name) => new(Alids.Domains.Of<T>(), name);
}