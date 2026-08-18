namespace PocketGrail.DataAccess.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.DataAccess.Entities.Characters;

internal sealed class CharacterClassConfiguration : IEntityTypeConfiguration<CharacterClass>
{
    public void Configure(EntityTypeBuilder<CharacterClass> builder)
    {
        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.ClassLevel).IsRequired();
        builder.Property(cc => cc.TotalHitDiceCount).IsRequired();

        builder.HasOne(cc => cc.Character)
            .WithMany(c => c.Classes)
            .HasForeignKey(cc => cc.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cc => cc.Class)
            .WithMany(c => c.Characters)
            .HasForeignKey(cc => cc.ClassId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cc => cc.CharacterSubclass)
            .WithMany()
            .HasForeignKey(cc => cc.CharacterSubclassId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(cc => new { cc.CharacterId, cc.ClassId })
            .IsUnique();
    }
}
