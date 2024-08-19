using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Vogen;

namespace Contracts;

public static class BookEndpoints
{
    public const string Path = "books";
    public const string FullPath = $"{WikiEndpoints.FullPath}/{Path}";
}

public record BookModel(
    BookUrlName UrlName,
    string Name,
    string Description,
    string? ImageUrl,
    string Author,
    bool IsPublic,
    bool Editable);

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

        _ => Validation.Invalid($"Url name {urlName} does not match regex {ValidationRegexText}")
    };
}

[ValueObject<string>]
public readonly partial struct BookInviteKey
{
    public const int MaxLength = 16;
    public static readonly IReadOnlySet<char> AllowedSymbols = ((char[])
    [
        ..Enumerable.Range('a', 26).Select(x => (char)x),
        ..Enumerable.Range('A', 26).Select(x => (char)x),
    ]).ToFrozenSet();

    public static Validation Validate(string inviteKey) => inviteKey switch
    {
        { Length: > MaxLength }
            => Validation.Invalid($"{nameof(BookInviteKey)} cannot be longer than {MaxLength} symbols"),

        _ when inviteKey.All(x => AllowedSymbols.Contains(x))
            => Validation.Ok,

        _ => Validation.Invalid($"Provided string {inviteKey} contains forbidden characters.")
    };

}