using System.Runtime.CompilerServices;
using Contracts;
using ErrorOr;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Caching.Memory;

namespace Client.Features.Shared;

public class AppContext(BackendClient backend, PersistentComponentState persistence, IServiceProvider sp)
{
    private IMemoryCache Cache => BlazorEnv.IsWasm
        ? sp.GetRequiredService<IMemoryCache>()
        : throw new InvalidOperationException();
    
    public Task<ErrorOr<IReadOnlyCollection<LoginSchemeModel>>> GetLoginSchemes() => GetInternal(
        () => backend.Auth.GetLoginSchemes());

    private async Task<ErrorOr<T>> GetInternal<T>(
        Func<Task<ErrorOr<T>>> fetchMethod,
        bool allowCache = true, 
        [CallerMemberName] string cacheKey = "")
    {
        if (BlazorEnv.IsWasm && allowCache && Cache.Get(cacheKey) is T cached)
        {
            return cached.ToErrorOr();
        }
        
        var result = persistence.TryTakeFromJson(cacheKey, out cached!)
            ? cached.ToErrorOr()
            : await fetchMethod();
            
        if (BlazorEnv.IsServer)
        {
            if (result.IsError is false)
            {
                persistence.RegisterOnPersisting(() =>
                {
                    persistence.PersistAsJson(cacheKey, result.Value);
                    return Task.CompletedTask;
                }, RenderMode.InteractiveWebAssembly);
            }

            return result;
        }

        if (result.IsError is false)
        {
            Cache.Set(cacheKey, result.Value);
        }

        return result;
    }
}