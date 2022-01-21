using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class SuppressionBAL0059 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 168);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "3bcf4744-e759-45d2-b02a-7449c9aca9ce");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "24a732ef-fd10-4187-bcea-c32b4a1605af");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "e50d61f9-aa03-4824-b8ca-53d6e19348e4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "0bf655d4-8cc3-466d-8254-1379dc17f7e6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "65c98939-b374-402b-9022-0320f8ef6442");

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "cheminFicheMateriel", "cheminFicheMetrologie", "mobile", "nom", "nomTabPcVue", "numGmao", "zoneID" },
                values: new object[] { 168, null, null, false, "Analyseur humidité METTLER TOLEDO 71 g (HE73/01)", null, "BAL0059", 11 });
        }
    }
}
