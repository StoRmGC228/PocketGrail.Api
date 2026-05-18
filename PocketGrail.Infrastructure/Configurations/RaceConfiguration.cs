namespace PocketGrail.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.Domain.Entities;

internal sealed class RaceConfiguration : IEntityTypeConfiguration<Race>
{
    public void Configure(EntityTypeBuilder<Race> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).IsRequired().HasMaxLength(50);
        builder.HasIndex(r => r.Name).IsUnique();

        builder.HasMany(r => r.Features)
            .WithOne(f => f.SourceRace)
            .HasForeignKey(f => f.RaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.WeaponGrants)
            .WithMany()
            .UsingEntity("RaceWeaponGrants");

        builder.HasMany(r => r.ArmorGrants)
            .WithMany()
            .UsingEntity("RaceArmorGrants");

        builder.HasMany(r => r.LanguageGrants)
            .WithMany()
            .UsingEntity("RaceLanguageGrants");

        builder.HasMany(r => r.InstrumentGrants)
            .WithMany()
            .UsingEntity("RaceInstrumentGrants");
    }
}
