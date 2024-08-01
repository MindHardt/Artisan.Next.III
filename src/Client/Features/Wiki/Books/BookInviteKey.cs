using System.Collections.Frozen;
using System.Security.Cryptography;
using Vogen;

namespace Client.Features.Wiki.Books;

[ValueObject<string>]
public readonly partial struct BookInviteKey
{
    public const int MaxLength = 16;

    public static readonly string AllowedCharacters = new(
    [
        ..Enumerable.Range('a', 26).Select(x => (char)x),
        ..Enumerable.Range('A', 26).Select(x => (char)x)
    ]);
    public static readonly IReadOnlySet<char> AllowedCharactersSet = AllowedCharacters.ToFrozenSet();

    public static Validation Validate(string inviteKey) => inviteKey switch
    {
        { Length: > MaxLength }
            => Validation.Invalid($"{nameof(BookInviteKey)} cannot be longer than {MaxLength} symbols"),

        _ when inviteKey.All(AllowedCharactersSet.Contains)
            => Validation.Ok,

        _ => Validation.Invalid($"Provided string {inviteKey} contains forbidden characters.")
    };

    public static BookInviteKey Create() => From(
        RandomNumberGenerator.GetString(AllowedCharacters, MaxLength));
}