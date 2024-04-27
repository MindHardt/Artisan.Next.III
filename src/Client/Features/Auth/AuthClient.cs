using System.Net.Http.Json;
using System.Text.Json;
using Contracts;
using Microsoft.Extensions.Options;

namespace Client.Features.Auth;

[RegisterScoped]
public class AuthClient(HttpClient http, IOptions<JsonSerializerOptions> jsonOptions) : IAuthClient
{
    public async Task<LoginSchemeModel[]> GetLoginSchemes(CancellationToken ct = default)
    {
        return (await http.GetFromJsonAsync<LoginSchemeModel[]>(Contracts.GetLoginSchemes.FullPath, jsonOptions.Value, ct))!;
    }

    public Task UpdateProfile(UpdateProfile.Request request, CancellationToken ct = default)
    {
        return http.PutAsJsonAsync(Contracts.UpdateProfile.FullPath, request, jsonOptions.Value, ct);
    }
}