﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Arklens.Generators;

public partial class SourceOf
{
    public static SourceText AlidLookup(IEnumerable<ITypeSymbol> types)
        => SourceText.From(@$"// <auto-generated/>
#nullable enable
using System.Collections.Frozen;
using System.Collections.Generic;
using Arklens.Core;

namespace Arklens.Alids;

partial interface IAlidEntity
{{
    private static readonly FrozenDictionary<Alid, IAlidEntity> AllValuesDictionary = GetAllValues()
        .ToFrozenDictionary(x => x.Alid);
    public static partial IAlidEntity? Find(Alid alid) => AllValuesDictionary.GetValueOrDefault(alid);

    private static IReadOnlyCollection<IAlidEntity> GetAllValues() => 
    [
{string.Join("\n", types.Select(x => $"\t\t..{x.ContainingNamespace.ToDisplayString()}.{x.Name}.AllValues,"))}
        ..AdditionalEntities
    ];
}}", Encoding.UTF8);
}