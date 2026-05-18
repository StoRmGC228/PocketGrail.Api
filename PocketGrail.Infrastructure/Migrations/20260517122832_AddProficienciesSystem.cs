using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PocketGrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProficienciesSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterProficiencies_Proficiencies_ProficiencyId",
                table: "CharacterProficiencies");

            migrationBuilder.DropTable(
                name: "Proficiencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterProficiencies",
                table: "CharacterProficiencies");

            migrationBuilder.DropIndex(
                name: "IX_CharacterProficiencies_ProficiencyId",
                table: "CharacterProficiencies");

            migrationBuilder.DropIndex(
                name: "IX_CharacterClasses_CharacterId_ClassName",
                table: "CharacterClasses");

            migrationBuilder.DropColumn(
                name: "ChaScore",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ConScore",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "DexScore",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "IntScore",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SpellAbility",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "AbilityKey",
                table: "CharacterProficiencies");

            migrationBuilder.DropColumn(
                name: "HasExpertise",
                table: "CharacterProficiencies");

            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "CharacterClasses");

            migrationBuilder.DropColumn(
                name: "HitDice",
                table: "CharacterClasses");

            migrationBuilder.DropColumn(
                name: "Subclass",
                table: "CharacterClasses");

            migrationBuilder.RenameColumn(
                name: "WisScore",
                table: "Characters",
                newName: "UsedHitDice");

            migrationBuilder.RenameColumn(
                name: "StrScore",
                table: "Characters",
                newName: "TotalHitDiceCount");

            migrationBuilder.RenameColumn(
                name: "ProficiencyId",
                table: "CharacterProficiencies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UsedHitDice",
                table: "CharacterClasses",
                newName: "TotalHitDiceCount");

            migrationBuilder.RenameColumn(
                name: "TotalHitDice",
                table: "CharacterClasses",
                newName: "ClassId");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Features",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionText",
                table: "Features",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Features",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GainingLevel",
                table: "Features",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CharacterProficiencies",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CharacterProficiencies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CharacterProficiencies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CharacterSubclassId",
                table: "CharacterClasses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterProficiencies",
                table: "CharacterProficiencies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AdditionalSavingThrowProficiencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterProficienciesId = table.Column<int>(type: "integer", nullable: false),
                    Ability = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalSavingThrowProficiencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalSavingThrowProficiencies_CharacterProficiencies_C~",
                        column: x => x.CharacterProficienciesId,
                        principalTable: "CharacterProficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArmorProficiencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArmorProficiencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    Strength = table.Column<int>(type: "integer", nullable: false),
                    Dexterity = table.Column<int>(type: "integer", nullable: false),
                    Constitution = table.Column<int>(type: "integer", nullable: false),
                    Intelligence = table.Column<int>(type: "integer", nullable: false),
                    Wisdom = table.Column<int>(type: "integer", nullable: false),
                    Charisma = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterStats_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ShortDesсription = table.Column<string>(type: "text", nullable: false),
                    SpellAbility = table.Column<string>(type: "text", nullable: false),
                    TotalHitDice = table.Column<int>(type: "integer", nullable: false),
                    UsedHitDice = table.Column<int>(type: "integer", nullable: false),
                    HitDice = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instruments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillProficiencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterProficienciesId = table.Column<int>(type: "integer", nullable: false),
                    Skill = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    HasExpertise = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillProficiencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillProficiencies_CharacterProficiencies_CharacterProficie~",
                        column: x => x.CharacterProficienciesId,
                        principalTable: "CharacterProficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeaponProficiencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponProficiencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterProficienciesArmorProficiencies",
                columns: table => new
                {
                    ArmorsId = table.Column<int>(type: "integer", nullable: false),
                    CharacterProficienciesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterProficienciesArmorProficiencies", x => new { x.ArmorsId, x.CharacterProficienciesId });
                    table.ForeignKey(
                        name: "FK_CharacterProficienciesArmorProficiencies_ArmorProficiencies~",
                        column: x => x.ArmorsId,
                        principalTable: "ArmorProficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterProficienciesArmorProficiencies_CharacterProficien~",
                        column: x => x.CharacterProficienciesId,
                        principalTable: "CharacterProficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassSavingThrowProficiencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    Ability = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSavingThrowProficiencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSavingThrowProficiencies_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subclass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subclass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subclass_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterProficienciesInstruments",
                columns: table => new
                {
                    CharacterProficienciesId = table.Column<int>(type: "integer", nullable: false),
                    InstrumentsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterProficienciesInstruments", x => new { x.CharacterProficienciesId, x.InstrumentsId });
                    table.ForeignKey(
                        name: "FK_CharacterProficienciesInstruments_CharacterProficiencies_Ch~",
                        column: x => x.CharacterProficienciesId,
                        principalTable: "CharacterProficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterProficienciesInstruments_Instruments_InstrumentsId",
                        column: x => x.InstrumentsId,
                        principalTable: "Instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterProficienciesLanguages",
                columns: table => new
                {
                    CharacterProficienciesId = table.Column<int>(type: "integer", nullable: false),
                    LanguagesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterProficienciesLanguages", x => new { x.CharacterProficienciesId, x.LanguagesId });
                    table.ForeignKey(
                        name: "FK_CharacterProficienciesLanguages_CharacterProficiencies_Char~",
                        column: x => x.CharacterProficienciesId,
                        principalTable: "CharacterProficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterProficienciesLanguages_Languages_LanguagesId",
                        column: x => x.LanguagesId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterProficienciesWeaponProficiencies",
                columns: table => new
                {
                    CharacterProficienciesId = table.Column<int>(type: "integer", nullable: false),
                    WeaponsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterProficienciesWeaponProficiencies", x => new { x.CharacterProficienciesId, x.WeaponsId });
                    table.ForeignKey(
                        name: "FK_CharacterProficienciesWeaponProficiencies_CharacterProficie~",
                        column: x => x.CharacterProficienciesId,
                        principalTable: "CharacterProficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterProficienciesWeaponProficiencies_WeaponProficienci~",
                        column: x => x.WeaponsId,
                        principalTable: "WeaponProficiencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Features_ClassId",
                table: "Features",
                column: "ClassId");

            // Old CharacterProficiencies was a many-to-many junction with multiple rows per CharacterId.
            // Clear stale rows before adding the 1:1 unique constraint.
            migrationBuilder.Sql("DELETE FROM \"CharacterProficiencies\";");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterProficiencies_CharacterId",
                table: "CharacterProficiencies",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClasses_CharacterId_ClassId",
                table: "CharacterClasses",
                columns: new[] { "CharacterId", "ClassId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClasses_CharacterSubclassId",
                table: "CharacterClasses",
                column: "CharacterSubclassId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClasses_ClassId",
                table: "CharacterClasses",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalSavingThrowProficiencies_CharacterProficienciesId~",
                table: "AdditionalSavingThrowProficiencies",
                columns: new[] { "CharacterProficienciesId", "Ability" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterProficienciesArmorProficiencies_CharacterProficien~",
                table: "CharacterProficienciesArmorProficiencies",
                column: "CharacterProficienciesId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterProficienciesInstruments_InstrumentsId",
                table: "CharacterProficienciesInstruments",
                column: "InstrumentsId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterProficienciesLanguages_LanguagesId",
                table: "CharacterProficienciesLanguages",
                column: "LanguagesId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterProficienciesWeaponProficiencies_WeaponsId",
                table: "CharacterProficienciesWeaponProficiencies",
                column: "WeaponsId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterStats_CharacterId",
                table: "CharacterStats",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSavingThrowProficiencies_ClassId_Ability",
                table: "ClassSavingThrowProficiencies",
                columns: new[] { "ClassId", "Ability" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillProficiencies_CharacterProficienciesId_Skill",
                table: "SkillProficiencies",
                columns: new[] { "CharacterProficienciesId", "Skill" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subclass_ClassId",
                table: "Subclass",
                column: "ClassId");

            // CharacterClasses rows have stale data in ClassId (formerly TotalHitDice — invalid FK values).
            // Clear the table before adding the FK; dev data only.
            migrationBuilder.Sql("DELETE FROM \"CharacterClasses\";");

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
                name: "FK_Features_Class_ClassId",
                table: "Features",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterClasses_Class_ClassId",
                table: "CharacterClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterClasses_Subclass_CharacterSubclassId",
                table: "CharacterClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_Features_Class_ClassId",
                table: "Features");

            migrationBuilder.DropTable(
                name: "AdditionalSavingThrowProficiencies");

            migrationBuilder.DropTable(
                name: "CharacterProficienciesArmorProficiencies");

            migrationBuilder.DropTable(
                name: "CharacterProficienciesInstruments");

            migrationBuilder.DropTable(
                name: "CharacterProficienciesLanguages");

            migrationBuilder.DropTable(
                name: "CharacterProficienciesWeaponProficiencies");

            migrationBuilder.DropTable(
                name: "CharacterStats");

            migrationBuilder.DropTable(
                name: "ClassSavingThrowProficiencies");

            migrationBuilder.DropTable(
                name: "SkillProficiencies");

            migrationBuilder.DropTable(
                name: "Subclass");

            migrationBuilder.DropTable(
                name: "ArmorProficiencies");

            migrationBuilder.DropTable(
                name: "Instruments");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "WeaponProficiencies");

            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropIndex(
                name: "IX_Features_ClassId",
                table: "Features");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterProficiencies",
                table: "CharacterProficiencies");

            migrationBuilder.DropIndex(
                name: "IX_CharacterProficiencies_CharacterId",
                table: "CharacterProficiencies");

            migrationBuilder.DropIndex(
                name: "IX_CharacterClasses_CharacterId_ClassId",
                table: "CharacterClasses");

            migrationBuilder.DropIndex(
                name: "IX_CharacterClasses_CharacterSubclassId",
                table: "CharacterClasses");

            migrationBuilder.DropIndex(
                name: "IX_CharacterClasses_ClassId",
                table: "CharacterClasses");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "DescriptionText",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "GainingLevel",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CharacterProficiencies");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CharacterProficiencies");

            migrationBuilder.DropColumn(
                name: "CharacterSubclassId",
                table: "CharacterClasses");

            migrationBuilder.RenameColumn(
                name: "UsedHitDice",
                table: "Characters",
                newName: "WisScore");

            migrationBuilder.RenameColumn(
                name: "TotalHitDiceCount",
                table: "Characters",
                newName: "StrScore");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CharacterProficiencies",
                newName: "ProficiencyId");

            migrationBuilder.RenameColumn(
                name: "TotalHitDiceCount",
                table: "CharacterClasses",
                newName: "UsedHitDice");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "CharacterClasses",
                newName: "TotalHitDice");

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
                name: "DexScore",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IntScore",
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

            migrationBuilder.AlterColumn<int>(
                name: "ProficiencyId",
                table: "CharacterProficiencies",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "AbilityKey",
                table: "CharacterProficiencies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasExpertise",
                table: "CharacterProficiencies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "CharacterClasses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HitDice",
                table: "CharacterClasses",
                type: "character varying(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subclass",
                table: "CharacterClasses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterProficiencies",
                table: "CharacterProficiencies",
                columns: new[] { "CharacterId", "ProficiencyId" });

            migrationBuilder.CreateTable(
                name: "Proficiencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProficiencyType = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proficiencies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterProficiencies_ProficiencyId",
                table: "CharacterProficiencies",
                column: "ProficiencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClasses_CharacterId_ClassName",
                table: "CharacterClasses",
                columns: new[] { "CharacterId", "ClassName" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterProficiencies_Proficiencies_ProficiencyId",
                table: "CharacterProficiencies",
                column: "ProficiencyId",
                principalTable: "Proficiencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
