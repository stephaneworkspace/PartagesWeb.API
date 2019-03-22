#pragma warning disable 1591
using Microsoft.EntityFrameworkCore.Migrations;

namespace PartagesWeb.API.Migrations
{
    public partial class CorrectionForumPosteEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPostes_ForumCategories_ForumTopicId",
                table: "ForumPostes");

            migrationBuilder.RenameColumn(
                name: "ForumTopicId",
                table: "ForumPostes",
                newName: "ForumCategorieId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumPostes_ForumTopicId",
                table: "ForumPostes",
                newName: "IX_ForumPostes_ForumCategorieId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPostes_ForumCategories_ForumCategorieId",
                table: "ForumPostes",
                column: "ForumCategorieId",
                principalTable: "ForumCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumPostes_ForumCategories_ForumCategorieId",
                table: "ForumPostes");

            migrationBuilder.RenameColumn(
                name: "ForumCategorieId",
                table: "ForumPostes",
                newName: "ForumTopicId");

            migrationBuilder.RenameIndex(
                name: "IX_ForumPostes_ForumCategorieId",
                table: "ForumPostes",
                newName: "IX_ForumPostes_ForumTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumPostes_ForumCategories_ForumTopicId",
                table: "ForumPostes",
                column: "ForumTopicId",
                principalTable: "ForumCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
