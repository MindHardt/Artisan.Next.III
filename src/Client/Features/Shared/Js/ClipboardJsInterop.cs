using Microsoft.JSInterop;

namespace Client.Features.Shared.Js;

[RegisterScoped]
public class ClipboardJsInterop(IJSRuntime jsRuntime) : JsInterop(jsRuntime)
{
    protected override string JsFilePath => "js/clipboard.local.js";

    public async ValueTask Copy(string text)
    {
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync("copy", text);
    }
}