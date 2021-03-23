using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomBackend.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KitchenSquare",
                table: "apartment",
                newName: "Price");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "apartment",
                newName: "KitchenSquare");
        }
    }
}
