using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomBackend.Migrations
{
    public partial class changeStreet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StreetName",
                table: "apartment");

            migrationBuilder.AddColumn<float>(
                name: "DistrictValue",
                table: "apartment",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictValue",
                table: "apartment");

            migrationBuilder.AddColumn<string>(
                name: "StreetName",
                table: "apartment",
                type: "text",
                nullable: true);
        }
    }
}
