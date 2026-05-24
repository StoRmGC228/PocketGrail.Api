namespace PocketGrail.DataAccess.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.DataAccess.Entities;

internal sealed class CampaignParticipantConfiguration : IEntityTypeConfiguration<CampaignParticipant>
{
    public void Configure(EntityTypeBuilder<CampaignParticipant> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Role)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.HasOne(p => p.Campaign)
            .WithMany(c => c.Participants)
            .HasForeignKey(p => p.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => new { p.CampaignId, p.UserId })
            .IsUnique();
    }
}
