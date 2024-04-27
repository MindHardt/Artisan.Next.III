using Contracts;

namespace Client.Features.Shared;

public class BackendClient(
    IAuthClient auth,
    IFileClient files,
    IWikiClient wiki)
{
    public IAuthClient Auth { get; } = auth;
    public IFileClient Files { get; } = files;
    public IWikiClient Wiki { get; } = wiki;
}