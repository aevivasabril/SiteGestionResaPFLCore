using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AddDocFicheMateriel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "doc_fiche_materiel",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contenu_fiche = table.Column<byte[]>(unicode: false, nullable: false),
                    nom_document = table.Column<string>(unicode: false, nullable: false),
                    date_modification = table.Column<DateTime>(type: "datetime", nullable: false),
                    equipementID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doc_fiche_materiel", x => x.id);
                    table.ForeignKey(
                        name: "FK_doc_fiche_materiel_equipement",
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
                value: "0f56e043-99fb-4464-a5ea-90f8d3a1ddd6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "46e56eb2-a3b1-419e-8f98-5827bf092a39");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "2425766f-5027-4c1b-97ea-a0c67be1f412");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "380ad174-aa33-41b0-8890-f47748a462a0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "19c09d1f-4a9f-4c7b-8c98-98abea778062");

            migrationBuilder.CreateIndex(
                name: "IX_doc_fiche_materiel_equipementID",
                table: "doc_fiche_materiel",
                column: "equipementID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "doc_fiche_materiel");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "bd3482e9-c7f1-4bb3-8b63-0b9db2b1d57a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "51d26f63-4d5e-48f6-bc1e-5e3546cbbe30");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "94158c4c-f90a-40cf-9ef7-5e99947c278a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "d8e6f34d-5d35-4395-8f7c-cdd2adadaa89");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "191e2f16-fd7b-4e90-bfa7-9525a25df6a2");
        }
    }
}
