using System.Text.Json;
using Client.Features.Auth;
using Client.Features.Shared;
using Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var backendUri = new Uri(builder.HostEnvironment.BaseAddress);

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = backendUri });
builder.Services.AddScoped<BackendClient>();
builder.Services.AutoRegisterFromClient();

builder.Services.AddScoped<AuthenticationStateProvider, ClientAuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddMemoryCache();

builder.Services.Configure<JsonSerializerOptions>(options => options.SetDefaults());

await builder.Build().RunAsync();
