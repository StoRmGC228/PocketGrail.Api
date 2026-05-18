namespace PocketGrail.Infrastructure;

using Microsoft.EntityFrameworkCore;
using PocketGrail.Domain.Entities;
using PocketGrail.Domain.Entities.Characters;
using PocketGrail.Domain.Entities.ClassEntities;
using PocketGrail.Domain.Entities.Proficiencies;
using PocketGrail.Domain.Entities.Enums;

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
    public DbSet<SpellSlot> SpellSlots => Set<SpellSlot>();
    public DbSet<CharacterWallet> CharacterWallets => Set<CharacterWallet>();
    public DbSet<CharacterItem> CharacterItems => Set<CharacterItem>();
    public DbSet<CharacterSpell> CharacterSpells => Set<CharacterSpell>();
    public DbSet<CharacterFeat> CharacterFeats => Set<CharacterFeat>();

    public DbSet<Class> Classes => Set<Class>();
    public DbSet<Subclass> Subclasses => Set<Subclass>();
    public DbSet<ClassFeature> ClassFeatures => Set<ClassFeature>();
    public DbSet<SubclassFeature> SubclassFeatures => Set<SubclassFeature>();
    public DbSet<MulticlassPrerequisite> MulticlassPrerequisites => Set<MulticlassPrerequisite>();
    public DbSet<ClassSpellSlotTemplate> ClassSpellSlotTemplates => Set<ClassSpellSlotTemplate>();
    public DbSet<ClassStartSkillProficiency> ClassStartSkillProficiencies => Set<ClassStartSkillProficiency>();
    public DbSet<ClassStartingItemSet> ClassStartingItemSets => Set<ClassStartingItemSet>();
    public DbSet<ClassStartingItemChoicePair> ClassStartingItemChoicePairs => Set<ClassStartingItemChoicePair>();
    public DbSet<Race> Races => Set<Race>();
    public DbSet<RaceFeature> RaceFeatures => Set<RaceFeature>();

    public DbSet<CharacterClass> CharacterClasses => Set<CharacterClass>();
    public DbSet<CharacterProficiencies> CharacterProficiencies => Set<CharacterProficiencies>();
    public DbSet<SkillProficiency> SkillProficiencies => Set<SkillProficiency>();

    public DbSet<AdditionalSavingThrowProficiency> AdditionalSavingThrowProficiencies =>
        Set<AdditionalSavingThrowProficiency>();

    public DbSet<ClassSavingThrowProficiency> ClassSavingThrowProficiencies => Set<ClassSavingThrowProficiency>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<Instrument> Instruments => Set<Instrument>();
    public DbSet<WeaponProficiency> WeaponProficiencies => Set<WeaponProficiency>();
    public DbSet<ArmorProficiency> ArmorProficiencies => Set<ArmorProficiency>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PocketGrailDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}