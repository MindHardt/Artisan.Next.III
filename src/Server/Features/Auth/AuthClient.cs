using Contracts;

namespace Server.Features.Auth;

[RegisterScoped]
public class AuthClient(IServiceProvider sp) : IAuthClient
{
    public async Task<LoginSchemeModel[]> GetLoginSchemes(CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<GetLoginSchemes.Handler>();
        var result = await handler.HandleAsync(new EmptyRequest(), ct);

        return result.Value!;
    }

    public async Task UpdateProfile(Contracts.UpdateProfile.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<UpdateProfile.Handler>();
        await handler.HandleAsync(request, ct);
    }
}