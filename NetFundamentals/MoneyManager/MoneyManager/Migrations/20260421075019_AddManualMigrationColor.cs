using Microsoft.EntityFrameworkCore.Migrations;
using MoneyManager.Constants;

#nullable disable

namespace MoneyManager.Migrations
{
    /// <inheritdoc />
    public partial class AddManualMigrationColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Color",
                table: "Categories",
                nullable: false,
                defaultValue: Colors.NileBlue);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "Color",
            table: "Categories");
        }
    }
}
