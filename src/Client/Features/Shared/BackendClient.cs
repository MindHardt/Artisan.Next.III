using Contracts;

namespace Client.Features.Shared;

public class BackendClient(
    IAuthClient auth,
    IFileClient files,
    IWikiClient wiki,
    INotionClient notion)
{
    public IAuthClient Auth { get; } = auth;
    public IFileClient Files { get; } = files;
    public IWikiClient Wiki { get; } = wiki;
    public INotionClient Notion { get; } = notion;
}