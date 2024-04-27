namespace Server.Features.Auth;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpContextFeatures(this IServiceCollection services) => services
        .AddScoped(sp => sp.GetRequiredService<IHttpContextAccessor>().HttpContext!)
        .AddScoped(sp => sp.GetRequiredService<IHttpContextAccessor>().HttpContext!.User)
        .AddScoped(sp => sp.GetRequiredService<IHttpContextAccessor>().HttpContext!.Request)
        .AddScoped(sp => sp.GetRequiredService<IHttpContextAccessor>().HttpContext!.Response);
}