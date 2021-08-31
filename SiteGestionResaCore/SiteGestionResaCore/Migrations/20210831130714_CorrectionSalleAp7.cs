using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class CorrectionSalleAp7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "zone",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "11e21d53-127c-4e82-8789-c560750a0ac1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "22dde20f-efea-480e-a3be-e578a1f7e2f4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "f0f5996e-29bf-417f-8cf8-1481b779cd43");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "565f684e-4def-4b89-a7b6-3e62130b68a5");

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "mobile", "nom", "nomTabPcVue", "numGmao", "zoneID" },
                values: new object[,]
                {
                    { 256, false, "Salle AP7 A", null, "CHF015", 18 },
                    { 257, false, "Salle AP7 B", null, "CHF021", 19 }
                });

            migrationBuilder.UpdateData(
                table: "zone",
                keyColumn: "id",
                keyValue: 15,
                column: "nom_zone",
                value: "Salle alimentaire Ap8");

            migrationBuilder.UpdateData(
                table: "zone",
                keyColumn: "id",
                keyValue: 16,
                column: "nom_zone",
                value: "Salle alimentaire Ap9");

            migrationBuilder.UpdateData(
                table: "zone",
                keyColumn: "id",
                keyValue: 17,
                column: "nom_zone",
                value: "Equipements mobiles");

            migrationBuilder.UpdateData(
                table: "zone",
                keyColumn: "id",
                keyValue: 18,
                column: "nom_zone",
                value: "Salle AP7 A");

            migrationBuilder.UpdateData(
                table: "zone",
                keyColumn: "id",
                keyValue: 19,
                column: "nom_zone",
                value: "Salle AP7 B");

            migrationBuilder.InsertData(
                table: "zone",
                columns: new[] { "id", "nom_zone" },
                values: new object[] { 20, "Salle AP7 C" });

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "mobile", "nom", "nomTabPcVue", "numGmao", "zoneID" },
                values: new object[] { 258, false, "Salle AP7 C", null, "CHF022", 20 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 256);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 257);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 258);

            migrationBuilder.DeleteData(
                table: "zone",
                keyColumn: "id",
                keyValue: 20);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "fa56b92e-72ff-48cf-b503-b93550878491");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "d2a4b1ff-e905-4394-8eff-65f3cfe43290");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "4d4b8512-4deb-4e79-8dd8-d8482834aad6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "0efbaf6c-9dec-400a-aed2-f7e7b603c79d");

            migrationBuilder.UpdateData(
                table: "zone",
                keyColumn: "id",
                keyValue: 15,
                column: "nom_zone",
                value: "Salle Ap7 B");

            migrationBuilder.UpdateData(
                table: "zone",
                keyColumn: "id",
                keyValue: 16,
                column: "nom_zone",
                value: "Salle Ap7 C");

            migrationBuilder.UpdateData(
                table: "zone",
                keyColumn: "id",
                keyValue: 17,
                column: "nom_zone",
                value: "Salle alimentaire Ap8");

            migrationBuilder.UpdateData(
                table: "zone",
                keyColumn: "id",
                keyValue: 18,
                column: "nom_zone",
                value: "Salle alimentaire Ap9");

            migrationBuilder.UpdateData(
                table: "zone",
                keyColumn: "id",
                keyValue: 19,
                column: "nom_zone",
                value: "Equipements mobiles");

            migrationBuilder.InsertData(
                table: "zone",
                columns: new[] { "id", "nom_zone" },
                values: new object[] { 14, "Salle Ap7 A" });
        }
    }
}
