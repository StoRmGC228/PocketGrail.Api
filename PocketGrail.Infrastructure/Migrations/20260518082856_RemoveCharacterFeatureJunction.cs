using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocketGrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCharacterFeatureJunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterFeatures");

            migrationBuilder.CreateTable(
                name: "CharacterFeature",
                columns: table => new
                {
                    CharactersId = table.Column<int>(type: "integer", nullable: false),
                    FeaturesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterFeature", x => new { x.CharactersId, x.FeaturesId });
                    table.ForeignKey(
                        name: "FK_CharacterFeature_Characters_CharactersId",
                        column: x => x.CharactersId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterFeature_Features_FeaturesId",
                        column: x => x.FeaturesId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterFeature_FeaturesId",
                table: "CharacterFeature",
                column: "FeaturesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterFeature");

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

            migrationBuilder.CreateIndex(
                name: "IX_CharacterFeatures_FeatureId",
                table: "CharacterFeatures",
                column: "FeatureId");
        }
    }
}
