﻿using System.Runtime.CompilerServices;
using Arklens.Alids;
using Arklens.Core;
using Arklens.Generators;

namespace Arklens.Classes;

[AlidDomain]
[GenerateEnumeration]
public abstract partial record Class : IAlidEntity, IArklensEntity
{
    public Alid Alid { get; }
    public abstract ClassSkills ClassSkills { get; }

    public virtual IReadOnlyCollection<Alignment>? AllowedAlignments => null;
    public virtual IReadOnlyCollection<Subclass> Subclasses { get; } = [];
    public abstract string Emoji { get; }
    public abstract string Name { get; }

    protected Class(string ownName)
        => Alid = Alid.OfType<Class>(ownName);

    public static Barbarian Barbarian { get; } = new();
    public static Bard Bard { get; } = new();
    public static Bookworm Bookworm { get; } = new();
    public static Crusader Crusader { get; } = new();
    public static Druid Druid { get; } = new();
    public static Kineticist Kineticist { get; } = new();
    public static Monk Monk { get; } = new();
    public static Priest Priest { get; } = new();
    public static Ranger Ranger { get; } = new();
    public static Rogue Rogue { get; } = new();
    public static Warrior Warrior { get; } = new();
    public static Wizard Wizard { get; } = new();
}