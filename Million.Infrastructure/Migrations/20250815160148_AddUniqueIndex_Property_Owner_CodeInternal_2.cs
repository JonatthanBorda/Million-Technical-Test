using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Million.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndex_Property_Owner_CodeInternal_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Property_CodeInternal",
                table: "Property");

            migrationBuilder.CreateIndex(
                name: "IX_Property_CodeInternal",
                table: "Property",
                column: "CodeInternal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Property_CodeInternal",
                table: "Property");

            migrationBuilder.CreateIndex(
                name: "IX_Property_CodeInternal",
                table: "Property",
                column: "CodeInternal",
                unique: true);
        }
    }
}
