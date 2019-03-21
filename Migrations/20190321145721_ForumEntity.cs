#pragma warning disable 1591
using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PartagesWeb.API.Migrations
{
    public partial class ForumEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForumCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForumPostes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ForumTopicId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    Contenu = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumPostes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumPostes_ForumCategories_ForumTopicId",
                        column: x => x.ForumTopicId,
                        principalTable: "ForumCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumPostes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumSujets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ForumCategorieId = table.Column<int>(nullable: false),
                    Nom = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    View = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumSujets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumSujets_ForumCategories_ForumCategorieId",
                        column: x => x.ForumCategorieId,
                        principalTable: "ForumCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumPostes_ForumTopicId",
                table: "ForumPostes",
                column: "ForumTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumPostes_UserId",
                table: "ForumPostes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumSujets_ForumCategorieId",
                table: "ForumSujets",
                column: "ForumCategorieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumPostes");

            migrationBuilder.DropTable(
                name: "ForumSujets");

            migrationBuilder.DropTable(
                name: "ForumCategories");
        }
    }
}
