#pragma warning disable 1591
using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PartagesWeb.API.Migrations
{
    public partial class UpdateFormEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPostes_ForumCategories_ForumCategorieId",
                table: "ForumPostes");

            migrationBuilder.DropIndex(
                name: "IX_ForumPostes_ForumCategorieId",
                table: "ForumPostes");

            migrationBuilder.DropColumn(
                name: "ForumCategorieId",
                table: "ForumPostes");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "ForumPostes",
                newName: "ForumSujetId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ForumPostes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ForumPostes_ForumSujetId",
                table: "ForumPostes",
                column: "ForumSujetId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPostes_ForumSujets_ForumSujetId",
                table: "ForumPostes",
                column: "ForumSujetId",
                principalTable: "ForumSujets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPostes_ForumSujets_ForumSujetId",
                table: "ForumPostes");

            migrationBuilder.DropIndex(
                name: "IX_ForumPostes_ForumSujetId",
                table: "ForumPostes");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "ForumPostes");

            migrationBuilder.RenameColumn(
                name: "ForumSujetId",
                table: "ForumPostes",
                newName: "Position");

            migrationBuilder.AddColumn<int>(
                name: "ForumCategorieId",
                table: "ForumPostes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ForumPostes_ForumCategorieId",
                table: "ForumPostes",
                column: "ForumCategorieId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPostes_ForumCategories_ForumCategorieId",
                table: "ForumPostes",
                column: "ForumCategorieId",
                principalTable: "ForumCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
