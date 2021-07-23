using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutCoquard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "mobile", "nom", "nomTabPcVue", "numGmao", "zoneID" },
                values: new object[] { 255, false, "Cuve 10 lts Coquard", null, "CUISMEL0002", 12 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 255);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "11c60d10-1f79-4058-9a94-3e39797c5576");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "b249b1a8-e7c9-4cf4-9b18-42ca4566e540");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "68a5f62d-8eb8-40ba-b125-832172d05313");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "9bdf8df1-174c-4a52-9a4f-d7a0110d2318");
        }
    }
}
