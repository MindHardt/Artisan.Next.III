using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Arklens.Generators;

[Generator]
public class AlidSearchGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.CreateSyntaxProvider(
                static (node, _) => node is TypeDeclarationSyntax,
                Transform)
            .Where(static x => x is not null)
            .Select(static (x, _) => x!);

        context.RegisterSourceOutput(provider.Collect(), (ctx, nodes) =>
        {
            ctx.AddSource("IAlidEntity.g.cs", SourceOf.AlidLookup(nodes));
        });
    }

    public static ITypeSymbol? Transform(GeneratorSyntaxContext ctx, CancellationToken ct)
    {
        var typeSyntax = (TypeDeclarationSyntax)ctx.Node;
        if (ctx.SemanticModel.GetDeclaredSymbol(typeSyntax, ct) is not ITypeSymbol { IsStatic: false, IsAbstract: false } typeSymbol)
        {
            return null;
        }

        var isIEnumeration =
            typeSymbol.Interfaces.Any(x => x.Name == "IEnumeration") ||
            typeSymbol.GetAttributes().Any(x => x.AttributeClass?.Name == "GenerateEnumerationAttribute");
        if (isIEnumeration is false)
        {
            return null;
        }

        var isAlidEntity = false;
        for (var baseType = typeSymbol.BaseType; baseType is not null; baseType = baseType.BaseType)
        {
            if (baseType.Interfaces.Any(x => x.Name == "IAlidEntity"))
            {
                continue;
            }
            isAlidEntity = true;
            break;
        }

        return isAlidEntity ? typeSymbol : null;
    }
}