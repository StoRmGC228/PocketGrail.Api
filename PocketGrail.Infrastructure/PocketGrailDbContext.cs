namespace PocketGrail.Infrastructure;

using Microsoft.EntityFrameworkCore;
using PocketGrail.Domain.Entities;

internal sealed class PocketGrailDbContext : DbContext
{
    public PocketGrailDbContext(DbContextOptions<PocketGrailDbContext> options) : base(options)
    {
    }

    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<CampaignParticipant> CampaignParticipants => Set<CampaignParticipant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Character> Characters => Set<Character>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Spell> Spells => Set<Spell>();
    public DbSet<Feat> Feats => Set<Feat>();
    public DbSet<Feature> Features => Set<Feature>();
    public DbSet<Proficiency> Proficiencies => Set<Proficiency>();
    public DbSet<SpellSlot> SpellSlots => Set<SpellSlot>();
    public DbSet<CharacterWallet> CharacterWallets => Set<CharacterWallet>();
    public DbSet<CharacterItem> CharacterItems => Set<CharacterItem>();
    public DbSet<CharacterSpell> CharacterSpells => Set<CharacterSpell>();
    public DbSet<CharacterFeat> CharacterFeats => Set<CharacterFeat>();
    public DbSet<CharacterFeature> CharacterFeatures => Set<CharacterFeature>();
    public DbSet<CharacterProficiency> CharacterProficiencies => Set<CharacterProficiency>();
    public DbSet<CharacterClass> CharacterClasses => Set<CharacterClass>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PocketGrailDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}