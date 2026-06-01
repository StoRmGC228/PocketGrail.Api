namespace PocketGrail.DataAccess.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.DataAccess.Entities.ClassEntities;

internal sealed class MulticlassPrerequisiteConfiguration : IEntityTypeConfiguration<MulticlassPrerequisite>
{
    public void Configure(EntityTypeBuilder<MulticlassPrerequisite> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.MinimumScore).IsRequired();
        builder.HasIndex(p => new { p.ClassId, p.RequiredAbility }).IsUnique();
    }
}
