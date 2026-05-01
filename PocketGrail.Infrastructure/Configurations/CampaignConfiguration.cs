namespace PocketGrail.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.Domain.Entities;

internal sealed class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
{
    public void Configure(EntityTypeBuilder<Campaign> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.PasswordHash)
            .IsRequired();

        builder.Property(c => c.ShortDescription)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.ConnectionCode)
            .IsRequired()
            .HasMaxLength(6);

        builder.HasIndex(c => c.ConnectionCode)
            .IsUnique();

        builder.Property(c => c.ImageUrl)
            .HasMaxLength(2048);

        builder.Property(c => c.IsActive)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.HasOne(c => c.DmOwner)
            .WithMany()
            .HasForeignKey(c => c.DmOwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Participants)
            .WithOne(p => p.Campaign)
            .HasForeignKey(p => p.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
