using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutEquipementsSalAP7ABC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "46f43ce6-9f2b-4382-992a-a1f4c8f33d53");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "ca88629f-3544-4e3e-8a39-9203504247d7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "eff3e55c-4afa-4dc7-b540-d488f199b27a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "3894332f-e47e-4307-b17d-d944f0b9bc7e");

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "mobile", "nom", "nomTabPcVue", "numGmao", "zoneID" },
                values: new object[,]
                {
                    { 256, false, "Salle AP7 A", null, "CHF015", 14 },
                    { 257, false, "Salle AP7 B", null, "CHF021", 15 },
                    { 258, false, "Salle AP7 C", null, "CHF022", 16 }
                });

            migrationBuilder.UpdateData(
                table: "zone",
                keyColumn: "id",
                keyValue: 12,
                column: "nom_zone",
                value: "Salle Ap5");

            migrationBuilder.UpdateData(
                table: "zone",
                keyColumn: "id",
                keyValue: 13,
                column: "nom_zone",
                value: "Salle Ap6");
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
                keyValue: 12,
                column: "nom_zone",
                value: "Salle alimentaire Ap5");

            migrationBuilder.UpdateData(
                table: "zone",
                keyColumn: "id",
                keyValue: 13,
                column: "nom_zone",
                value: "Salle alimentaire Ap6");
        }
    }
}
