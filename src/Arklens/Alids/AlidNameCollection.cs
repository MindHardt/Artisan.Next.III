using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Arklens.Alids;

[CollectionBuilder(typeof(AlidNameCollection), nameof(From))]
[DebuggerDisplay(nameof(Values))]
public readonly struct AlidNameCollection : IEquatable<AlidNameCollection>, IReadOnlyList<AlidName>
{
    public IReadOnlyList<AlidName> Values { get; }

    public static AlidNameCollection From(ReadOnlySpan<AlidName> values) => values.Length > 0
        ? new AlidNameCollection(values)
        : Empty;

    public static AlidNameCollection Create(IEnumerable<string> names) => 
        new(names.Select(x => new AlidName(x)));
    public static AlidNameCollection Create(params string[] names) => 
        Create(names.AsEnumerable());
    
    public AlidNameCollection(IEnumerable<AlidName> values) => Values = [..values];
    public AlidNameCollection(ReadOnlySpan<AlidName> values) => Values = [..values];

    public static AlidNameCollection Empty => default;

    #region Overrides

    public override bool Equals(object? obj) => obj is AlidNameCollection other && Equals(other);
    public bool Equals(AlidNameCollection other) => (Values, other.Values) switch
    {
        (null, null) => true,
        (null, not null) or (not null, null) => false,
        _ => Values.SequenceEqual(other.Values)
    };
    
    public override int GetHashCode() => Values?.Select(x => x.GetHashCode()).Aggregate(HashCode.Combine) ?? default;

    public static bool operator ==(AlidNameCollection left, AlidNameCollection right) => left.Equals(right);
    public static bool operator !=(AlidNameCollection left, AlidNameCollection right) => left.Equals(right) is false;
    
    #endregion

    #region IReadOnlyList

    public IEnumerator<AlidName> GetEnumerator() => (Values ?? []).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public int Count => Values?.Count ?? 0;

    public AlidName this[int index] => (Values ?? [])[index];

    #endregion
}