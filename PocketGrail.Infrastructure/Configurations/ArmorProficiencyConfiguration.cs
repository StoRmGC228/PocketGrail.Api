namespace PocketGrail.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.Domain.Entities.Proficiencies;

internal sealed class ArmorProficiencyConfiguration : IEntityTypeConfiguration<ArmorProficiency>
{
    public void Configure(EntityTypeBuilder<ArmorProficiency> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
