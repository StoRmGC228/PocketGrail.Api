namespace PocketGrail.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.Domain.Entities.ClassEntities;

internal sealed class ClassSavingThrowProficiencyConfiguration : IEntityTypeConfiguration<ClassSavingThrowProficiency>
{
    public void Configure(EntityTypeBuilder<ClassSavingThrowProficiency> builder)
    {
        builder.HasKey(st => st.Id);

        builder.Property(st => st.Ability)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(st => new { st.ClassId, st.Ability })
            .IsUnique();

        builder.HasOne(st => st.Class)
            .WithMany(c => c.SavingThrows)
            .HasForeignKey(st => st.ClassId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
