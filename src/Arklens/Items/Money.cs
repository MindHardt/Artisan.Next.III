using System.Numerics;

namespace Arklens.Items;

/// <summary>
/// Represents Arklens money. Can store non-negative value up to
/// 18446744073709551615 (~18 quintillion) copper.
/// </summary>
public readonly struct Money : 
    IComparable<Money>,
    IEquatable<Money>,
    IComparisonOperators<Money, Money, bool>,
    IAdditionOperators<Money, Money, Money>,
    ISubtractionOperators<Money, Money, Money>,
    IMultiplyOperators<Money, Money, Money>,
    IDivisionOperators<Money, Money, Money>
{
    public const int CopperInSilver = 10;
    public const int SilverInGold = 10;
    public const int CopperInGold = CopperInSilver * SilverInGold;
    
    private readonly ulong _copper;

    public decimal Copper => _copper;
    public decimal Silver => Copper / CopperInSilver;
    public decimal Gold => Copper / CopperInGold;

    public static Money FromCopper(decimal copper) => new(ulong.CreateChecked(copper));
    public static Money FromSilver(decimal silver) => FromCopper(silver * CopperInSilver);
    public static Money FromGold(decimal gold) => FromCopper(gold * CopperInGold);

    public static Money None => new(0);
    
    private Money(ulong copper) => _copper = copper;
    
    public bool Equals(Money other) => _copper == other._copper;
    public override bool Equals(object? obj) => obj is Money other && Equals(other);
    public override int GetHashCode() => _copper.GetHashCode();
    public override string ToString() => $"{Gold}🪙";
    public int CompareTo(Money other) => _copper.CompareTo(other._copper);
    
    public static bool operator ==(Money left, Money right) => left._copper == right._copper;
    public static bool operator !=(Money left, Money right) => left._copper != right._copper;
    public static bool operator >(Money left, Money right) => left._copper > right._copper;
    public static bool operator >=(Money left, Money right) => left._copper >= right._copper;
    public static bool operator <(Money left, Money right) => left._copper < right._copper;
    public static bool operator <=(Money left, Money right) => left._copper <= right._copper;
    public static Money operator +(Money left, Money right) => new(left._copper + right._copper);
    public static Money operator -(Money left, Money right) => new(left._copper - right._copper);
    public static Money operator *(Money left, Money right) => new(left._copper * right._copper);
    public static Money operator /(Money left, Money right) => new(left._copper / right._copper);
}