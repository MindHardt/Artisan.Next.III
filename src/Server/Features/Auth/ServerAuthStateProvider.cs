using System.Security.Claims;
using Client.Features.Auth;
using Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Features.Auth;

public class ServerAuthStateProvider(
    ClaimsPrincipal principal,
    PersistentComponentState persistence,
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
        var dbUser = await dataContext.Users
            .Select(user => new
            { 
                user.Id,
                user.UserName,
                user.AvatarUrl,
                
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
            .FirstAsync(user => user.Id == userId);

        var user = new UserModel(
            dbUser.Id,
            dbUser.UserName!,
            dbUser.AvatarUrl,
            dbUser.Roles,
            dbUser.Logins);

        persistence.RegisterOnPersisting(() =>
        {
            persistence.PersistAsJson(nameof(UserModel), user);
            return Task.CompletedTask;
        });

        return new AuthenticationState(user.AsPrincipal());
    }
}