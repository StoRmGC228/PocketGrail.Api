namespace PocketGrail.DataAccess.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.DataAccess.Entities.ClassEntities;

internal sealed class SubclassConfiguration : IEntityTypeConfiguration<Subclass>
{
    public void Configure(EntityTypeBuilder<Subclass> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.ShortDescription).HasMaxLength(500);
    }
}
