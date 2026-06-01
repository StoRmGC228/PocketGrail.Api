using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PocketGrail.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddClassRacePrerequisiteData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterClasses_Class_ClassId",
                table: "CharacterClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterClasses_Subclass_CharacterSubclassId",
                table: "CharacterClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterFeature_Characters_CharactersId",
                table: "CharacterFeature");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSavingThrowProficiencies_Class_ClassId",
                table: "ClassSavingThrowProficiencies");

            migrationBuilder.DropForeignKey(
                name: "FK_Features_Class_ClassId",
                table: "Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Subclass_Class_ClassId",
                table: "Subclass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subclass",
                table: "Subclass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Class",
                table: "Class");

            migrationBuilder.DropColumn(
                name: "DescriptionText",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "FeatureType",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "SourceClass",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "SourceRace",
                table: "Features");

            migrationBuilder.RenameTable(
                name: "Subclass",
                newName: "Subclasses");

            migrationBuilder.RenameTable(
                name: "Class",
                newName: "Classes");

            migrationBuilder.DropColumn(
                name: "FeatureLevel",
                table: "Features");

            migrationBuilder.AddColumn<int>(
                name: "RaceId",
                table: "Features",
                type: "integer",
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "CharactersId",
                table: "CharacterFeature",
                newName: "CharacterId");

            migrationBuilder.RenameIndex(
                name: "IX_Subclass_ClassId",
                table: "Subclasses",
                newName: "IX_Subclasses_ClassId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Features",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShortDescription",
                table: "Subclasses",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Subclasses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SpellAbility",
                table: "Classes",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Classes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "HitDice",
                table: "Classes",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "SkillChoiceCount",
                table: "Classes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subclasses",
                table: "Subclasses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classes",
                table: "Classes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ClassFeatureArmorGrants",
                columns: table => new
                {
                    ArmorGrantsId = table.Column<int>(type: "integer", nullable: false),
                    ClassFeatureId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassFeatureArmorGrants", x => new { x.ArmorGrantsId, x.ClassFeatureId });
                    table.ForeignKey(
                        name: "FK_ClassFeatureArmorGrants_ArmorProficiencies_ArmorGrantsId",
                        column: x => x.ArmorGrantsId,
                        principalTable: "ArmorProficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassFeatureArmorGrants_Features_ClassFeatureId",
                        column: x => x.ClassFeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassFeatureInstrumentGrants",
                columns: table => new
                {
                    ClassFeatureId = table.Column<int>(type: "integer", nullable: false),
                    InstrumentGrantsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassFeatureInstrumentGrants", x => new { x.ClassFeatureId, x.InstrumentGrantsId });
                    table.ForeignKey(
                        name: "FK_ClassFeatureInstrumentGrants_Features_ClassFeatureId",
                        column: x => x.ClassFeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassFeatureInstrumentGrants_Instruments_InstrumentGrantsId",
                        column: x => x.InstrumentGrantsId,
                        principalTable: "Instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassFeatureLanguageGrants",
                columns: table => new
                {
                    ClassFeatureId = table.Column<int>(type: "integer", nullable: false),
                    LanguageGrantsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassFeatureLanguageGrants", x => new { x.ClassFeatureId, x.LanguageGrantsId });
                    table.ForeignKey(
                        name: "FK_ClassFeatureLanguageGrants_Features_ClassFeatureId",
                        column: x => x.ClassFeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassFeatureLanguageGrants_Languages_LanguageGrantsId",
                        column: x => x.LanguageGrantsId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassFeatureWeaponGrants",
                columns: table => new
                {
                    ClassFeatureId = table.Column<int>(type: "integer", nullable: false),
                    WeaponGrantsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassFeatureWeaponGrants", x => new { x.ClassFeatureId, x.WeaponGrantsId });
                    table.ForeignKey(
                        name: "FK_ClassFeatureWeaponGrants_Features_ClassFeatureId",
                        column: x => x.ClassFeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassFeatureWeaponGrants_WeaponProficiencies_WeaponGrantsId",
                        column: x => x.WeaponGrantsId,
                        principalTable: "WeaponProficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassSpellSlotTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    ClassLevel = table.Column<int>(type: "integer", nullable: false),
                    SpellSlotLevel = table.Column<int>(type: "integer", nullable: false),
                    TotalSlots = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSpellSlotTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSpellSlotTemplates_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MulticlassPrerequisites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    RequiredAbility = table.Column<int>(type: "integer", nullable: false),
                    MinimumScore = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MulticlassPrerequisites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MulticlassPrerequisites_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BaseSpeed = table.Column<int>(type: "integer", nullable: false),
                    StrBonus = table.Column<int>(type: "integer", nullable: false),
                    DexBonus = table.Column<int>(type: "integer", nullable: false),
                    ConBonus = table.Column<int>(type: "integer", nullable: false),
                    IntBonus = table.Column<int>(type: "integer", nullable: false),
                    WisBonus = table.Column<int>(type: "integer", nullable: false),
                    ChaBonus = table.Column<int>(type: "integer", nullable: false),
                    FlexibleBonusPoints = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RaceArmorGrants",
                columns: table => new
                {
                    ArmorGrantsId = table.Column<int>(type: "integer", nullable: false),
                    RaceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceArmorGrants", x => new { x.ArmorGrantsId, x.RaceId });
                    table.ForeignKey(
                        name: "FK_RaceArmorGrants_ArmorProficiencies_ArmorGrantsId",
                        column: x => x.ArmorGrantsId,
                        principalTable: "ArmorProficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceArmorGrants_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaceInstrumentGrants",
                columns: table => new
                {
                    InstrumentGrantsId = table.Column<int>(type: "integer", nullable: false),
                    RaceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceInstrumentGrants", x => new { x.InstrumentGrantsId, x.RaceId });
                    table.ForeignKey(
                        name: "FK_RaceInstrumentGrants_Instruments_InstrumentGrantsId",
                        column: x => x.InstrumentGrantsId,
                        principalTable: "Instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceInstrumentGrants_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaceLanguageGrants",
                columns: table => new
                {
                    LanguageGrantsId = table.Column<int>(type: "integer", nullable: false),
                    RaceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceLanguageGrants", x => new { x.LanguageGrantsId, x.RaceId });
                    table.ForeignKey(
                        name: "FK_RaceLanguageGrants_Languages_LanguageGrantsId",
                        column: x => x.LanguageGrantsId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceLanguageGrants_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RaceWeaponGrants",
                columns: table => new
                {
                    RaceId = table.Column<int>(type: "integer", nullable: false),
                    WeaponGrantsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceWeaponGrants", x => new { x.RaceId, x.WeaponGrantsId });
                    table.ForeignKey(
                        name: "FK_RaceWeaponGrants_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceWeaponGrants_WeaponProficiencies_WeaponGrantsId",
                        column: x => x.WeaponGrantsId,
                        principalTable: "WeaponProficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Features_RaceId",
                table: "Features",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_Name",
                table: "Classes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassFeatureArmorGrants_ClassFeatureId",
                table: "ClassFeatureArmorGrants",
                column: "ClassFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassFeatureInstrumentGrants_InstrumentGrantsId",
                table: "ClassFeatureInstrumentGrants",
                column: "InstrumentGrantsId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassFeatureLanguageGrants_LanguageGrantsId",
                table: "ClassFeatureLanguageGrants",
                column: "LanguageGrantsId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassFeatureWeaponGrants_WeaponGrantsId",
                table: "ClassFeatureWeaponGrants",
                column: "WeaponGrantsId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSpellSlotTemplates_ClassId_ClassLevel_SpellSlotLevel",
                table: "ClassSpellSlotTemplates",
                columns: new[] { "ClassId", "ClassLevel", "SpellSlotLevel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MulticlassPrerequisites_ClassId_RequiredAbility",
                table: "MulticlassPrerequisites",
                columns: new[] { "ClassId", "RequiredAbility" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RaceArmorGrants_RaceId",
                table: "RaceArmorGrants",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_RaceInstrumentGrants_RaceId",
                table: "RaceInstrumentGrants",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_RaceLanguageGrants_RaceId",
                table: "RaceLanguageGrants",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Races_Name",
                table: "Races",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RaceWeaponGrants_WeaponGrantsId",
                table: "RaceWeaponGrants",
                column: "WeaponGrantsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterClasses_Classes_ClassId",
                table: "CharacterClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterClasses_Subclasses_CharacterSubclassId",
                table: "CharacterClasses",
                column: "CharacterSubclassId",
                principalTable: "Subclasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterFeature_Characters_CharacterId",
                table: "CharacterFeature",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSavingThrowProficiencies_Classes_ClassId",
                table: "ClassSavingThrowProficiencies",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Classes_ClassId",
                table: "Features",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Races_RaceId",
                table: "Features",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subclasses_Classes_ClassId",
                table: "Subclasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterClasses_Classes_ClassId",
                table: "CharacterClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterClasses_Subclasses_CharacterSubclassId",
                table: "CharacterClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterFeature_Characters_CharacterId",
                table: "CharacterFeature");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassSavingThrowProficiencies_Classes_ClassId",
                table: "ClassSavingThrowProficiencies");

            migrationBuilder.DropForeignKey(
                name: "FK_Features_Classes_ClassId",
                table: "Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Features_Races_RaceId",
                table: "Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Subclasses_Classes_ClassId",
                table: "Subclasses");

            migrationBuilder.DropTable(
                name: "ClassFeatureArmorGrants");

            migrationBuilder.DropTable(
                name: "ClassFeatureInstrumentGrants");

            migrationBuilder.DropTable(
                name: "ClassFeatureLanguageGrants");

            migrationBuilder.DropTable(
                name: "ClassFeatureWeaponGrants");

            migrationBuilder.DropTable(
                name: "ClassSpellSlotTemplates");

            migrationBuilder.DropTable(
                name: "MulticlassPrerequisites");

            migrationBuilder.DropTable(
                name: "RaceArmorGrants");

            migrationBuilder.DropTable(
                name: "RaceInstrumentGrants");

            migrationBuilder.DropTable(
                name: "RaceLanguageGrants");

            migrationBuilder.DropTable(
                name: "RaceWeaponGrants");

            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropIndex(
                name: "IX_Features_RaceId",
                table: "Features");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subclasses",
                table: "Subclasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classes",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_Name",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "SkillChoiceCount",
                table: "Classes");

            migrationBuilder.RenameTable(
                name: "Subclasses",
                newName: "Subclass");

            migrationBuilder.RenameTable(
                name: "Classes",
                newName: "Class");

            migrationBuilder.DropColumn(
                name: "RaceId",
                table: "Features");

            migrationBuilder.AddColumn<int>(
                name: "FeatureLevel",
                table: "Features",
                type: "integer",
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "CharacterId",
                table: "CharacterFeature",
                newName: "CharactersId");

            migrationBuilder.RenameIndex(
                name: "IX_Subclasses_ClassId",
                table: "Subclass",
                newName: "IX_Subclass_ClassId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Features",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionText",
                table: "Features",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeatureType",
                table: "Features",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceClass",
                table: "Features",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceRace",
                table: "Features",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShortDescription",
                table: "Subclass",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Subclass",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "SpellAbility",
                table: "Class",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Class",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "HitDice",
                table: "Class",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subclass",
                table: "Subclass",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Class",
                table: "Class",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterClasses_Class_ClassId",
                table: "CharacterClasses",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterClasses_Subclass_CharacterSubclassId",
                table: "CharacterClasses",
                column: "CharacterSubclassId",
                principalTable: "Subclass",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterFeature_Characters_CharactersId",
                table: "CharacterFeature",
                column: "CharactersId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSavingThrowProficiencies_Class_ClassId",
                table: "ClassSavingThrowProficiencies",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Class_ClassId",
                table: "Features",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subclass_Class_ClassId",
                table: "Subclass",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
