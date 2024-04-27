using Client.Features.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Client.Features.Wiki;

[RegisterScoped]
public class ShowdownJsInterop(IJSRuntime jsRuntime) : JsInterop(jsRuntime)
{
    protected override string JsFilePath => "js/showdown.local.js";

    public async ValueTask<MarkupString> RenderMd(string markdown)
    {
        var module = await GetModuleAsync();

        return (MarkupString)await module.InvokeAsync<string>("renderMd", markdown);
    }
}