using Vogen;

namespace Client.Features.Files;

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