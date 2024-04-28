using System.Security.Claims;
using Client.Features.Auth;
using Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Features.Auth;

public class ServerAuthStateProvider(
    ClaimsPrincipal principal,
    PersistentComponentState persistence,
    SignInManager<User> signInManager,
    IDbContextFactory<DataContext> dbContextFactory) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (principal.Identity?.IsAuthenticated is not true)
        {
            return new AuthenticationState(new ClaimsPrincipal());
        }

        await using var dataContext = await dbContextFactory.CreateDbContextAsync();

        var userId = principal.GetUserId()!.Value;
        // ReSharper disable AccessToDisposedClosure
        var queryResult = await dataContext.Users
            .Where(user => user.Id == userId)
            .Select(user => new
            { 
                User = user,
                
                Roles = dataContext.UserRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Select(ur => dataContext.Roles
                        .First(role => role.Id == ur.RoleId).Name!)
                    .ToArray(),
                
                Logins = dataContext.UserLogins
                    .Where(login => login.UserId == user.Id)
                    .Select(login => login.LoginProvider)
                    .ToArray()
                
            })
            .FirstAsync();

        await signInManager.SignInWithClaimsAsync(queryResult.User, isPersistent: true,
        [
            new Claim(CustomClaims.AvatarUrl, queryResult.User.AvatarUrl),
            ..queryResult.Logins.Select(login => new Claim(CustomClaims.LoginScheme, login))
        ]);
        
        var user = new UserModel(
            queryResult.User.Id,
            queryResult.User.UserName!,
            queryResult.User.AvatarUrl,
            queryResult.Roles,
            queryResult.Logins);
        
        persistence.RegisterOnPersisting(() =>
        {
            persistence.PersistAsJson(nameof(UserModel), user);
            return Task.CompletedTask;
        });

        return new AuthenticationState(user.AsPrincipal());
    }
}