using Refit;
using ErrorOr;

namespace Client.Features.Auth;

public partial interface IAuthClient
{
    public const string GetLoginSchemesPath = $"{Prefix}/login-schemes";
    [Get(GetLoginSchemesPath)]
    public Task<ErrorOr<IReadOnlyCollection<LoginSchemeModel>>> GetLoginSchemes(CancellationToken ct = default);
}