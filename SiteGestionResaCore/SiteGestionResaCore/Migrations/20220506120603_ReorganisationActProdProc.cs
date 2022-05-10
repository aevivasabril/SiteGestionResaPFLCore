using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class ReorganisationActProdProc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d05a5b67-0cd5-4759-8180-e7e41180a815");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "2fd85e5f-d46f-42b5-9160-6e6be241f9ac");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "4e769f46-43cc-4c1d-8c26-3e8998414a6c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "215af7e1-aeb7-4ab4-89a9-b3539180cfcf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "2d796a18-22d8-4672-80dc-a7d08d4132fc");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 19,
                column: "nom_activite",
                value: "Processed Cheese");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 20,
                column: "nom_activite",
                value: "Nanofiltration");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 21,
                column: "nom_activite",
                value: "Autres filtrations");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 22,
                column: "nom_activite",
                value: "Evaporation");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 23,
                column: "nom_activite",
                value: "Séchage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c4724648-48dd-4449-8c9d-44dcd65f4c48");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "9f28a94f-1328-42b6-a3f0-16e0536583d0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "d1206729-acf4-4a4e-89ff-fa6afeacfc04");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "9fcaa7bc-c950-4484-b0e5-209bea4ab120");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "c0da1e79-052e-412d-bd1c-363c9b639f50");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 19,
                column: "nom_activite",
                value: "Nanofiltration");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 20,
                column: "nom_activite",
                value: "Autres filtrations");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 21,
                column: "nom_activite",
                value: "Evaporation");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 22,
                column: "nom_activite",
                value: "Séchage");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 23,
                column: "nom_activite",
                value: "Processed Cheese");
        }
    }
}
