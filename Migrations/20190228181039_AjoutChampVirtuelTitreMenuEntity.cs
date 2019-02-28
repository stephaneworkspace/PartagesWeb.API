#pragma warning disable 1591
using Microsoft.EntityFrameworkCore.Migrations;

namespace PartagesWeb.API.Migrations
{
    public partial class AjoutChampVirtuelTitreMenuEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SousTitreMenus_TitreMenuId",
                table: "SousTitreMenus",
                column: "TitreMenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_SousTitreMenus_TitreMenus_TitreMenuId",
                table: "SousTitreMenus",
                column: "TitreMenuId",
                principalTable: "TitreMenus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SousTitreMenus_TitreMenus_TitreMenuId",
                table: "SousTitreMenus");

            migrationBuilder.DropIndex(
                name: "IX_SousTitreMenus_TitreMenuId",
                table: "SousTitreMenus");
        }
    }
}
