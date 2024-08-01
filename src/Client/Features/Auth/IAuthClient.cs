using Client.Infrastructure;

namespace Client.Features.Auth;

public partial interface IAuthClient
{
    public const string Prefix = $"{Api.Prefix}/auth";
}