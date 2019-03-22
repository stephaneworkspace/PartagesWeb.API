#pragma warning disable 1591
using Microsoft.EntityFrameworkCore.Migrations;

namespace PartagesWeb.API.Migrations
{
    public partial class UpdateFormEntity3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPostes_Users_UserId",
                table: "ForumPostes");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPostes_Users_UserId",
                table: "ForumPostes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPostes_Users_UserId",
                table: "ForumPostes");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPostes_Users_UserId",
                table: "ForumPostes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
