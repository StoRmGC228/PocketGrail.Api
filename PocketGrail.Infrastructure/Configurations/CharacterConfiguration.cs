namespace PocketGrail.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.Domain.Entities.Characters;

internal sealed class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Race).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Level).IsRequired();
        builder.Ignore(c => c.ProficiencyBonus);
        builder.Property(c => c.CurrentHp).IsRequired();
        builder.Property(c => c.MaxHp).IsRequired();
        builder.Property(c => c.ImageUrl).HasMaxLength(500);
        builder.Property(c => c.Alignment).HasMaxLength(50);
        builder.Property(c => c.BackgroundStory).HasMaxLength(2000);
        builder.Property(c => c.Appearance).HasMaxLength(1000);
        builder.Property(c => c.Notes).HasMaxLength(2000);

        builder.HasOne(c => c.Owner)
            .WithMany()
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Campaign)
            .WithMany(campaign => campaign.Characters)
            .HasForeignKey(c => c.CampaignId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.HasOne(c => c.Wallet)
            .WithOne(w => w.Character)
            .HasForeignKey<CharacterWallet>(w => w.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Proficiencies)
            .WithOne(cp => cp.Character)
            .HasForeignKey<CharacterProficiencies>(cp => cp.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.SpellSlots)
            .WithOne(s => s.Character)
            .HasForeignKey(s => s.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Items)
            .WithMany(i => i.Characters)
            .UsingEntity<CharacterItem>(
                j => j.HasOne(ci => ci.Item).WithMany(i => i.CharacterItems).HasForeignKey(ci => ci.ItemId),
                j => j.HasOne(ci => ci.Character).WithMany().HasForeignKey(ci => ci.CharacterId),
                j =>
                {
                    j.HasKey(ci => new { ci.CharacterId, ci.ItemId });
                    j.Property(ci => ci.Quantity).HasDefaultValue(1);
                });

        builder.HasMany(c => c.Spells)
            .WithMany(s => s.Characters)
            .UsingEntity<CharacterSpell>(
                j => j.HasOne(cs => cs.Spell).WithMany(s => s.CharacterSpells).HasForeignKey(cs => cs.SpellId),
                j => j.HasOne(cs => cs.Character).WithMany().HasForeignKey(cs => cs.CharacterId),
                j => j.HasKey(cs => new { cs.CharacterId, cs.SpellId }));

        builder.HasMany(c => c.Feats)
            .WithMany(f => f.Characters)
            .UsingEntity<CharacterFeat>(
                j => j.HasOne(cf => cf.Feat).WithMany().HasForeignKey(cf => cf.FeatId),
                j => j.HasOne(cf => cf.Character).WithMany().HasForeignKey(cf => cf.CharacterId),
                j => j.HasKey(cf => new { cf.CharacterId, cf.FeatId }));

        builder.HasMany(c => c.Features)
            .WithMany(f => f.Characters)
            .UsingEntity<CharacterFeature>(
                j => j.HasOne(cf => cf.Feature).WithMany(f => f.CharacterFeatures).HasForeignKey(cf => cf.FeatureId),
                j => j.HasOne(cf => cf.Character).WithMany().HasForeignKey(cf => cf.CharacterId),
                j => j.HasKey(cf => new { cf.CharacterId, cf.FeatureId }));
    }
}
