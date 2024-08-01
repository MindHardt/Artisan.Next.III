using Client.Infrastructure;

namespace Client.Features.Wiki;

public partial interface IWikiClient
{
    public const string Prefix = $"{Api.Prefix}/wiki";
}