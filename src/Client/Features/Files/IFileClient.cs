using Client.Infrastructure;

namespace Client.Features.Files;

public partial interface IFileClient
{
    public const string Prefix = $"{Api.Prefix}/files";
}