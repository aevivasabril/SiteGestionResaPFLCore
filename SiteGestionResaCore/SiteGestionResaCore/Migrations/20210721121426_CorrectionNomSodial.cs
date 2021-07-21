using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class CorrectionNomSodial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "organisme",
                keyColumn: "id",
                keyValue: 6,
                column: "nom_organisme",
                value: "Sodiaal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "42843173-1929-454b-8eb7-226ef14aa1df");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "97e1a161-0979-484a-b71c-60436768ec91");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "44f36d08-ebeb-474e-9242-e89c09a594a1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "59cf0b9d-2aac-4e72-b816-cf5b85e40a42");

            migrationBuilder.UpdateData(
                table: "organisme",
                keyColumn: "id",
                keyValue: 6,
                column: "nom_organisme",
                value: "Sodial");
        }
    }
}
