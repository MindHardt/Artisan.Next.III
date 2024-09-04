using Contracts;
using ErrorOr;
using Server.Features.Shared;

namespace Server.Features.Auth;

[RegisterScoped]
public class AuthClient(IServiceProvider sp) : IAuthClient
{
    public async Task<ErrorOr<IReadOnlyCollection<LoginSchemeModel>>> GetLoginSchemes(CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<GetLoginSchemes.Handler>();
        return (await handler.HandleAsync(new EmptyRequest(), ct)).AsErrorOr<IReadOnlyCollection<LoginSchemeModel>>();
    }

    public async Task<Error?> UpdateProfile(Contracts.UpdateProfile.Request request, CancellationToken ct = default)
    {
        var handler = sp.GetRequiredService<UpdateProfile.Handler>();
        return (await handler.HandleAsync(request, ct)).AsPossibleError();
    }
}