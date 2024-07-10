using System.Runtime.CompilerServices;
using Arklens.Alids;
using Arklens.Generators;

namespace Arklens.Classes;

[AlidDomain]
[GenerateEnumeration]
public partial record WizardSchool : Subclass
{
    private WizardSchool(string emoji, string name, [CallerMemberName] string ownName = "")
    {
        Alid = Alid.OfType<WizardSchool>(ownName);
        Emoji = emoji;
        Name = name;
    }

    public override Alid Alid { get; }

    public override string Emoji { get; }
    public override string Name { get; }

    public static WizardSchool Animaturgy { get; } = new("👻", "Аниматургия");
    public static WizardSchool Illusion { get; } = new("👁️", "Иллюзия");
    public static WizardSchool Divination { get; } = new("🔮", "Прорицание");
    public static WizardSchool Destruction { get; } = new("💥", "Разрушение");
    public static WizardSchool Relocation { get; } = new("💫", "Релокация");
    public static WizardSchool Transmutation { get; } = new("♻️", "Трансмутация");
    public static WizardSchool Universalism { get; } = new("🪄", "Универсализм");
}