using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class MigrationTabEnquete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "enqueteId",
                table: "essai",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "enquete",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date_envoi_enquete = table.Column<DateTime>(unicode: false, nullable: false),
                    essaiId = table.Column<int>(nullable: false),
                    reponduEnquete = table.Column<bool>(nullable: true),
                    fichierReponse = table.Column<string>(nullable: true),
                    date_reponse = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enquete", x => x.id);
                    table.ForeignKey(
                        name: "FK_enquete_essai",
                        column: x => x.essaiId,
                        principalTable: "essai",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "644dbaad-602b-4e3d-ae41-9dfecc0f0101");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "04346adc-257e-4e07-a001-48123fd3e466");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "58b590d4-929f-4f3a-a0ae-42fa789980e0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "b1deddb2-d31f-42c9-bb3a-dd0dec18d467");

            migrationBuilder.CreateIndex(
                name: "IX_enquete_essaiId",
                table: "enquete",
                column: "essaiId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enquete");

            migrationBuilder.DropColumn(
                name: "enqueteId",
                table: "essai");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "3e37fefe-c76b-48ce-a0a3-0bf3ebcd23dd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "19ef18fa-29cb-4f21-99cb-43a6b2334eb9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "d7352a83-2b79-4e87-8d4a-bf84ee723b69");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "8461f932-a89a-4f73-85c6-aed542489c29");
        }
    }
}
