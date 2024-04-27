using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;

namespace Client.Features.Shared;

public abstract class JsInterop : IAsyncDisposable
{
    /// <summary>
    /// The path of the .js file from the local wwwroot.
    /// The leading '/' symbol is not required.
    /// </summary>
    protected abstract string JsFilePath { get; }

    /// <summary>
    /// The underlying <see cref="IJSRuntime"/> for using
    /// default js functions.
    /// </summary>
    protected IJSRuntime Runtime { get; }

    /// <summary>
    /// The underlying <see cref="IJSRuntime"/> object, wrapped for lazy evaluation.
    /// </summary>
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    /// <summary>
    /// Gets the imported javascript module.
    /// </summary>
    /// <returns></returns>
    protected Task<IJSObjectReference> GetModuleAsync() => _moduleTask.Value;

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    protected JsInterop(IJSRuntime jsRuntime)
    {
        Runtime = jsRuntime;
        var normalizedFileName = JsFilePath.StartsWith('/')
            ? JsFilePath
            : $"/{JsFilePath}";
        _moduleTask = new Lazy<Task<IJSObjectReference>>(() =>
            Runtime.InvokeAsync<IJSObjectReference>("import", normalizedFileName).AsTask());
    }

#pragma warning disable CA1816
    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await GetModuleAsync();
            await module.DisposeAsync();
        }
    }
}