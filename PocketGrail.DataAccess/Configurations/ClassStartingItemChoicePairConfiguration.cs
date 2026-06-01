namespace PocketGrail.DataAccess.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.DataAccess.Entities.ClassEntities;

internal sealed class ClassStartingItemChoicePairConfiguration : IEntityTypeConfiguration<ClassStartingItemChoicePair>
{
    public void Configure(EntityTypeBuilder<ClassStartingItemChoicePair> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasMany(p => p.OptionA)
            .WithMany()
            .UsingEntity("ClassStartingItemChoicePairOptionA");

        builder.HasMany(p => p.OptionB)
            .WithMany()
            .UsingEntity("ClassStartingItemChoicePairOptionB");
    }
}
