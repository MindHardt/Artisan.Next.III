using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Arklens.Alids;
using Client.Features.Files;
using Client.Features.Wiki.Books;

namespace Client.Infrastructure;

public static class JsonSerializerDefaults
{
    public static void SetDefaults(this JsonSerializerOptions options)
    {
        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new AlidJsonConverterFactory());
        options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.PropertyNameCaseInsensitive = true;
        options.TypeInfoResolver = new CombinedJsonTypeInfoResolver(
            VogenSerializerContext.Default, new DefaultJsonTypeInfoResolver());
    }
    
}

internal class CombinedJsonTypeInfoResolver(params IJsonTypeInfoResolver[] resolvers) : IJsonTypeInfoResolver
{
    public JsonTypeInfo? GetTypeInfo(Type type, JsonSerializerOptions options) => resolvers
        .Select(x => x.GetTypeInfo(type, options))
        .FirstOrDefault(x => x is not null);
}

[JsonSerializable(typeof(BookUrlName))]
[JsonSerializable(typeof(FileIdentifier))]
[JsonSerializable(typeof(FileHashString))]
[JsonSerializable(typeof(FileSize))]
internal partial class VogenSerializerContext : JsonSerializerContext;