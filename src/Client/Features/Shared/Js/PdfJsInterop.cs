using Microsoft.JSInterop;

namespace Client.Features.Shared.Js;

[RegisterScoped]
public class PdfJsInterop(IJSRuntime jsRuntime) : JsInterop(jsRuntime)
{
    protected override string JsFilePath => "js/pdf.local.js";
    public async ValueTask DownloadSvg(string svgSelector, string fileName, PageSize pageSize)
    {
        var (width, height) = pageSize switch
        {
            PageSize.A4 => (595, 840),
            PageSize.A5 => (420, 595),
            _ => throw new NotSupportedException()
        };
        await (await GetModuleAsync()).InvokeVoidAsync("downloadSvg", 
            svgSelector, width, height, fileName);
    }
}

public enum PageSize
{
    A4,
    A5
}