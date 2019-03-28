#pragma warning disable 1591
using Microsoft.EntityFrameworkCore.Migrations;

namespace PartagesWeb.API.Migrations
{
    public partial class AjoutDUneCleVirtualForumSujetEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForumCategorieId",
                table: "ForumPostes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForumCategorieId",
                table: "ForumPostes");
        }
    }
}
