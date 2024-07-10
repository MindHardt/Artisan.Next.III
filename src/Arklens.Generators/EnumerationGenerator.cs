using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Arklens.Generators;

[Generator]
public class EnumerationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource(nameof(SourceOf.GenerateEnumerationAttribute) + ".g.cs", SourceOf.GenerateEnumerationAttribute);
        });
        var attributeName = $"{typeof(SourceOf).Namespace}.{nameof(SourceOf.GenerateEnumerationAttribute)}";
        var typeProvider = context.SyntaxProvider.ForAttributeWithMetadataName(attributeName,
            static (_, _) => true,
            static (ctx, _) => (TypeDeclarationSyntax)ctx.TargetNode);
        var fullProvider = context.CompilationProvider.Combine(typeProvider.Collect());

        context.RegisterSourceOutput(fullProvider, GenerateEnumeration);
    }

    private static void GenerateEnumeration(
        SourceProductionContext context,
        (Compilation, ImmutableArray<TypeDeclarationSyntax>) values)
    {
        var (compilation, types) = values;
        foreach (var type in types)
        {
            var semanticModel = compilation.GetSemanticModel(type.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(type) is not INamedTypeSymbol classSymbol)
            {
                continue;
            }

            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var properties = classSymbol.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(x => x is
                {
                    IsStatic: true,
                    IsReadOnly: true
                });

            context.AddSource(
                type.Identifier.Text + ".g.cs",
                SourceOf.EnumerationClass(namespaceName, type, properties));
        }
    }
}