using System.Collections.Concurrent;
using System.Reflection;

namespace Arklens.Alids;

public static class Domains
{
    private static readonly ConcurrentDictionary<Type, AlidNameCollection> Cache = new();

    /// <inheritdoc cref="Of"/>
    public static AlidNameCollection Of<T>() => Of(typeof(T));

    /// <summary>
    /// Creates an <see cref="AlidNameCollection"/> for a provided type.
    /// This method caches its result to improve performance.
    /// </summary>
    /// <remarks>
    /// This method expects classes to be decorated with <see cref="AlidDomainAttribute"/>.
    /// </remarks>
    public static AlidNameCollection Of(Type type) => Cache.GetOrAdd(type, domainType =>
    {
        Stack<AlidName> domains = [];
        for (var baseType = domainType; baseType is not null; baseType = baseType.BaseType)
        {
            if (baseType.GetCustomAttribute<AlidDomainAttribute>() is not null)
            {
                domains.Push(new AlidName(baseType.Name));
            }
        }

        return [..domains];
    });
}