using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class SuppressionLienFicheMatPILOT0014_et_manuelQualite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "doc_qualite",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "2c82ddef-a8ba-4728-9a28-4accd3ef5f41");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e6ae8621-03f0-48dd-b714-780f6ce0eb35");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "6d4e18f9-1bdb-4cfe-8f61-bf29d92cf2e4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "a1e716c0-e104-49ea-9fcc-d5b6f23b609f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "f9ba2cfd-72de-48a4-b330-c9587286bfc4");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 225,
                column: "cheminFicheMateriel",
                value: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "fe440907-5f89-4d6d-a598-4b1238294d04");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "960e6ba6-93e6-4b43-a785-a1f4a1fbac87");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "90de1e55-9194-416f-be43-491e1091034f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "0bbfa23b-4879-4a61-aeb8-5ba4a4d3e52f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "626531d0-f75c-4034-9a7a-2747c4f5052b");

            migrationBuilder.InsertData(
                table: "doc_qualite",
                columns: new[] { "id", "chemin_document", "description_doc", "nom_document" },
                values: new object[] { 4, "M:\\PFL\\smq-pfl\\smq-site-resa\\manuel-qualite.pdf", null, "Manuel Qualité" });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 225,
                column: "cheminFicheMateriel",
                value: "M:\\PFL\\Dossier matériel et métrologie\\Dossiers finalisés\\EN-MAT-123_évapo.doc");
        }
    }
}
