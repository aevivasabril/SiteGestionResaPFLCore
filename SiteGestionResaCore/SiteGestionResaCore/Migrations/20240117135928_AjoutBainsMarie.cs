using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutBainsMarie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ad90df36-1746-4913-8a0f-a6cdc1b0280b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "abaa7cdd-76c0-4fd4-bd09-f47ad4f0bdd0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "1ca09524-fdff-4df0-ab64-8d70c8fa8d8d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "e39a1d30-0f83-4cab-ac8f-278c4b7d837c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "913dca53-5eb1-4951-ad64-11c622dfd749");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "7762ffe3-68a2-40f4-b231-4dcd0dc237ff");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 214,
                column: "nom",
                value: "Pilote UF TIA (JYG)");

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "equip_delete", "mobile", "nom", "nomTabPcVue", "numGmao", "type_activites", "zoneID" },
                values: new object[,]
                {
                    { 269, null, true, "Bain-marie sans couvercle", null, "BTH0066", "9", 13 },
                    { 270, null, true, "Bain-marie MEMMERT (avec couvercle, petite capacité)", null, "BTH0065", "9", 16 },
                    { 271, null, true, "Bain-marie MEMMERT (avec couvercle, grande capacité)", null, "BTH0033", "9", 15 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 269);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 270);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 271);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5fc62038-83b3-49f5-a45f-d54a52fd8698");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "838c9253-6252-44bf-875a-7b12c31e9869");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "70bcfa21-fcba-41b1-8322-15f0d8700f8d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "4089afa1-7afe-427e-ada5-20e0c0aade44");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "ee2afca7-c042-423e-b4c3-8a5aaaa25625");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "2da4b473-94b7-4d82-acb4-d241039e349a");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 214,
                column: "nom",
                value: "Pilote UF TIA/PALL 0,02u (JYG)");
        }
    }
}
