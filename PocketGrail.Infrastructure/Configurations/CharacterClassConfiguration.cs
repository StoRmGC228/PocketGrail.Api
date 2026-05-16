namespace PocketGrail.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.Domain.Entities;

internal sealed class CharacterClassConfiguration : IEntityTypeConfiguration<CharacterClass>
{
    public void Configure(EntityTypeBuilder<CharacterClass> builder)
    {
        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.ClassName).IsRequired().HasMaxLength(50);
        builder.Property(cc => cc.HitDice).IsRequired().HasMaxLength(4);
        builder.Property(cc => cc.Subclass).HasMaxLength(100);
        builder.Property(cc => cc.ClassLevel).IsRequired();
        builder.Property(cc => cc.TotalHitDice).IsRequired();
        builder.Property(cc => cc.UsedHitDice).IsRequired();

        builder.HasOne(cc => cc.Character)
            .WithMany(c => c.Classes)
            .HasForeignKey(cc => cc.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(cc => new { cc.CharacterId, cc.ClassName }).IsUnique();
    }
}