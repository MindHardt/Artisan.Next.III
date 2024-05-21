namespace Arklens.Core;

/// <summary>
/// Exposes a property with all possible values of <typeparamref name="T"/>.
/// </summary>
public interface IEnumeration<out T>
{
    public static abstract IReadOnlyCollection<T> AllValues { get; }
}