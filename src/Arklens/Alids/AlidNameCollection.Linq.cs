namespace Arklens.Alids;

public static class AlidNameCollectionLinq
{
    /// <summary>
    /// Creates a <see cref="AlidNameCollection"/> from a <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static AlidNameCollection ToAlidNameCollection(this IEnumerable<AlidName> names) => [..names];
    
    /// <inheritdoc cref="ToAlidNameCollection(System.Collections.Generic.IEnumerable{Arklens.Alids.AlidName})"/>
    public static AlidNameCollection ToAlidNameCollection(this IEnumerable<string> names) => [..names.Select(x => new AlidName(x))];
}