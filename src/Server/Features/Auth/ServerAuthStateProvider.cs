using Client.Features.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Features.Auth;

public class ServerAuthStateProvider : ServerAuthenticationStateProvider, IDisposable
{
    private readonly PersistentComponentState _persistence;
    private readonly DataContext _dataContext;

    private readonly PersistingComponentStateSubscription _sub;
    private Task<AuthenticationState>? _authenticationStateTask;

    public ServerAuthStateProvider(
        PersistentComponentState persistence,
        DataContext dataContext)
    {
        _persistence = persistence;
        _dataContext = dataContext;

        AuthenticationStateChanged += OnAuthenticationStateChanged;
        _sub = _persistence.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
    }

    private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        => _authenticationStateTask = task;

    private async Task OnPersistingAsync()
    {
        ArgumentNullException.ThrowIfNull(_authenticationStateTask);

        var principal = (await _authenticationStateTask).User;
        if (principal.Identity?.IsAuthenticated is not true)
        {
            return;
        }

        var userId = principal.GetUserId()!.Value;
        var queryResult = await _dataContext.Users
            .Where(user => user.Id == userId)
            .Select(user => new
            {
                User = user,

                Roles = _dataContext.UserRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Select(ur => _dataContext.Roles
                        .First(role => role.Id == ur.RoleId).Name!)
                    .ToArray(),

                Logins = _dataContext.UserLogins
                    .Where(login => login.UserId == user.Id)
                    .Select(login => login.LoginProvider)
                    .ToArray()

            })
            .AsSingleQuery()
            .FirstAsync();

        _persistence.PersistAsJson(nameof(UserModel), new UserModel(
            queryResult.User.Id,
            queryResult.User.UserName!,
            queryResult.User.AvatarUrl,
            queryResult.Roles,
            queryResult.Logins));
    }

    public void Dispose()
    {
        _sub.Dispose();
        AuthenticationStateChanged -= OnAuthenticationStateChanged;
        GC.SuppressFinalize(this);
    }
}