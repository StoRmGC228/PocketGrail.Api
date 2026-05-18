using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PocketGrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SubclassFeatureSeparation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<int>>(
                name: "FlexBonusSlots",
                table: "Races",
                type: "integer[]",
                nullable: false,
                defaultValue: new List<int>());

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Features",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(13)",
                oldMaxLength: 13);

            migrationBuilder.AddColumn<int>(
                name: "ClassFeature_GainingLevel",
                table: "Features",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubclassId",
                table: "Features",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClassStartingItemSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStartingItemSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassStartingItemSets_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassStartSkillProficiencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    Skill = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStartSkillProficiencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassStartSkillProficiencies_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubclassFeatureArmorGrants",
                columns: table => new
                {
                    ArmorGrantsId = table.Column<int>(type: "integer", nullable: false),
                    SubclassFeatureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubclassFeatureArmorGrants", x => new { x.ArmorGrantsId, x.SubclassFeatureId });
                    table.ForeignKey(
                        name: "FK_SubclassFeatureArmorGrants_ArmorProficiencies_ArmorGrantsId",
                        column: x => x.ArmorGrantsId,
                        principalTable: "ArmorProficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubclassFeatureArmorGrants_Features_SubclassFeatureId",
                        column: x => x.SubclassFeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubclassFeatureInstrumentGrants",
                columns: table => new
                {
                    InstrumentGrantsId = table.Column<int>(type: "integer", nullable: false),
                    SubclassFeatureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubclassFeatureInstrumentGrants", x => new { x.InstrumentGrantsId, x.SubclassFeatureId });
                    table.ForeignKey(
                        name: "FK_SubclassFeatureInstrumentGrants_Features_SubclassFeatureId",
                        column: x => x.SubclassFeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubclassFeatureInstrumentGrants_Instruments_InstrumentGrant~",
                        column: x => x.InstrumentGrantsId,
                        principalTable: "Instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubclassFeatureLanguageGrants",
                columns: table => new
                {
                    LanguageGrantsId = table.Column<int>(type: "integer", nullable: false),
                    SubclassFeatureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubclassFeatureLanguageGrants", x => new { x.LanguageGrantsId, x.SubclassFeatureId });
                    table.ForeignKey(
                        name: "FK_SubclassFeatureLanguageGrants_Features_SubclassFeatureId",
                        column: x => x.SubclassFeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubclassFeatureLanguageGrants_Languages_LanguageGrantsId",
                        column: x => x.LanguageGrantsId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubclassFeatureWeaponGrants",
                columns: table => new
                {
                    SubclassFeatureId = table.Column<int>(type: "integer", nullable: false),
                    WeaponGrantsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubclassFeatureWeaponGrants", x => new { x.SubclassFeatureId, x.WeaponGrantsId });
                    table.ForeignKey(
                        name: "FK_SubclassFeatureWeaponGrants_Features_SubclassFeatureId",
                        column: x => x.SubclassFeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubclassFeatureWeaponGrants_WeaponProficiencies_WeaponGrant~",
                        column: x => x.WeaponGrantsId,
                        principalTable: "WeaponProficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassStartingItemChoicePairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassStartingItemSetId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStartingItemChoicePairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassStartingItemChoicePairs_ClassStartingItemSets_ClassSta~",
                        column: x => x.ClassStartingItemSetId,
                        principalTable: "ClassStartingItemSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassStartingItemChoicePairOptionA",
                columns: table => new
                {
                    ClassStartingItemChoicePairId = table.Column<int>(type: "integer", nullable: false),
                    OptionAId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStartingItemChoicePairOptionA", x => new { x.ClassStartingItemChoicePairId, x.OptionAId });
                    table.ForeignKey(
                        name: "FK_ClassStartingItemChoicePairOptionA_ClassStartingItemChoiceP~",
                        column: x => x.ClassStartingItemChoicePairId,
                        principalTable: "ClassStartingItemChoicePairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassStartingItemChoicePairOptionA_Items_OptionAId",
                        column: x => x.OptionAId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassStartingItemChoicePairOptionB",
                columns: table => new
                {
                    ClassStartingItemChoicePair1Id = table.Column<int>(type: "integer", nullable: false),
                    OptionBId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStartingItemChoicePairOptionB", x => new { x.ClassStartingItemChoicePair1Id, x.OptionBId });
                    table.ForeignKey(
                        name: "FK_ClassStartingItemChoicePairOptionB_ClassStartingItemChoiceP~",
                        column: x => x.ClassStartingItemChoicePair1Id,
                        principalTable: "ClassStartingItemChoicePairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassStartingItemChoicePairOptionB_Items_OptionBId",
                        column: x => x.OptionBId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Features_SubclassId",
                table: "Features",
                column: "SubclassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassStartingItemChoicePairOptionA_OptionAId",
                table: "ClassStartingItemChoicePairOptionA",
                column: "OptionAId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassStartingItemChoicePairOptionB_OptionBId",
                table: "ClassStartingItemChoicePairOptionB",
                column: "OptionBId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassStartingItemChoicePairs_ClassStartingItemSetId",
                table: "ClassStartingItemChoicePairs",
                column: "ClassStartingItemSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassStartingItemSets_ClassId",
                table: "ClassStartingItemSets",
                column: "ClassId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassStartSkillProficiencies_ClassId",
                table: "ClassStartSkillProficiencies",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SubclassFeatureArmorGrants_SubclassFeatureId",
                table: "SubclassFeatureArmorGrants",
                column: "SubclassFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_SubclassFeatureInstrumentGrants_SubclassFeatureId",
                table: "SubclassFeatureInstrumentGrants",
                column: "SubclassFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_SubclassFeatureLanguageGrants_SubclassFeatureId",
                table: "SubclassFeatureLanguageGrants",
                column: "SubclassFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_SubclassFeatureWeaponGrants_WeaponGrantsId",
                table: "SubclassFeatureWeaponGrants",
                column: "WeaponGrantsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Subclasses_SubclassId",
                table: "Features",
                column: "SubclassId",
                principalTable: "Subclasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Subclasses_SubclassId",
                table: "Features");

            migrationBuilder.DropTable(
                name: "ClassStartingItemChoicePairOptionA");

            migrationBuilder.DropTable(
                name: "ClassStartingItemChoicePairOptionB");

            migrationBuilder.DropTable(
                name: "ClassStartSkillProficiencies");

            migrationBuilder.DropTable(
                name: "SubclassFeatureArmorGrants");

            migrationBuilder.DropTable(
                name: "SubclassFeatureInstrumentGrants");

            migrationBuilder.DropTable(
                name: "SubclassFeatureLanguageGrants");

            migrationBuilder.DropTable(
                name: "SubclassFeatureWeaponGrants");

            migrationBuilder.DropTable(
                name: "ClassStartingItemChoicePairs");

            migrationBuilder.DropTable(
                name: "ClassStartingItemSets");

            migrationBuilder.DropIndex(
                name: "IX_Features_SubclassId",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "FlexBonusSlots",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "ClassFeature_GainingLevel",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "SubclassId",
                table: "Features");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Features",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(21)",
                oldMaxLength: 21);
        }
    }
}
