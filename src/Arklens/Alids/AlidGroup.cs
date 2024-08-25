using System.Collections;
using System.Collections.Frozen;

namespace Arklens.Alids;

public abstract class AlidGroup : IEnumerable<IAlidEntity>
{
    public abstract Alid Alid { get; }
    public abstract IEnumerable<IAlidEntity> EnumerateEntities();
    
    // ReSharper disable once NotDisposedResourceIsReturned
    public IEnumerator<IAlidEntity> GetEnumerator() => EnumerateEntities().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public abstract class AlidGroup<TEntity> : AlidGroup, IReadOnlyCollection<TEntity>
    where TEntity : IAlidEntity
{
    public override Alid Alid { get; }
    public IReadOnlyCollection<TEntity> Values { get; }

    public AlidGroup(IEnumerable<TEntity> values, string? name = null)
    {
        Alid = Alid.CreateGroupOf<TEntity>(name ?? GetType().Name);
        Values = [..values];
    }
    
    public override IEnumerable<IAlidEntity> EnumerateEntities() => [..Values];
    
    // ReSharper disable once NotDisposedResourceIsReturned
    public new IEnumerator<TEntity> GetEnumerator() => Values.GetEnumerator();
    public int Count => Values.Count;
}

file class DomainAlidGroup<TEntity>(IEnumerable<IAlidEntity> rawValues) : 
    AlidGroup<TEntity>(rawValues.Cast<TEntity>(), "all")
    where TEntity : IAlidEntity;

public static partial class AlidGroups
{
    private static readonly FrozenDictionary<Alid, AlidGroup> Dictionary = GetGroups()
        .Concat(GetDomainGroups())
        .ToFrozenDictionary(x => x.Alid);
    private static partial IEnumerable<AlidGroup> GetGroups();
    private static IEnumerable<AlidGroup> GetDomainGroups() => IAlidEntity.All
        .GroupBy(x => x.Alid.Domains)
        .Select(x => x.ToArray())
        .Select(entities =>
        {
            var entityType = entities.Select(AlidExtensions.GetDomainType).Distinct().Single();
            var groupType = typeof(DomainAlidGroup<>).MakeGenericType(entityType);
            return (AlidGroup)Activator.CreateInstance(groupType, args: [entities])!;
        });

    public static IReadOnlyCollection<AlidGroup> All => Dictionary.Values;
    
    #region FindMethods

    /// <summary>
    /// Finds single <see cref="IAlidEntity"/> from provided non-group <see cref="Alid"/>.
    /// </summary>
    /// <returns>
    /// The <see cref="IAlidEntity"/> for given <see cref="Alid"/> or <see langword="null"/> if none is found.
    /// </returns>
    public static AlidGroup? Find(Alid alid) => alid.IsGroup
        ? Dictionary.GetValueOrDefault(alid)
        : throw new InvalidOperationException("Cannot get alid group from single entity alid");

    /// <summary>
    /// Finds single <see cref="IAlidEntity"/> from provided non-group <see cref="Alid"/>.
    /// </summary>
    /// <returns>
    /// The <see cref="IAlidEntity"/> for given <see cref="Alid"/> or <see langword="null"/> if none is found,
    /// or it is not of type <typeparamref name="TEntity"/>.
    /// </returns>
    public static AlidGroup<TEntity>? Find<TEntity>(Alid alid) where TEntity : IAlidEntity
        => Find(alid) as AlidGroup<TEntity>;

    /// <summary>
    /// Finds single <see cref="IAlidEntity"/> from provided non-group <see cref="Alid"/>.
    /// </summary>
    /// <exception cref="KeyNotFoundException">If no entity is found for given <see cref="Alid"/>.</exception>
    public static AlidGroup Get(Alid alid) =>
        Find(alid) ?? throw new KeyNotFoundException($"{nameof(IAlidEntity)} with alid {alid} not found");

    /// <summary>
    /// Finds single <see cref="IAlidEntity"/> from provided non-group <see cref="Alid"/>.
    /// </summary>
    /// <exception cref="KeyNotFoundException">If no entity is found for given <see cref="Alid"/>.</exception>
    /// <exception cref="InvalidCastException">If found entity is not of type <typeparamref name="TEntity"/>.</exception>
    public static AlidGroup<TEntity> Get<TEntity>(Alid alid) where TEntity : IAlidEntity => 
        Get(alid) as AlidGroup<TEntity> ?? 
        throw new InvalidCastException($"The found entity is not of type {typeof(TEntity)}");

    #endregion
}