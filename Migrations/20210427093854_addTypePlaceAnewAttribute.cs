﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomBackend.Migrations
{
    public partial class addTypePlaceAnewAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "typePlaces",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "typePlaces");
        }
    }
}
