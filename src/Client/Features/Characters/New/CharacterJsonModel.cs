using System.Text.Encodings.Web;
using System.Text.Json;
using Arklens;
using Arklens.Alids;
using Arklens.Classes;
using Arklens.Races;

namespace Client.Features.Characters.New;

public record CharacterJsonModel
{
    public Gender? Gender { get; set; }
    public string? Name { get; set; }
    public Race? Race { get; set; }
    public RaceImpacts? RaceImpacts { get; set; }
    public Class? Class { get; set; }
    public Alignment? Alignment { get; set; }
    public Subclass? Subclass { get; set; }
    public required CharacteristicsJsonModel Characteristics { get; set; }
    public string? AvatarBase64 { get; set; }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new AlidJsonConverterFactory() },
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static CharacterJsonModel FromBuilder(CharacterBuilder builder) => new()
    {
        Gender = builder.Gender,
        Name = builder.Name,
        Race = builder.Race,
        RaceImpacts = builder.RaceImpacts,
        Class = builder.Class,
        Subclass = builder.Subclass,
        Alignment = builder.Alignment,
        AvatarBase64 = builder.AvatarBase64,
        Characteristics = CharacteristicsJsonModel.FromBuilder(builder.Characteristics)
    };

    public CharacterBuilder ToBuilder() => new()
    {
        Name = Name,

        RaceImpacts = RaceImpacts,
        Race = Race,

        Class = Class,
        Subclass = Subclass,

        Gender = Gender,
        Alignment = Alignment,

        AvatarBase64 = AvatarBase64,

        Characteristics =
        {
            ["💪"] = Characteristics.Strength,
            ["🏃"] = Characteristics.Dexterity,
            ["🧡"] = Characteristics.Constitution,
            ["🧠"] = Characteristics.Intelligence,
            ["🦉"] = Characteristics.Wisdom,
            ["👄"] = Characteristics.Charisma
        }
    };
}

public record CharacteristicsJsonModel(
    int Strength,
    int Dexterity,
    int Constitution,
    int Intelligence,
    int Wisdom,
    int Charisma)
{
    public static CharacteristicsJsonModel FromBuilder(CharacteristicsBuilder builder) => new(
        builder["💪"], builder["🏃"], builder["🧡"],
        builder["🧠"], builder["🦉"], builder["👄"]);
}