#pragma warning disable 1591
using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PartagesWeb.API.Migrations
{
    public partial class UpdateFormEntity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "ForumPostes");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ForumPostes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "ForumPostes");

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "ForumPostes",
                nullable: false,
                defaultValue: 0);
        }
    }
}
