using System.Text.Json;
using Arklens.Alids;
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
builder.Services.AddYandexFrames();

builder.Services.Configure<JsonSerializerOptions>(options => options.SetDefaults());

var app = builder.Build();

app.Services.GetRequiredService<ILogger<IAlidEntity>>().LogInformation("Loaded {Count} alid entities: {Entities}",
    IAlidEntity.AllValues.Count, IAlidEntity.AllValues.Select(x => $"\n{x.Alid.Value}").Order());

await app.RunAsync();
