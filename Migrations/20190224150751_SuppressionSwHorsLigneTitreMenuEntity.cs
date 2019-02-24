#pragma warning disable 1591
using Microsoft.EntityFrameworkCore.Migrations;

namespace PartagesWeb.API.Migrations
{
    public partial class SuppressionSwHorsLigneTitreMenuEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SwHorsLigne",
                table: "TitreMenus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SwHorsLigne",
                table: "TitreMenus",
                nullable: false,
                defaultValue: false);
        }
    }
}
