using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PocketGrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacterClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    ClassName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ClassLevel = table.Column<int>(type: "integer", nullable: false),
                    HitDice = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Subclass = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TotalHitDice = table.Column<int>(type: "integer", nullable: false),
                    UsedHitDice = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterClasses_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Backfill existing characters: create one CharacterClass row per character
            migrationBuilder.Sql(@"
INSERT INTO ""CharacterClasses""
    (""CharacterId"", ""ClassName"", ""ClassLevel"", ""HitDice"", ""Subclass"",
     ""TotalHitDice"", ""UsedHitDice"", ""CreatedAt"", ""UpdatedAt"")
SELECT
    ""Id"",
    ""Class"",
    ""Level"",
    CASE ""Class""
        WHEN 'Barbarian' THEN 'd12'
        WHEN 'Fighter'   THEN 'd10'
        WHEN 'Paladin'   THEN 'd10'
        WHEN 'Ranger'    THEN 'd10'
        WHEN 'Sorcerer'  THEN 'd6'
        WHEN 'Wizard'    THEN 'd6'
        ELSE 'd8'
    END,
    ""Subclass"",
    ""Level"",
    0,
    NOW(),
    NOW()
FROM ""Characters""
WHERE ""Class"" IS NOT NULL AND ""Class"" != '';
");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClasses_CharacterId_ClassName",
                table: "CharacterClasses",
                columns: new[] { "CharacterId", "ClassName" },
                unique: true);

            migrationBuilder.DropColumn(
                name: "Class",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Subclass",
                table: "Characters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterClasses");

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "Characters",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subclass",
                table: "Characters",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
