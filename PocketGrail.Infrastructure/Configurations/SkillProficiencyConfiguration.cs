namespace PocketGrail.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.Domain.Entities.Proficiencies;

internal sealed class SkillProficiencyConfiguration : IEntityTypeConfiguration<SkillProficiency>
{
    public void Configure(EntityTypeBuilder<SkillProficiency> builder)
    {
        builder.HasKey(sp => sp.Id);

        builder.Property(sp => sp.Skill)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(sp => sp.HasExpertise)
            .IsRequired();

        builder.HasIndex(sp => new { sp.CharacterProficienciesId, sp.Skill })
            .IsUnique();

        builder.HasOne(sp => sp.CharacterProficiencies)
            .WithMany(cp => cp.Skills)
            .HasForeignKey(sp => sp.CharacterProficienciesId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
