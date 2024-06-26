﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Arklens.Generators;

public static partial class SourceOf
{
    public static SourceText EnumerationClass(
        string @namespace,
        TypeDeclarationSyntax type,
        IEnumerable<IPropertySymbol> properties)
    {
        var keyword = type switch
        {
            ClassDeclarationSyntax cds => cds.Keyword.Text,
            StructDeclarationSyntax sds => sds.Keyword.Text,
            RecordDeclarationSyntax rds => $"{rds.Keyword.Text} {rds.ClassOrStructKeyword.Text}".TrimEnd(),
            _ => throw new InvalidOperationException()
        };
        var typeName = type.Identifier.Text;
        return SourceText.From(@$"// <auto-generated/>

using System.Collections.Generic;
using Arklens.Core;

namespace {@namespace};

partial {keyword} {typeName} : IEnumeration<{typeName}>
{{
    public static IReadOnlyCollection<{typeName}> AllValues {{ get; }} = 
    [
{string.Join(",\n", properties.Select(prop => $"\t\t{prop.Name}"))}
    ];
}}
", Encoding.UTF8);
    }
}