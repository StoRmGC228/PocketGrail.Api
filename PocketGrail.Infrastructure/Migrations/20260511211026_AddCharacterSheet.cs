using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PocketGrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterSheet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alignment",
                table: "Characters",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Appearance",
                table: "Characters",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArmorClass",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundStory",
                table: "Characters",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChaScore",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConScore",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CpCoins",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeathFailures",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeathSuccesses",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DexScore",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EpCoins",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Exhaustion",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GpCoins",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasInspiration",
                table: "Characters",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "ImageCropHeight",
                table: "Characters",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ImageCropWidth",
                table: "Characters",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ImageCropX",
                table: "Characters",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ImageCropY",
                table: "Characters",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntScore",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Characters",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PpCoins",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpCoins",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Speed",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SpellAbility",
                table: "Characters",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StrScore",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Subclass",
                table: "Characters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TempHp",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WisScore",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "XpPoints",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Feats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Requirement = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    FeatureType = table.Column<string>(type: "text", nullable: false),
                    FeatureLevel = table.Column<int>(type: "integer", nullable: true),
                    SourceClass = table.Column<string>(type: "text", nullable: true),
                    SourceRace = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Rarity = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Cost = table.Column<string>(type: "text", nullable: true),
                    IsWeapon = table.Column<bool>(type: "boolean", nullable: false),
                    IsMagical = table.Column<bool>(type: "boolean", nullable: false),
                    AtkMod = table.Column<string>(type: "text", nullable: true),
                    Damage = table.Column<string>(type: "text", nullable: true),
                    DamageType = table.Column<string>(type: "text", nullable: true),
                    WeaponProperties = table.Column<string>(type: "text", nullable: true),
                    ChargesInfo = table.Column<string>(type: "text", nullable: true),
                    RechargeType = table.Column<string>(type: "text", nullable: true),
                    Tags = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proficiencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProficiencyType = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proficiencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spells",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    School = table.Column<string>(type: "text", nullable: true),
                    Range = table.Column<string>(type: "text", nullable: true),
                    CastingTime = table.Column<string>(type: "text", nullable: true),
                    Concentration = table.Column<bool>(type: "boolean", nullable: false),
                    IsRitual = table.Column<bool>(type: "boolean", nullable: false),
                    Components = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spells", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpellSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    SlotLevel = table.Column<int>(type: "integer", nullable: false),
                    TotalSlots = table.Column<int>(type: "integer", nullable: false),
                    RemainingSlots = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpellSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpellSlots_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterFeats",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    FeatId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterFeats", x => new { x.CharacterId, x.FeatId });
                    table.ForeignKey(
                        name: "FK_CharacterFeats_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterFeats_Feats_FeatId",
                        column: x => x.FeatId,
                        principalTable: "Feats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterFeatures",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    FeatureId = table.Column<int>(type: "integer", nullable: false),
                    IsAutoAdded = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterFeatures", x => new { x.CharacterId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_CharacterFeatures_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterItems",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    IsEquipped = table.Column<bool>(type: "boolean", nullable: false),
                    IsAttuned = table.Column<bool>(type: "boolean", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterItems", x => new { x.CharacterId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_CharacterItems_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterProficiencies",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    ProficiencyId = table.Column<int>(type: "integer", nullable: false),
                    HasExpertise = table.Column<bool>(type: "boolean", nullable: false),
                    AbilityKey = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterProficiencies", x => new { x.CharacterId, x.ProficiencyId });
                    table.ForeignKey(
                        name: "FK_CharacterProficiencies_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterProficiencies_Proficiencies_ProficiencyId",
                        column: x => x.ProficiencyId,
                        principalTable: "Proficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterSpells",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    SpellId = table.Column<int>(type: "integer", nullable: false),
                    Prepared = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSpells", x => new { x.CharacterId, x.SpellId });
                    table.ForeignKey(
                        name: "FK_CharacterSpells_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterSpells_Spells_SpellId",
                        column: x => x.SpellId,
                        principalTable: "Spells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterFeats_FeatId",
                table: "CharacterFeats",
                column: "FeatId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterFeatures_FeatureId",
                table: "CharacterFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterItems_ItemId",
                table: "CharacterItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterProficiencies_ProficiencyId",
                table: "CharacterProficiencies",
                column: "ProficiencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSpells_SpellId",
                table: "CharacterSpells",
                column: "SpellId");

            migrationBuilder.CreateIndex(
                name: "IX_SpellSlots_CharacterId",
                table: "SpellSlots",
                column: "CharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterFeats");

            migrationBuilder.DropTable(
                name: "CharacterFeatures");

            migrationBuilder.DropTable(
                name: "CharacterItems");

            migrationBuilder.DropTable(
                name: "CharacterProficiencies");

            migrationBuilder.DropTable(
                name: "CharacterSpells");

            migrationBuilder.DropTable(
                name: "SpellSlots");

            migrationBuilder.DropTable(
                name: "Feats");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Proficiencies");

            migrationBuilder.DropTable(
                name: "Spells");

            migrationBuilder.DropColumn(
                name: "Alignment",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Appearance",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ArmorClass",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "BackgroundStory",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ChaScore",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ConScore",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CpCoins",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "DeathFailures",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "DeathSuccesses",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "DexScore",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "EpCoins",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Exhaustion",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "GpCoins",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "HasInspiration",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ImageCropHeight",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ImageCropWidth",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ImageCropX",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ImageCropY",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "IntScore",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "PpCoins",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SpCoins",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SpellAbility",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "StrScore",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Subclass",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "TempHp",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "WisScore",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "XpPoints",
                table: "Characters");
        }
    }
}
