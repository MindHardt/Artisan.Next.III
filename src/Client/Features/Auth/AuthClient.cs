using System.Net.Http.Json;
using System.Text.Json;
using Client.Features.Shared;
using Contracts;
using ErrorOr;
using Microsoft.Extensions.Options;

namespace Client.Features.Auth;

[RegisterScoped]
public class AuthClient(HttpClient http, IOptions<JsonSerializerOptions> jsonOptions) : IAuthClient
{
    public async Task<ErrorOr<IReadOnlyCollection<LoginSchemeModel>>> GetLoginSchemes(CancellationToken ct = default)
    {
        return await http.GetAsync(Contracts.GetLoginSchemes.FullPath, ct)
            .AsErrorOr<IReadOnlyCollection<LoginSchemeModel>>(jsonOptions.Value, ct);
    }

    public Task<Error?> UpdateProfile(UpdateProfile.Request request, CancellationToken ct = default)
    {
        return http.PutAsJsonAsync(Contracts.UpdateProfile.FullPath, request, jsonOptions.Value, ct)
            .AsPossibleError();
    }
}