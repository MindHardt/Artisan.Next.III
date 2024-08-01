using System.Collections.Frozen;
using NickBuhro.Translit;
using Vogen;

namespace Client.Features.Wiki.Books;

[ValueObject<string>]
public readonly partial struct BookUrlName
{
    public const int MaxLength = 64;

    public static readonly string AllowedCharacters = new(
    [
        ..Enumerable.Range('a', 26).Select(x => (char)x),
        ..Enumerable.Range('0', 10).Select(x => (char)x),
        '-'
    ]);
    private static readonly IReadOnlySet<char> AllowedCharactersSet = AllowedCharacters.ToFrozenSet();

    public static Validation Validate(string urlName) => urlName switch
    {
        { Length: > MaxLength }
            => Validation.Invalid($"Url name exceeds a limit of {MaxLength} characters"),

        _ when urlName.All(AllowedCharactersSet.Contains)
            => Validation.Ok,

        _ => Validation.Invalid($"Url name {urlName} contains forbidden symbols. Allowed symbols are [{AllowedCharacters}].")
    };

    private static string NormalizeInput(string input)
    {
        return Validate(input) is { ErrorMessage: [] } 
            ? input 
            : Transliteration.CyrillicToLatin(input, Language.Russian).Replace(' ', '-').ToLower();
    }
}