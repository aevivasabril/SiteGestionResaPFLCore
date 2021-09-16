using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutCuv850LZoneMembranes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "06d2479c-63c5-44fa-888c-1a24416b8427");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "43be82de-0e2f-4950-9c85-74f622847b0d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "153b3bd1-9e06-4507-b2c3-2aa96fde4ee1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "7c848d91-1137-429f-8593-1d1ae70d3288");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 241,
                column: "numGmao",
                value: "ECUV0034, ECUV0035, ECUV0036");

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "mobile", "nom", "nomTabPcVue", "numGmao", "zoneID" },
                values: new object[] { 259, false, "Tank 850 L", null, "ECUV0038", 4 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 259);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "71a5dea0-bed0-49d5-8e15-5509c4aa6a2e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a8099981-36e3-49c0-892a-fc6f18a5c3e3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "04305f36-268f-4af6-9122-88d06c43c67f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "c419825a-e493-42e1-b61c-4e2640fef6d4");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 241,
                column: "numGmao",
                value: "ACTALIA");
        }
    }
}
