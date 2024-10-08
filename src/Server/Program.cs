using System.Text.Json;
using Client.Features.Shared;
using Contracts;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Server;
using Server.Data;
using Server.Features.Auth;
using Server.Features.Files;
using Server.Features.Notion;
using Server.Features.Shared;
using UserOptions = Server.Features.Auth.UserOptions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(kestrel => kestrel.Limits.MaxRequestBodySize = int.MaxValue);

builder.Logging.AddSerilog();
builder.Services.AddSerilog(logger =>
{
    logger.ReadFrom.Configuration(builder.Configuration);
});

builder.Services.AddDataProtection()
    .PersistKeysToDbContext<DataContext>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddDbContextFactory<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});
builder.Services.AddAuthSetup(builder.Configuration);
builder.Services.AddScoped<RoleSeeder>();

builder.Services.AddNotionClient(client =>
{
    client.AuthToken = builder.Configuration["Notion:AuthToken"];
});
builder.Services.AddOptions<NotionConfiguration>()
    .BindConfiguration("Notion")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options =>
{
    options.CustomSchemaIds(x => x.FullName?.Replace("+", ".", StringComparison.Ordinal));
});

builder.Services.AddFileStorage(builder.Environment);
builder.Services.AddOptions<UserOptions>()
    .BindConfiguration(UserOptions.Section)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddScoped<BackendClient>()
    .AutoRegisterFromServer();

builder.Services.AddHttpContextFeatures();

builder.Services.AddHandlers();
builder.Services.Configure<JsonSerializerOptions>(options => options.SetDefaults());
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.SetDefaults());
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options => options.JsonSerializerOptions.SetDefaults());

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    await scope.ServiceProvider.GetRequiredService<DataContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<RoleSeeder>().SeedAsync();
}

app.MapGet("health", () => TypedResults.Ok()).WithTags("Utility");

if (app.Configuration["ExternalHost"] is { Length: > 0 } host)
{
    var hostUrl = new Uri(host);
    app.Use((http, next) =>
    {
        http.Request.Host = new HostString(hostUrl.Host);
        http.Request.Scheme = hostUrl.Scheme;
        return next.Invoke();
    });
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.Use(async (http, next) => await next.Invoke(http));

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.Use(async (http, next) => await next.Invoke(http));

app.MapServerEndpoints();
app.MapRazorComponents<App>()   
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

app.Run();
