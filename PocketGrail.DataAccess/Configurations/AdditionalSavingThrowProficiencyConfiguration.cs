namespace PocketGrail.DataAccess.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.DataAccess.Entities.Proficiencies;

internal sealed class AdditionalSavingThrowProficiencyConfiguration : IEntityTypeConfiguration<AdditionalSavingThrowProficiency>
{
    public void Configure(EntityTypeBuilder<AdditionalSavingThrowProficiency> builder)
    {
        builder.HasKey(st => st.Id);

        builder.Property(st => st.Ability)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(st => new { st.CharacterProficienciesId, st.Ability })
            .IsUnique();

        builder.HasOne(st => st.CharacterProficiencies)
            .WithMany(cp => cp.AdditionalSavingThrows)
            .HasForeignKey(st => st.CharacterProficienciesId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
