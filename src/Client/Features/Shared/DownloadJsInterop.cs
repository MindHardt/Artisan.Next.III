using System.Text;
using Microsoft.JSInterop;

namespace Client.Features.Shared;

[RegisterScoped]
public class DownloadJsInterop(IJSRuntime jsRuntime) : JsInterop(jsRuntime)
{
    protected override string JsFilePath => "js/download.local.js";

    public async Task Download(string fileName, Stream content)
    {
        var streamRef = new DotNetStreamReference(content);
        await (await GetModuleAsync()).InvokeVoidAsync("downloadFile", fileName, streamRef);
    }

    public Task Download(string fileName, string content, Encoding? encoding = null)
    {
        var stream = new MemoryStream((encoding ?? Encoding.UTF8).GetBytes(content));
        return Download(fileName, stream);
    }
}