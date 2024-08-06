using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Features.BoardGames;

public class SignalRClient(IWebAssemblyHostEnvironment env)
{
    public HubConnection Connection { get; } = new HubConnectionBuilder()
        .WithUrl($"{env.BaseAddress}/signalr")
        .WithAutomaticReconnect()
        .Build();

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        await Connection.StartAsync(ct);
        Connection.Rea
    }
}