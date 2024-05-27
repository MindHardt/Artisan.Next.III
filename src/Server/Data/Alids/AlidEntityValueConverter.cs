using Arklens.Alids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Server.Data.Alids;

internal class AlidEntityValueConverter<TEntity> : ValueConverter<TEntity, string>
    where TEntity : IAlidEntity
{
    public AlidEntityValueConverter() : base(
        entity => entity.Alid.Value,
        alid => IAlidEntity.Get<TEntity>(Alid.Create(alid)))
    { }
}

internal static class AlidEntityValueConverterExtensions
{
    public static ModelConfigurationBuilder ConfigureAlidEntityConversions(this ModelConfigurationBuilder model)
    {
        var alidTypes = typeof(Alid).Assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IAlidEntity)));
        foreach (var alidType in alidTypes)
        {
            var converterType = typeof(AlidEntityValueConverter<>).MakeGenericType(alidType);
            model.Properties(alidType)
                .HaveConversion(converterType)
                .HaveMaxLength(Alid.MaxLength)
                .UseCollation("C");
        }

        return model;
    }
}