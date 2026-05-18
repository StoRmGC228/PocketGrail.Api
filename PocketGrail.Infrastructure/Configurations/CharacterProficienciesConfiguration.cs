namespace PocketGrail.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.Domain.Entities;
using PocketGrail.Domain.Entities.Characters;
using PocketGrail.Domain.Entities.Proficiencies;

internal sealed class CharacterProficienciesConfiguration : IEntityTypeConfiguration<CharacterProficiencies>
{
    public void Configure(EntityTypeBuilder<CharacterProficiencies> builder)
    {
        builder.HasKey(cp => cp.Id);

        builder.HasOne(cp => cp.Character)
            .WithOne(c => c.Proficiencies)
            .HasForeignKey<CharacterProficiencies>(cp => cp.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(cp => cp.Languages)
            .WithMany()
            .UsingEntity("CharacterProficienciesLanguages");

        builder.HasMany(cp => cp.Instruments)
            .WithMany()
            .UsingEntity("CharacterProficienciesInstruments");

        builder.HasMany(cp => cp.Weapons)
            .WithMany()
            .UsingEntity("CharacterProficienciesWeaponProficiencies");

        builder.HasMany(cp => cp.Armors)
            .WithMany()
            .UsingEntity("CharacterProficienciesArmorProficiencies");
    }
}
