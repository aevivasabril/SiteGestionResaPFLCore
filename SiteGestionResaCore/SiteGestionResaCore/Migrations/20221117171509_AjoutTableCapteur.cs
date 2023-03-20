using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutTableCapteur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "capteur",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_capteur = table.Column<string>(unicode: false, nullable: false),
                    code_capteur = table.Column<string>(unicode: false, nullable: false),
                    equipementID = table.Column<int>(nullable: false),
                    date_prochaine_verif = table.Column<DateTime>(type: "datetime", nullable: true),
                    date_derniere_verif = table.Column<DateTime>(type: "datetime", nullable: true),
                    periodicite_metrologie = table.Column<double>(nullable: false),
                    capteur_conforme = table.Column<bool>(nullable: false),
                    emt_capteur = table.Column<double>(nullable: false),
                    facteur_correctif = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_capteur", x => x.id);
                    table.ForeignKey(
                        name: "FK_capteur_equipement",
                        column: x => x.equipementID,
                        principalTable: "equipement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e6091087-8ad3-4970-a9c4-3a08b1107b2e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1d0cad7d-cf35-46fe-ad53-830ec0c73b15");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "fb2ed8c2-179a-4f17-8168-92dd9fb869e0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "224466fc-7b7b-4737-85c9-dfd22be6bfa1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "efc29fa5-6ec5-4929-b121-103755016745");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "3812d0c7-4906-4eb1-b76e-fec0080e951d");

            migrationBuilder.CreateIndex(
                name: "IX_capteur_equipementID",
                table: "capteur",
                column: "equipementID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "capteur");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d697ab5e-0f76-4161-aab2-8a6e65caa299");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3643bd4f-f7b8-40b5-ae24-89fb418c474d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "bfa24627-fda2-49ec-8179-a86f720cc246");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "51080f98-6266-4b63-a91d-f76e2c20f633");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "858769a3-5181-420d-9d09-54f310ad2181");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "5748bfb6-31d7-4668-b9ea-49412ba84e6f");
        }
    }
}
