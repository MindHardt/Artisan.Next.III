using System.Security.Claims;
using System.Text.Json;
using Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Client.Features.Auth;

public class ClientAuthStateProvider(
    PersistentComponentState state,
    ILogger<ClientAuthStateProvider> logger,
    IOptions<JsonSerializerOptions> jsonOptions) : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (state.TryTakeFromJson(nameof(UserModel), out UserModel? user) && user is not null)
        {
            logger.LogInformation("Fetched auth state {User}", 
                JsonSerializer.Serialize(user, jsonOptions.Value));
            
            return Task.FromResult(new AuthenticationState(user.AsPrincipal()));
        }

        logger.LogInformation("Unable to fetch authentication state");
        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
    }
}