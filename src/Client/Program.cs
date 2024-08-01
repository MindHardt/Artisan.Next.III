using System.Reflection;
using System.Text.Json;
using Arklens.Alids;
using Client.Features.Auth;
using Client.Features.Files;
using Client.Features.Maps;
using Client.Features.Notion;
using Client.Features.Shared;
using Client.Features.Wiki;
using Client.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;
using IAuthClient = Client.Features.Auth.IAuthClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient(string.Empty, http =>
    {
        http.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    })
    .AddHttpMessageHandler<HttpErrorHandler>();
builder.Services.AddScoped<HttpErrorHandler>();

builder.Services.AddScoped<BackendClient>();
builder.Services.AddRefitClient<IAuthClient>(BackendRefitConfiguration.GetSettings).WithBackendPrefix();
builder.Services.AddRefitClient<IFileClient>(BackendRefitConfiguration.GetSettings).WithBackendPrefix();
builder.Services.AddRefitClient<INotionClient>(BackendRefitConfiguration.GetSettings).WithBackendPrefix();
builder.Services.AddRefitClient<IWikiClient>(BackendRefitConfiguration.GetSettings).WithBackendPrefix();

builder.Services.AutoRegisterFromClient();

builder.Services.AddScoped<AuthenticationStateProvider, ClientAuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddMemoryCache();
builder.Services.AddYandexFrames();

builder.Services.Configure<JsonSerializerOptions>(options => options.SetDefaults());

var app = builder.Build();

app.Services.GetRequiredService<ILogger<IAlidEntity>>().LogInformation("Loaded {Count} alid entities: {Entities}",
    IAlidEntity.AllValues.Count, IAlidEntity.AllValues.Select(x => $"\n{x.Alid.Value}").Order());

var refAssemblies = typeof(Program).Assembly.GetReferencedAssemblies();
foreach (var refAsm in refAssemblies)
{
    Assembly.Load(refAsm);
}
app.Services.GetRequiredService<ILogger<Program>>().LogInformation("Loaded {Count} assemblies {Assemblies}",
    refAssemblies.Length, refAssemblies.Select(x => $"\n{x.Name}").Order());

await app.RunAsync();
