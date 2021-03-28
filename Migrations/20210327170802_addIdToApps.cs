using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomBackend.Migrations
{
    public partial class addIdToApps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdFromApi",
                table: "apartment",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdFromApi",
                table: "apartment");
        }
    }
}
