using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PartagesWeb.API.Migrations
{
    public partial class InitialAfterCrashEntity : Migration
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
                name: "Icones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FaValeur = table.Column<string>(nullable: true),
                    NomSelectBox = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Icones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(nullable: true),
                    Icone = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    SwHorsLigne = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "TitreMenus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SectionId = table.Column<int>(nullable: true),
                    Nom = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitreMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TitreMenus_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ForumPostes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ForumSujetId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Contenu = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumPostes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumPostes_ForumSujets_ForumSujetId",
                        column: x => x.ForumSujetId,
                        principalTable: "ForumSujets",
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
                name: "SousTitreMenus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TitreMenuId = table.Column<int>(nullable: true),
                    Nom = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SousTitreMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SousTitreMenus_TitreMenus_TitreMenuId",
                        column: x => x.TitreMenuId,
                        principalTable: "TitreMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SousTitreMenuId = table.Column<int>(nullable: true),
                    Nom = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Contenu = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_SousTitreMenus_SousTitreMenuId",
                        column: x => x.SousTitreMenuId,
                        principalTable: "SousTitreMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_SousTitreMenuId",
                table: "Articles",
                column: "SousTitreMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumPostes_ForumSujetId",
                table: "ForumPostes",
                column: "ForumSujetId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumPostes_UserId",
                table: "ForumPostes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumSujets_ForumCategorieId",
                table: "ForumSujets",
                column: "ForumCategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_SousTitreMenus_TitreMenuId",
                table: "SousTitreMenus",
                column: "TitreMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_TitreMenus_SectionId",
                table: "TitreMenus",
                column: "SectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "ForumPostes");

            migrationBuilder.DropTable(
                name: "Icones");

            migrationBuilder.DropTable(
                name: "SousTitreMenus");

            migrationBuilder.DropTable(
                name: "ForumSujets");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TitreMenus");

            migrationBuilder.DropTable(
                name: "ForumCategories");

            migrationBuilder.DropTable(
                name: "Sections");
        }
    }
}
