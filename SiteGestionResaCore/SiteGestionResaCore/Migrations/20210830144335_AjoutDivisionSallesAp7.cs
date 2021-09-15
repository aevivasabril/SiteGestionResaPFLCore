using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutDivisionSallesAp7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                keyValue: 14,
                column: "nom_zone",
                value: "Salle Ap7 A");

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

            migrationBuilder.InsertData(
                table: "zone",
                columns: new[] { "id", "nom_zone" },
                values: new object[,]
                {
                    { 18, "Salle alimentaire Ap9" },
                    { 19, "Equipements mobiles" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "zone",
                keyColumn: "id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "zone",
                keyColumn: "id",
                keyValue: 19);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c19f6894-aaec-4724-b646-4a79a9c91319");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "0d7442f2-d39a-4689-9977-ac71d03e1369");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "59fae6a4-643b-4822-89b6-4bb933c81123");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "ea8131af-deed-4ab6-8367-46856eab523f");

            migrationBuilder.UpdateData(
                table: "zone",
                keyColumn: "id",
                keyValue: 14,
                column: "nom_zone",
                value: "Hâloir Ap7");

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
        }
    }
}
