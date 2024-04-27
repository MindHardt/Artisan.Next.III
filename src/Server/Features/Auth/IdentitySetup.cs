using System.Net;
using Contracts;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Server.Data;

namespace Server.Features.Auth;

public static class IdentitySetup
{
    public static IServiceCollection AddAuthSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<User>(options =>
            {
                options.User.AllowedUserNameCharacters =
                    "АаБбВвГгДдЕеËëЖжЗзИиЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЪъЫыЬьЭэЮюЯя" +
                    "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz" +
                    "0123456789+-._@!?*#()[]{} ";
            })
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<DataContext>()
            .AddSignInManager();

        services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddGoogle(options =>
            {
                options.ClientId = configuration["OAuth:Google:ClientId"]!;
                options.ClientSecret = configuration["OAuth:Google:ClientSecret"]!;
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.CallbackPath = "/signin-google";
                options.ClaimActions.Add(new CustomJsonClaimAction(CustomClaims.AvatarUrl,
                    "http://www.w3.org/2001/XMLSchema#string", json =>
                    {
                        var lowResUrl = json.GetProperty("picture").GetString()!;
                        return lowResUrl[..lowResUrl.IndexOf('=')] + "=s288-c";
                    }));
            })
            .AddYandex(options =>
            {
                options.ClientId = configuration["OAuth:Yandex:ClientId"]!;
                options.ClientSecret = configuration["OAuth:Yandex:ClientSecret"]!;
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.CallbackPath = "/signin-yandex";
                options.ClaimActions.Add(new CustomJsonClaimAction(CustomClaims.AvatarUrl, 
                    "http://www.w3.org/2001/XMLSchema#string", json =>
                    {
                        var avatarId = json.GetProperty("default_avatar_id").GetString()!;
                        return $"https://avatars.yandex.net/get-yapic/{avatarId}/islands-200";
                    }));
            }).AddIdentityCookies(options => options.ApplicationCookie!.Configure(cookie =>
            {
                cookie.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                };
                cookie.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Task.CompletedTask;
                };
            }));
        
        services.AddAuthorization();
        services.AddScoped<AuthenticationStateProvider, ServerAuthStateProvider>();

        return services;
    }
}