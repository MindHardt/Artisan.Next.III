using Arklens.Core;
using Arklens.Generators;

namespace Arklens;

[GenerateEnumeration]
public partial record Deity(string Emoji, string Name, Alignment Alignment) : IArklensEntity
{
    public static Deity Neras { get; } = new("⚒️", "Нерас", Alignment.LawfulGood);
    public static Deity Sol { get; } = new("☀️", "Сол", Alignment.NeutralGood);
    public static Deity Yunai { get; } = new("✨", "Юнай", Alignment.ChaoticGood);
    public static Deity Avar { get; } = new("⚔️", "Авар", Alignment.LawfulNeutral);
    public static Deity Justar { get; } = new("⚖️", "Джастар", Alignment.Neutral);
    public static Deity Mortess { get; } = new("💋", "Мортисса", Alignment.ChaoticNeutral);
    public static Deity Archivarius { get; } = new("💀", "Архивариус", Alignment.LawfulEvil);
    public static Deity Asterio { get; } = new("👑", "Астерио", Alignment.NeutralEvil);
    public static Deity Sanguise { get; } = new("🦷", "Сангиз", Alignment.ChaoticEvil);    
}