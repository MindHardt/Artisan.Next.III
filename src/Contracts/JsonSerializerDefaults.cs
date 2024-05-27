using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Arklens.Alids;

namespace Contracts;

public static class JsonSerializerDefaults
{
    public static void SetDefaults(this JsonSerializerOptions options)
    {
        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new AlidJsonConverterFactory());
        options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.PropertyNameCaseInsensitive = true;
    }
}