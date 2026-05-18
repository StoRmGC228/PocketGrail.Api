using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocketGrail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixClassFeatureGainingLevelColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // When SubclassFeature was added to the Feature hierarchy, EF Core remapped
            // ClassFeature.GainingLevel from "GainingLevel" to "ClassFeature_GainingLevel".
            // Rows seeded before that migration only have the value in the old column.
            migrationBuilder.Sql(
                """
                UPDATE "Features"
                SET    "ClassFeature_GainingLevel" = "GainingLevel"
                WHERE  "Discriminator"             = 'ClassFeature'
                  AND  "ClassFeature_GainingLevel" IS NULL
                  AND  "GainingLevel"              IS NOT NULL
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
