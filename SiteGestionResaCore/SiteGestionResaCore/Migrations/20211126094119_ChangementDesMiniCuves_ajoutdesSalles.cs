using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class ChangementDesMiniCuves_ajoutdesSalles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "a2d8bbd4-6e14-4a62-bf51-24bca45e8db9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "51cd082c-b5b7-4fbc-a6f0-31f12436eb78");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "1b3c64e6-9b11-4980-85ca-b0da04013db5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "67f7a013-1e47-477a-ad2b-8658246c07cd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "549abd07-c046-460f-a7c0-a093df3542b4");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 196,
                column: "zoneID",
                value: 5);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 197,
                column: "zoneID",
                value: 5);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 198,
                column: "zoneID",
                value: 5);

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "mobile", "nom", "nomTabPcVue", "numGmao", "zoneID" },
                values: new object[,]
                {
                    { 261, false, "Salle Saumurage", null, "CHF018", 10 },
                    { 262, false, "Pâtes molles moulage", null, "LAB0048", 5 },
                    { 263, false, "Pâtes molles tranchage", null, "LAB0049", 6 },
                    { 264, false, "Labo", null, "LAB0017", 11 },
                    { 265, false, "Pâtes préssées cuites", null, "LAB0047", 7 },
                    { 266, false, "Salle Sthepan", null, "LAB0046", 9 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 261);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 262);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 263);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 264);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 265);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 266);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "554e8068-e440-4087-93dd-69750de36c15");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "ec3c68a5-8e55-46d5-805f-88d56a2bc483");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "2e913d33-27c4-4976-8155-9485242c65e1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "d8cbdec5-1951-4b4a-864d-eeba1c03cee9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "cf9ae2a8-fad9-47aa-b2c0-39cbff743fba");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 196,
                column: "zoneID",
                value: 7);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 197,
                column: "zoneID",
                value: 7);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 198,
                column: "zoneID",
                value: 7);
        }
    }
}
