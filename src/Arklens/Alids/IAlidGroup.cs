namespace Arklens.Alids;

public interface IAlidGroup
{
    public static abstract Alid Alid { get; }
    public static abstract IReadOnlyCollection<IAlidEntity> Values { get; }
}