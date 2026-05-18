namespace PocketGrail.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.Domain.Entities.ClassEntities;

internal sealed class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
        builder.Property(c => c.HitDice).IsRequired().HasMaxLength(10);
        builder.Property(c => c.SpellAbility).HasMaxLength(20);
        builder.Property(c => c.SkillChoiceCount).IsRequired();

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasMany(c => c.SavingThrows)
            .WithOne(st => st.Class)
            .HasForeignKey(st => st.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.ClassFeatures)
            .WithOne(cf => cf.SourceClass)
            .HasForeignKey(cf => cf.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Subclasses)
            .WithOne(s => s.SourceClass)
            .HasForeignKey(s => s.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.SpellSlotTemplates)
            .WithOne(t => t.Class)
            .HasForeignKey(t => t.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.MulticlassPrerequisites)
            .WithOne(p => p.Class)
            .HasForeignKey(p => p.ClassId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
