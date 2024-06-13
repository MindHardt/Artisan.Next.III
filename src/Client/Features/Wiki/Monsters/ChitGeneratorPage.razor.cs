using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Client.Features.Shared.Js;
using Client.Features.Shared.Toasts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Client.Features.Wiki.Monsters;

public partial class ChitGeneratorPage : ComponentBase
{
    [GeneratedRegex(@"\$\{(?<NAME>[А-Яа-я]+)(?<ADD>.*?)\}")]
    private static partial Regex PreparationRegex();
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private string PrepareSvg() => PreparationRegex().Replace(_originalSvg!, match =>
    {
        var paramName = match.Groups["NAME"].Value;
        var replacement = _parameters?.GetValueOrDefault(paramName) ?? string.Empty;

        return replacement + match.Groups["ADD"].Value;
    });

    private string GetFileName() => _parameters?["Имя"] ?? $"chit-{DateTimeOffset.Now:s}";

    private async Task ExportJson()
    {
        var json = JsonSerializer.Serialize(_parameters, JsonOptions);
        var fileName = $"{GetFileName()}.json";
        
        await Services.GetRequiredService<DownloadJsInterop>().Download(fileName, json);
    }

    private async Task ImportJson(InputFileChangeEventArgs e)
    {
        var toasts = Services.GetRequiredService<ToastSender>();
        await using var fileContent = e.File.OpenReadStream(long.MaxValue);
        var json = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(fileContent, JsonOptions);
        if (json is not null)
        {
            _parameters = json;
            InitSvgRender();
            await toasts.ShowInfoToast("Успешно импортировано!");
        }
        else
        {
            await toasts.ShowWarningToast("Ошибка при импорте");
        }
    }
}