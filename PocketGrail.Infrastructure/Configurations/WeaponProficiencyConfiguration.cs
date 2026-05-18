namespace PocketGrail.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.Domain.Entities.Proficiencies;

internal sealed class WeaponProficiencyConfiguration : IEntityTypeConfiguration<WeaponProficiency>
{
    public void Configure(EntityTypeBuilder<WeaponProficiency> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
