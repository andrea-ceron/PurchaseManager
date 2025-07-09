using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PurchaseManager.Api.Migrations
{
    /// <inheritdoc />
    public partial class patch_repository : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableQuantity",
                table: "RawMaterials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableQuantity",
                table: "RawMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
