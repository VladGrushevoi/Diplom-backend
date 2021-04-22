using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomBackend.Migrations
{
    public partial class changeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_importantPlaces_typePalces_TypePlaceId",
                table: "importantPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_typePalces_districts_DistrictId",
                table: "typePalces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_typePalces",
                table: "typePalces");

            migrationBuilder.RenameTable(
                name: "typePalces",
                newName: "typePlaces");

            migrationBuilder.RenameIndex(
                name: "IX_typePalces_DistrictId",
                table: "typePlaces",
                newName: "IX_typePlaces_DistrictId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_typePlaces",
                table: "typePlaces",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_importantPlaces_typePlaces_TypePlaceId",
                table: "importantPlaces",
                column: "TypePlaceId",
                principalTable: "typePlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_typePlaces_districts_DistrictId",
                table: "typePlaces",
                column: "DistrictId",
                principalTable: "districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_importantPlaces_typePlaces_TypePlaceId",
                table: "importantPlaces");

            migrationBuilder.DropForeignKey(
                name: "FK_typePlaces_districts_DistrictId",
                table: "typePlaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_typePlaces",
                table: "typePlaces");

            migrationBuilder.RenameTable(
                name: "typePlaces",
                newName: "typePalces");

            migrationBuilder.RenameIndex(
                name: "IX_typePlaces_DistrictId",
                table: "typePalces",
                newName: "IX_typePalces_DistrictId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_typePalces",
                table: "typePalces",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_importantPlaces_typePalces_TypePlaceId",
                table: "importantPlaces",
                column: "TypePlaceId",
                principalTable: "typePalces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_typePalces_districts_DistrictId",
                table: "typePalces",
                column: "DistrictId",
                principalTable: "districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
