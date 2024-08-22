using System.Runtime.CompilerServices;
using Arklens.Alids;
using Arklens.Generators;

namespace Arklens.Classes;

[AlidDomain]
[GenerateEnumeration]
public partial record DruidCircle : Subclass
{
    public override Alid Alid { get; }
    public override string Emoji { get; }
    public override string Name { get; }

    private DruidCircle(string emoji, string name, [CallerMemberName] string ownName = "")
    {
        Emoji = emoji;
        Name = name;
        Alid = Alid.CreateOwnFor<DruidCircle>(ownName);
    }

    public static DruidCircle Plants { get; } = new("🌿", "Растения");
    public static DruidCircle Beasts { get; } = new("🦌", "Звери");
    public static DruidCircle Fungi { get; } = new("🍄", "Грибы");
    public static DruidCircle Decay { get; } = new("🍖", "Гниения");
    public static DruidCircle Insects { get; } = new("🦂", "Насекомые");
}