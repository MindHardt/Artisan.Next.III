using Client.Infrastructure;

namespace Client.Features.Notion;

public partial interface INotionClient
{
    public const string Prefix = $"{Api.Prefix}/notion";
}