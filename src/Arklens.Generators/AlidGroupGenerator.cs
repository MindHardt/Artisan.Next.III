using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Arklens.Generators;

[Generator]
public class AlidGroupGenerator : IIncrementalGenerator
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
            ctx.AddSource("AlidGroup.g.cs", SourceOf.AlidGroups(nodes));
        });
    }

    public static ITypeSymbol? Transform(GeneratorSyntaxContext ctx, CancellationToken ct)
        => ctx.SemanticModel.GetDeclaredSymbol((TypeDeclarationSyntax)ctx.Node, ct) is ITypeSymbol
        {
            IsStatic: false,
            IsAbstract: false,
            DeclaredAccessibility: Accessibility.Public,
            BaseType.Name: "AlidGroup"
        } type ? type : null;
}