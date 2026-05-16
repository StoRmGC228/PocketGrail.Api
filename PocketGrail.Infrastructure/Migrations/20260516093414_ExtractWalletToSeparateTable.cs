using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PocketGrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExtractWalletToSeparateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CpCoins",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "EpCoins",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "GpCoins",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "PpCoins",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "SpCoins",
                table: "Characters");

            migrationBuilder.CreateTable(
                name: "CharacterWallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    CpCoins = table.Column<int>(type: "integer", nullable: false),
                    SpCoins = table.Column<int>(type: "integer", nullable: false),
                    EpCoins = table.Column<int>(type: "integer", nullable: false),
                    GpCoins = table.Column<int>(type: "integer", nullable: false),
                    PpCoins = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterWallets_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterWallets_CharacterId",
                table: "CharacterWallets",
                column: "CharacterId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterWallets");

            migrationBuilder.AddColumn<int>(
                name: "CpCoins",
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
                name: "GpCoins",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
        }
    }
}
