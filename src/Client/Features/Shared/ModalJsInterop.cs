using Microsoft.JSInterop;

namespace Client.Features.Shared;

[RegisterScoped]
public class ModalJsInterop(IJSRuntime jsRuntime) : JsInterop(jsRuntime)
{
    protected override string JsFilePath => "js/modals.local.js";

    public async ValueTask ShowModal(string selector)
        => await (await GetModuleAsync()).InvokeVoidAsync("showModal", selector);

    public ValueTask ShowProfileModal() => ShowModal("#UserInfoModal");
    public ValueTask ShowLoginModal() => ShowModal("#LoginModal");
}