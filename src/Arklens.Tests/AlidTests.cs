using System.Text.Json;
using Arklens.Alids;
using Xunit.Abstractions;

namespace Arklens.Tests;

public class AlidTests(ITestOutputHelper output)
{
    [Theory]
    [InlineData("Test", "test")]
    [InlineData("FooBarBaz", "foo_bar_baz")]
    public void AlidNameNormalizationTests(string originalName, string expectedName)
    {
        var alidName = new AlidName(originalName);
        Assert.Equal(expectedName, alidName.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("кириллица")]
    public void AlidNameExceptionTests(string? malformedAlid)
    {
        Assert.Throws<ArgumentException>(() => new AlidName(malformedAlid));
    }

    [Theory]
    [InlineData("Spell", "Wizard")]
    [InlineData("Foo")]
    public void AlidCollectionExpressionsTests(params string[] names)
    {
        _ = AlidNameCollection.Create(names);
    }

    [Fact]
    public void AlidNameCollectionDefaultTests()
    {
        Assert.Equal(AlidNameCollection.Empty, default);
        Assert.Equal(AlidNameCollection.Create([]), []);
    }

    [Theory]
    [InlineData("spell:wizard:fireball")]
    [InlineData("weapon:longsword+well_made")]
    [InlineData("monster:dragon:dova+ancient")]
    public void AlidParseTests(string value)
    {
        _ = Alid.Parse(value);
    }

    [Theory]
    [InlineData("weapon:#monk")]
    [InlineData("spell:wizard:#cantrips")]
    public void AlidGroupTests(string value)
    {
        var alid = Alid.Parse(value);
        Assert.True(alid.IsGroup);
        Assert.Equal(alid.Value, value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("alid")]
    [InlineData("alid::alid")]
    [InlineData("alid:alid++alid")]
    [InlineData("alid:which_is_too_long_to_be_parsed_and_will_throw_because_max_length_of_alid_is_128_characters_but_this_alid_is_132_characters_long")]
    public void AlidParseExceptionTests(string value)
    {
        Assert.Throws<FormatException>(() => Alid.Parse(value));
    }

    [Fact]
    public void ListAlidValues()
    {
        foreach (var entity in IAlidEntity.All.OrderBy(x => x.Alid.Value))
        {
            output.WriteLine(entity.Alid.ToString());
        }
    }
    
    [Fact]
    public void ListAlidGroups()
    {
        foreach (var entity in AlidGroups.All.OrderBy(x => x.Alid.Value))
        {
            output.WriteLine(entity.Alid.ToString());
        }
    }

    [Fact]
    public void TestJsonConverter()
    {
        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new AlidJsonConverterFactory() }
        };
        foreach (var entity in IAlidEntity.All)
        {
            var json = JsonSerializer.Serialize(entity, jsonOptions);
            var deserialized = JsonSerializer.Deserialize<IAlidEntity>(json, jsonOptions);
            Assert.NotNull(deserialized);
            Assert.Equal(entity, deserialized);
            Assert.IsType(entity.GetType(), deserialized);
        }

        var nerasJson = JsonSerializer.Serialize(Deity.Neras, jsonOptions);
        var deseralizedNeras = JsonSerializer.Deserialize<Deity>(nerasJson, jsonOptions);
        Assert.Same(deseralizedNeras, Deity.Neras);
    }
}