using Contracts;
using Server.Data;

namespace Server.Features.Auth;

public static class UserExtensions
{
    public static IQueryable<UserModel> ProjectToUserModel(this IQueryable<User> query, DataContext dataContext) =>
        query.Select(user => new UserModel(
            user.Id,
            user.UserName!,
            user.AvatarUrl,
            dataContext.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => dataContext.Roles
                    .Where(role => role.Id == ur.RoleId)
                    .Select(role => role.Name!)
                    .First())
                .ToArray(),
            dataContext.UserLogins
                .Where(ul => ul.UserId == user.Id)
                .Select(ul => ul.ProviderDisplayName ?? ul.LoginProvider)
                .ToArray()));
}