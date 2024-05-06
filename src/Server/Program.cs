using System.Text.Json;
using Client.Features.Shared;
using Contracts;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Server;
using Server.Components;
using Server.Data;
using Server.Features.Auth;
using Server.Features.Files;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog();
builder.Services.AddSerilog(logger =>
{
    logger.ReadFrom.Configuration(builder.Configuration);
});

builder.Services.AddDataProtection()
    .PersistKeysToDbContext<DataContext>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddDbContextFactory<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});
builder.Services.AddAuthSetup(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options =>
{
    options.CustomSchemaIds(x => x.FullName?.Replace("+", ".", StringComparison.Ordinal));
});

builder.Services.AddOptions<FileStorageOptions>()
    .BindConfiguration(FileStorageOptions.Section);

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
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    foreach (var roleName in RoleNames.Enumerate())
    {
        await roleManager.CreateAsync(new IdentityRole<int>(roleName));
    }
}

app.MapGet("health", () => TypedResults.Ok());

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
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapServerEndpoints();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

app.Run();
