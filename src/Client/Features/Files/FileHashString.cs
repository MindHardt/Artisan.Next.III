using System.Security.Cryptography;
using Vogen;

namespace Client.Features.Files;

[ValueObject<string>]
public readonly partial struct FileHashString
{
    public const int ExpectedLength = SHA256.HashSizeInBytes * 2;
    private static string NormalizeInput(string hex) => hex.ToLower();

    private static Validation Validate(string hex) => hex switch
    {
        { Length: not ExpectedLength } => Validation.Invalid($"Unexpected length: {hex.Length} instead of {ExpectedLength}"),
        not null when hex.All(char.IsAsciiHexDigitLower) => Validation.Ok,
        _ => Validation.Invalid($"Unexpected characters in hex string {hex}")
    };
}