namespace PocketGrail.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.Domain.Entities;

internal sealed class RaceFeatureConfiguration : IEntityTypeConfiguration<RaceFeature>
{
    public void Configure(EntityTypeBuilder<RaceFeature> builder)
    {
        builder.HasOne(rf => rf.SourceRace)
            .WithMany(r => r.Features)
            .HasForeignKey(rf => rf.RaceId);
    }
}
