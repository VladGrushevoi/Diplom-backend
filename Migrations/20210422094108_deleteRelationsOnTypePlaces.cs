using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomBackend.Migrations
{
    public partial class deleteRelationsOnTypePlaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_typePalces_districts_DistrictId",
                table: "typePalces");

            migrationBuilder.AlterColumn<int>(
                name: "DistrictId",
                table: "typePalces",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_typePalces_districts_DistrictId",
                table: "typePalces",
                column: "DistrictId",
                principalTable: "districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_typePalces_districts_DistrictId",
                table: "typePalces");

            migrationBuilder.AlterColumn<int>(
                name: "DistrictId",
                table: "typePalces",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_typePalces_districts_DistrictId",
                table: "typePalces",
                column: "DistrictId",
                principalTable: "districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
