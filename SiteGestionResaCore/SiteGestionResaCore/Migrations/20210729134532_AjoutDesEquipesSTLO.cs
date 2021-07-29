using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutDesEquipesSTLO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ld_equipes_stlo",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_equipe = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ld_equipes_stlo", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7c720f97-f71e-4a18-83a2-1196672d454d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "72f4530a-3c05-486e-b89c-30175d82d5fa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "da448997-5164-4504-ac6a-ead79c0fcb11");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "12aa6d7a-6ad4-4a3a-87a6-9f3de0642f8d");

            migrationBuilder.InsertData(
                table: "ld_equipes_stlo",
                columns: new[] { "id", "nom_equipe" },
                values: new object[,]
                {
                    { 1, "Microbio" },
                    { 2, "BN" },
                    { 3, "PSM" },
                    { 4, "ISF" },
                    { 5, "SMCF" },
                    { 6, "PFL" },
                    { 7, "CIRM-BIA" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ld_equipes_stlo");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "6dbb37e8-ce2d-461f-9ebc-af5b13facd9c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1d96a815-f5e2-4eae-b37e-69fe9ac45ab9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "ab4744ac-ac20-4ab2-9457-4e1161b22809");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "a60d093f-3b5b-4ad2-93c5-b331e39a4d4a");
        }
    }
}
