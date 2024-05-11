using System.Text.Json;
using Client.Features.Auth;
using Client.Features.Maps;
using Client.Features.Shared;
using Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient(string.Empty, http =>
    {
        http.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    })
    .AddHttpMessageHandler<HttpErrorHandler>();
builder.Services.AddScoped<HttpErrorHandler>();

builder.Services.AddScoped<BackendClient>();
builder.Services.AutoRegisterFromClient();

builder.Services.AddScoped<AuthenticationStateProvider, ClientAuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IMapFramesProvider, YandexMapFramesProvider>();

builder.Services.Configure<JsonSerializerOptions>(options => options.SetDefaults());

await builder.Build().RunAsync();
