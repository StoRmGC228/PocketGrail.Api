namespace PocketGrail.DataAccess.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.DataAccess.Entities.ClassEntities;

internal sealed class ClassFeatureConfiguration : IEntityTypeConfiguration<ClassFeature>
{
    public void Configure(EntityTypeBuilder<ClassFeature> builder)
    {
        builder.Property(cf => cf.GainingLevel).IsRequired();

        builder.HasMany(cf => cf.WeaponGrants)
            .WithMany()
            .UsingEntity("ClassFeatureWeaponGrants");

        builder.HasMany(cf => cf.ArmorGrants)
            .WithMany()
            .UsingEntity("ClassFeatureArmorGrants");

        builder.HasMany(cf => cf.LanguageGrants)
            .WithMany()
            .UsingEntity("ClassFeatureLanguageGrants");

        builder.HasMany(cf => cf.InstrumentGrants)
            .WithMany()
            .UsingEntity("ClassFeatureInstrumentGrants");
    }
}
