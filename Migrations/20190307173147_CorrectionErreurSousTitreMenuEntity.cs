using Microsoft.EntityFrameworkCore.Migrations;

namespace PartagesWeb.API.Migrations
{
    public partial class CorrectionErreurSousTitreMenuEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Articles_SousTitreMenuId",
                table: "Articles",
                column: "SousTitreMenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_SousTitreMenus_SousTitreMenuId",
                table: "Articles",
                column: "SousTitreMenuId",
                principalTable: "SousTitreMenus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_SousTitreMenus_SousTitreMenuId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_SousTitreMenuId",
                table: "Articles");
        }
    }
}
