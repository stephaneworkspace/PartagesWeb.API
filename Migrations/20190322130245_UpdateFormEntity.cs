#pragma warning disable 1591
using Microsoft.EntityFrameworkCore.Migrations;

namespace PartagesWeb.API.Migrations
{
    public partial class UpdateFormEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForumSujetId",
                table: "ForumPostes",
                nullable: false,
                defaultValue: 0);

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
                name: "ForumSujetId",
                table: "ForumPostes");
        }
    }
}
