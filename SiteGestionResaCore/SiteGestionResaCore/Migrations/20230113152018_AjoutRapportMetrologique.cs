using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutRapportMetrologique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rapport_metrologie",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contenu_rapport = table.Column<byte[]>(unicode: false, nullable: false),
                    nom_document = table.Column<string>(unicode: false, nullable: false),
                    capteurID = table.Column<int>(nullable: false),
                    date_verif_metrologie = table.Column<DateTime>(type: "datetime", nullable: false),
                    type_rapport_metrologie = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rapport_metrologie", x => x.id);
                    table.ForeignKey(
                        name: "FK_rapport_metrologique_capteur",
                        column: x => x.capteurID,
                        principalTable: "capteur",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e2c7f3f5-1ffe-4caf-b3d4-c4d8a78aeea3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "90f9fe35-009e-43c0-978b-15b90a05c914");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "c63ba489-314c-42cd-b7c5-c4f93bdd3439");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "0ddf12ee-f23f-4f75-afee-05468358e1ef");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "044a9918-4616-40a1-9920-d6e935690e50");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "b539b77d-f67c-4a0f-8b95-78e297a4c55b");

            migrationBuilder.CreateIndex(
                name: "IX_rapport_metrologie_capteurID",
                table: "rapport_metrologie",
                column: "capteurID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rapport_metrologie");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "0ece18d9-5ff3-4120-92f7-f0323a121254");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "c7b20fd5-90b7-46da-aaf4-4006c9cedb34");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "9d96dc4f-53f9-47b6-b991-baf1eacfa973");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "08d4deac-9caa-48bc-8c00-0618e0424f63");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "14d272ae-e8b1-4e50-aa37-48437bdacf47");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "cfca80b0-3377-4ebf-8076-707a79d7efa0");
        }
    }
}
