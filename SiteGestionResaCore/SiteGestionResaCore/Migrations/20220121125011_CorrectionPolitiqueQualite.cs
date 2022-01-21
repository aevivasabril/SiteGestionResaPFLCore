using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class CorrectionPolitiqueQualite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "8352050e-bbaa-447d-bf4c-19daafc34cc6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "54cb73a3-77e4-4d95-8cbb-264954c34073");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "dd12c11a-d832-4618-b27c-f211f45aea2b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "befade2a-2767-4f84-9961-4b61f83fd100");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "4f489d87-6c34-49f1-a8dc-a4056229258f");

            migrationBuilder.UpdateData(
                table: "doc_qualite",
                keyColumn: "id",
                keyValue: 1,
                column: "chemin_document",
                value: "D:\\SiteReservation2021\\smq-site-resa\\doc_qualite\\politique-qualité.pdf");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "aa9a9874-2ef9-4a53-ae9b-71c4a5ac28d9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1bab337c-a6ef-4a0e-9f9a-eb09839a011e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "d1902519-16eb-402b-a4f4-76b6a99e5f3c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "a923f8f3-e91c-4962-b818-87d8db41789e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "98227e4c-9f61-414e-9339-e530c8651900");

            migrationBuilder.UpdateData(
                table: "doc_qualite",
                keyColumn: "id",
                keyValue: 1,
                column: "chemin_document",
                value: "D:\\SiteReservation2021\\smq-site-resa\\doc_qualite\\politique-qualité.pdff");
        }
    }
}
