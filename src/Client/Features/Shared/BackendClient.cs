using Client.Features.Auth;
using Client.Features.Files;
using Client.Features.Notion;
using Client.Features.Wiki;

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