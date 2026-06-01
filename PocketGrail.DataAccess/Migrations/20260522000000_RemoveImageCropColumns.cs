using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocketGrail.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveImageCropColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
