namespace PocketGrail.DataAccess.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.DataAccess.Entities.ClassEntities;

internal sealed class ClassStartingItemSetConfiguration : IEntityTypeConfiguration<ClassStartingItemSet>
{
    public void Configure(EntityTypeBuilder<ClassStartingItemSet> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasMany(s => s.ChoicePairs)
            .WithOne(p => p.Set)
            .HasForeignKey(p => p.ClassStartingItemSetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
