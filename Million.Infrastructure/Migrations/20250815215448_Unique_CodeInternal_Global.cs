using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Million.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Unique_CodeInternal_Global : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Property_CodeInternal",
                table: "Property");

            migrationBuilder.DropIndex(
                name: "UX_Property_Owner_CodeInternal",
                table: "Property");

            migrationBuilder.CreateIndex(
                name: "UX_Property_CodeInternal",
                table: "Property",
                column: "CodeInternal",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Property_CodeInternal",
                table: "Property");

            migrationBuilder.CreateIndex(
                name: "IX_Property_CodeInternal",
                table: "Property",
                column: "CodeInternal");

            migrationBuilder.CreateIndex(
                name: "UX_Property_Owner_CodeInternal",
                table: "Property",
                columns: new[] { "OwnerId", "CodeInternal" },
                unique: true);
        }
    }
}
