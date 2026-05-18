namespace PocketGrail.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.Domain.Entities.ClassEntities;

internal sealed class ClassSpellSlotTemplateConfiguration : IEntityTypeConfiguration<ClassSpellSlotTemplate>
{
    public void Configure(EntityTypeBuilder<ClassSpellSlotTemplate> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.ClassLevel).IsRequired();
        builder.Property(t => t.SpellSlotLevel).IsRequired();
        builder.Property(t => t.TotalSlots).IsRequired();
        builder.HasIndex(t => new { t.ClassId, t.ClassLevel, t.SpellSlotLevel }).IsUnique();
    }
}
