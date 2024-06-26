using Arklens.Alids;

namespace Arklens.Core;

public interface IAlidModifier
{
    public string Name { get; }
}

public interface IAlidModifier<TEntity> : IAlidModifier
    where TEntity : IAlidEntity
{
    /// <summary>
    /// Applies this <see cref="IAlidModifier{TEntity}"/>
    /// to <paramref name="entity"/>.
    /// </summary>
    /// <param name="entity">The template entity.</param>
    /// <returns>
    /// A new instance of <typeparamref name="TEntity"/> with this <see cref="IAlidModifier"/> applied.
    /// </returns>
    public TEntity Apply(TEntity entity);
}