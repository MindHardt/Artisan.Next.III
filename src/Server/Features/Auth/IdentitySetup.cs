using System.Net;
using System.Security.Claims;
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

        var auth = services.AddAuthentication(IdentityConstants.ApplicationScheme);
        auth.AddIdentityCookies(options => options.ApplicationCookie!.Configure(cookie =>
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
        if (configuration.GetSection("OAuth:Google").Exists())
        {
            auth.AddGoogle(options =>
            {
                options.ClientId = configuration["OAuth:Google:ClientId"]!;
                options.ClientSecret = configuration["OAuth:Google:ClientSecret"]!;
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.CallbackPath = "/signin-google";
                options.ClaimActions.Add(new CustomJsonClaimAction(CustomClaims.AvatarUrl,
                    ClaimValueTypes.String, json =>
                    {
                        var lowResUrl = json.GetProperty("picture").GetString()!;
                        return lowResUrl[..lowResUrl.IndexOf('=')] + "=s288-c";
                    }));
            });
        }

        if (configuration.GetSection("OAuth:Yandex").Exists())
        {
            auth.AddYandex(options =>
            {
                options.ClientId = configuration["OAuth:Yandex:ClientId"]!;
                options.ClientSecret = configuration["OAuth:Yandex:ClientSecret"]!;
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.CallbackPath = "/signin-yandex";
                options.ClaimActions.Add(new CustomJsonClaimAction(CustomClaims.AvatarUrl,
                    ClaimValueTypes.String, json =>
                    {
                        var avatarId = json.GetProperty("default_avatar_id").GetString()!;
                        return $"https://avatars.yandex.net/get-yapic/{avatarId}/islands-200";
                    }));
            });
        }

        if (configuration.GetSection("OAuth:Notion").Exists())
        {
            auth.AddNotion(options =>
            {
                options.ClientId = configuration["OAuth:Notion:ClientId"]!;
                options.ClientSecret = configuration["OAuth:Notion:ClientSecret"]!;
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.CallbackPath = "/signin-notion";
                options.ClaimActions.Add(new CustomJsonClaimAction(CustomClaims.AvatarUrl,
                    ClaimValueTypes.String,
                    json => json.GetProperty("owner").GetProperty("user").GetProperty("avatar_url").GetString()));
                options.ClaimActions.Add(new CustomJsonClaimAction(ClaimTypes.NameIdentifier,
                    ClaimValueTypes.String,
                    json => json.GetProperty("owner").GetProperty("user").GetProperty("id").GetString()));
                options.ClaimActions.Add(new CustomJsonClaimAction(ClaimTypes.Name,
                    ClaimValueTypes.String,
                    json => json.GetProperty("owner").GetProperty("user").GetProperty("name").GetString()));
            });
        }

        services.AddAuthorization();
        services.AddScoped<AuthenticationStateProvider, ServerAuthStateProvider>();

        return services;
    }
}