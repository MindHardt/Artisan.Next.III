using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Vogen;

namespace Contracts;

public static class FileEndpoints
{
    public const string Path = "files";
    public const string FullPath = $"{Api.Prefix}/{Path}";
}

public record FileModel(
    FileIdentifier Identifier,
    FileHashString Hash,
    string OriginalName,
    FileSize Size,
    FileScope Scope)
{
    public string Url => GetFile.FullPath
        .Replace($"{{{nameof(GetFile.Request.Identifier)}}}", Identifier.Value);
    public string GetUrl(GetFile.Name name) => name is GetFile.Name.Server
        ? Url
        : $"{Url}?{nameof(GetFile.Request.Name)}={name}";
}

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

[ValueObject<string>]
public readonly partial struct FileIdentifier
{
    public const int MaxLength = 255;

    private static Validation Validate(string identifier) => identifier switch
    {
        { Length: > MaxLength }
            => Validation.Invalid($"File identifier {identifier} exceeds a limit of {MaxLength} characters"),

        _ when string.IsNullOrEmpty(Path.GetExtension(identifier))
            => Validation.Invalid($"File identifier {identifier} is missing extension"),

        _ => Validation.Ok
    };
}

[ValueObject<long>]
public readonly partial struct FileSize
{
    public long Bytes => Value;
    public double Kilobytes => Bytes / 1024d;
    public double Megabytes => Kilobytes / 1024d;
    public double Gigabytes => Megabytes / 1024d;

    private static Validation Validate(long size) => size > 0
        ? Validation.Ok
        : Validation.Invalid("File size cannot be negative");
}

public enum FileScope
{
    Unknown,
    Avatar,
    Attachment,
    Character
}