using System.Text.Json.Serialization;
using Arklens.Classes;

namespace Arklens.Alids;

[JsonConverter(typeof(AlidJsonConverter))]
public partial interface IAlidEntity
{
    public Alid Alid { get; }

    public static IReadOnlyCollection<IAlidEntity> AllValues => AllValuesDictionary.Values;
    private static readonly IEnumerable<IAlidEntity> AdditionalEntities =
    [
        ..Class.AllValues
    ];
    
    public static partial IAlidEntity? Find(Alid alid);

    public static TEntity? Find<TEntity>(Alid alid) where TEntity : IAlidEntity
        => Find(alid) is TEntity entity ? entity : default;

    public static IAlidEntity Get(Alid alid) => 
        Find(alid) ?? throw new KeyNotFoundException($"{nameof(IAlidEntity)} with alid {alid} not found");
    public static TEntity Get<TEntity>(Alid alid) where TEntity : IAlidEntity
        => (TEntity)Get(alid);
}