namespace PocketGrail.DataAccess.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.DataAccess.Entities.ClassEntities;

internal sealed class SubclassFeatureConfiguration : IEntityTypeConfiguration<SubclassFeature>
{
    public void Configure(EntityTypeBuilder<SubclassFeature> builder)
    {
        builder.Property(sf => sf.GainingLevel).IsRequired();

        builder.HasOne(sf => sf.SourceSubclass)
            .WithMany(s => s.SubclassFeatures)
            .HasForeignKey(sf => sf.SubclassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(sf => sf.WeaponGrants)
            .WithMany()
            .UsingEntity("SubclassFeatureWeaponGrants");

        builder.HasMany(sf => sf.ArmorGrants)
            .WithMany()
            .UsingEntity("SubclassFeatureArmorGrants");

        builder.HasMany(sf => sf.LanguageGrants)
            .WithMany()
            .UsingEntity("SubclassFeatureLanguageGrants");

        builder.HasMany(sf => sf.InstrumentGrants)
            .WithMany()
            .UsingEntity("SubclassFeatureInstrumentGrants");
    }
}
