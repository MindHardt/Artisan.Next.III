using Client.Features.Auth;
using Client.Infrastructure;
using ErrorOr;

namespace Server.Features.Auth;

[RegisterScoped]
public class ServerAuthClient(IServiceProvider sp) : IAuthClient
{
    public Task<Error?> UpdateProfile(IAuthClient.UpdateProfileRequest request, CancellationToken ct = default)
        => throw new NotSupportedException();

    public async Task<ErrorOr<IReadOnlyCollection<LoginSchemeModel>>> GetLoginSchemes(CancellationToken ct = default)
        => await sp.GetRequiredService<GetLoginSchemes.Handler>().HandleAsync(new EmptyRequest(), ct);
}