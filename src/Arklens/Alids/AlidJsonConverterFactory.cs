using System.Text.Json;
using System.Text.Json.Serialization;

namespace Arklens.Alids;

public class AlidJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert.IsAssignableTo(typeof(IAlidEntity));

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => (JsonConverter)Activator.CreateInstance(typeof(AlidJsonConverter<>).MakeGenericType(typeToConvert))!;
}

public class AlidJsonConverter : AlidJsonConverter<IAlidEntity>;
public class AlidJsonConverter<TEntity> : JsonConverter<TEntity>
    where TEntity: IAlidEntity
{
    public override TEntity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => IAlidEntity.GetSingle<TEntity>(Alid.Parse(reader.GetString()!));

    public override void Write(Utf8JsonWriter writer, TEntity value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Alid.ToString());
}