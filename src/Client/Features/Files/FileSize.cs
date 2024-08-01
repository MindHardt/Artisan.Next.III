using Vogen;

namespace Client.Features.Files;

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