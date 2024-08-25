using System.Reflection;

namespace Arklens.Alids;

public static class AlidExtensions
{
    public static Type GetDomainType(this IAlidEntity entity)
    {
        var type = entity.GetType();
        do
        {
            if (type.GetCustomAttribute<AlidDomainAttribute>() is not null)
            {
                return type;
            }

            type = type.BaseType;
        } 
        while (type is not null);

        throw new InvalidOperationException($"Could not retrieve domain type for entity type {entity.GetType().Name}");
    }

    /// <summary>
    /// Defines whether given <see cref="AlidNameCollection"/>
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool IsSubdomainOf(this AlidNameCollection first, AlidNameCollection second) =>
        first.Count <= second.Count &&
        first.Zip(second).All(x => x.First == x.Second);
}