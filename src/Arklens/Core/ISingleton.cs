namespace Arklens.Core;

/// <summary>
/// Exposes a property with single possible value of <typeparamref name="T"/>.
/// </summary>
public interface ISingleton<out T>
{
    public static abstract T Instance { get; }
}