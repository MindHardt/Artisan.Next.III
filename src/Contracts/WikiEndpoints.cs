using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Vogen;

namespace Contracts;

public static class WikiEndpoints
{
    public const string Path = "wiki";
    public const string FullPath = $"{Api.Prefix}/{Path}";
}

public record BookModel(
    BookUrlName UrlName,
    string Name,
    string Description,
    string? ImageUrl,
    string Author);

[ValueObject<string>]
public readonly partial struct BookUrlName
{
    public const int MaxLength = 64;

    [StringSyntax(StringSyntaxAttribute.Regex)]
    public const string ValidationRegexText = @"[a-z\-0-9]";
    
    [GeneratedRegex(ValidationRegexText)]
    public static partial Regex ValidationRegex();

    public static Validation Validate(string urlName) => urlName switch
    {
        { Length: > MaxLength }
            => Validation.Invalid($"Url name exceeds a limit of {MaxLength} characters"),

        _ when ValidationRegex().IsMatch(urlName)
            => Validation.Ok,
        
        _ 
            => Validation.Invalid($"Url name {urlName} does not match regex {ValidationRegexText}")
    };
}