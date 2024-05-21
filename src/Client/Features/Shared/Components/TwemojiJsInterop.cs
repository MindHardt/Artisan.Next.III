using Microsoft.JSInterop;

namespace Client.Features.Shared.Components;

[RegisterScoped]
public class TwemojiJsInterop(IJSRuntime jsRuntime) : JsInterop(jsRuntime)
{
    protected override string JsFilePath => "./Features/Shared/Components/Twemoji.razor.js";

    public async ValueTask UpdateTwemoji(string elementId, string text)
        => await (await GetModuleAsync()).InvokeVoidAsync("updateTwemoji", elementId, text);
}