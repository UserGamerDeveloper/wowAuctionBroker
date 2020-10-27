using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mvc.Client.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Realms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ConnectedRealmId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    TimeUpdate = table.Column<DateTime>(nullable: false),
                    FarmMode = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Realms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActiveRecipe",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdRecipe = table.Column<int>(nullable: false),
                    RealmId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveRecipe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiveRecipe_Realms_RealmId",
                        column: x => x.RealmId,
                        principalTable: "Realms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Character",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RealmId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Character", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Character_Realms_RealmId",
                        column: x => x.RealmId,
                        principalTable: "Realms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FactionModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FactionType = table.Column<int>(nullable: false),
                    MoneyMax = table.Column<long>(nullable: false),
                    RealmId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactionModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactionModel_Realms_RealmId",
                        column: x => x.RealmId,
                        principalTable: "Realms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveRecipe_RealmId",
                table: "ActiveRecipe",
                column: "RealmId");

            migrationBuilder.CreateIndex(
                name: "IX_Character_RealmId",
                table: "Character",
                column: "RealmId");

            migrationBuilder.CreateIndex(
                name: "IX_FactionModel_RealmId",
                table: "FactionModel",
                column: "RealmId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveRecipe");

            migrationBuilder.DropTable(
                name: "Character");

            migrationBuilder.DropTable(
                name: "FactionModel");

            migrationBuilder.DropTable(
                name: "Realms");
        }
    }
}
