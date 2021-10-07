using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutBalanceSalleAP9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "0e217275-003d-4535-845f-24076fd8389c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "8fc40f3e-44cc-48ee-a9fc-c4e7bbf32e54");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "aaacd971-dd7f-442b-944a-f725926507b7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "9d005b60-9e83-4b4e-b499-c723918cc649");

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "mobile", "nom", "nomTabPcVue", "numGmao", "zoneID" },
                values: new object[] { 260, true, "Balance OHAUS Ranger 3000 -30Kg", null, "BAL0079", 12 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 260);

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
        }
    }
}
