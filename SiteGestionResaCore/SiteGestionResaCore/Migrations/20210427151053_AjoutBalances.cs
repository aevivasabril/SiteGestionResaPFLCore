using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutBalances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "403f1a66-36dc-4f10-9c87-3c933f416d3c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "dd1a593c-445c-4f19-aa88-06369fb2d768");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "211a3d4e-2b36-4958-8837-d43d1c561efa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "5563703e-12f5-4169-b640-a4f114482b00");

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "mobile", "nom", "nomTabPcVue", "numGmao", "zoneID" },
                values: new object[,]
                {
                    { 245, true, "Balance OHAUS Ranger 3000 -30Kg- tour de sechage", null, "BAL0068", 1 },
                    { 246, true, "Balance OHAUS Ranger 3000 -30Kg", null, "BAL0074", 12 },
                    { 247, true, "Balance PRECIA MOLEN 150 kg", null, "BAL0073", 7 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 247);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e9f8887a-a747-4973-a57a-986af6146a60");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "d0b0ed54-c3b8-4f4b-9997-a2166351cfa0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "be84df92-e33b-4225-9f36-9785f7a5c95d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "0b145b42-f9a7-43c4-914d-4dd1c5c87a0d");
        }
    }
}
