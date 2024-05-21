using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Client.Features.Shared.Components;

[RegisterScoped]
public class ShowdownJsInterop(IJSRuntime jsRuntime) : JsInterop(jsRuntime)
{
    protected override string JsFilePath => "./Features/Shared/Components/RenderedMd.razor.js";

    public async ValueTask<MarkupString> RenderMd(string markdown)
        => (MarkupString)await (await GetModuleAsync()).InvokeAsync<string>("renderMd", markdown);

    public async ValueTask RenderTo(string markdown, string elementId)
        => await (await GetModuleAsync()).InvokeVoidAsync("renderTo", markdown, elementId);
}