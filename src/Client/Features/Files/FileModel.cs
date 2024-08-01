namespace Client.Features.Files;

public record FileModel(
    FileIdentifier Identifier,
    FileHashString Hash,
    string OriginalName,
    FileSize Size,
    FileScope Scope);