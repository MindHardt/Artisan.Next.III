using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Arklens.Alids;

[JsonConverter(typeof(AlidJsonConverter))]
public partial interface IAlidEntity
{
    public Alid Alid { get; }

    private static readonly FrozenDictionary<Alid, IAlidEntity> Dictionary = GetEntities()
        .ToFrozenDictionary(x => x.Alid);
    private static partial IEnumerable<IAlidEntity> GetEntities();

    /// <summary>
    /// Contains all the alid entities <see cref="IAlidEntity"/> that are discoverable by <see cref="FindSingle"/>
    /// and similar static methods of <see cref="IAlidEntity"/>.
    /// </summary>
    public static IReadOnlyCollection<IAlidEntity> All => Dictionary.Values;

    #region FindMethods

    /// <summary>
    /// Finds single <see cref="IAlidEntity"/> from provided non-group <see cref="Alid"/>.
    /// </summary>
    /// <returns>
    /// The <see cref="IAlidEntity"/> for given <see cref="Alid"/> or <see langword="null"/> if none is found.
    /// </returns>
    public static IAlidEntity? FindSingle(Alid alid) => alid.IsGroup
        ? throw new InvalidOperationException("Cannot get single entity from group alid")
        : Dictionary.GetValueOrDefault(alid);

    /// <summary>
    /// Finds single <see cref="IAlidEntity"/> from provided non-group <see cref="Alid"/>.
    /// </summary>
    /// <returns>
    /// The <see cref="IAlidEntity"/> for given <see cref="Alid"/> or <see langword="null"/> if none is found,
    /// or it is not of type <typeparamref name="TEntity"/>.
    /// </returns>
    public static TEntity? FindSingle<TEntity>(Alid alid) where TEntity : IAlidEntity
        => FindSingle(alid) is TEntity entity ? entity : default;

    /// <summary>
    /// Finds single <see cref="IAlidEntity"/> from provided non-group <see cref="Alid"/>.
    /// </summary>
    /// <exception cref="KeyNotFoundException">If no entity is found for given <see cref="Alid"/>.</exception>
    public static IAlidEntity GetSingle(Alid alid) =>
        FindSingle(alid) ?? throw new KeyNotFoundException($"{nameof(IAlidEntity)} with alid {alid} not found");

    /// <summary>
    /// Finds single <see cref="IAlidEntity"/> from provided non-group <see cref="Alid"/>.
    /// </summary>
    /// <exception cref="KeyNotFoundException">If no entity is found for given <see cref="Alid"/>.</exception>
    /// <exception cref="InvalidCastException">If found entity is not of type <typeparamref name="TEntity"/>.</exception>
    public static TEntity GetSingle<TEntity>(Alid alid) where TEntity : IAlidEntity => GetSingle(alid) is TEntity entity
        ? entity
        : throw new InvalidCastException($"The found entity is not of type {typeof(TEntity)}");

    #endregion
}