using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutOperationMetrologieXBlocage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5f9001b5-987a-44e7-99f9-1ac56c3454c9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "945ca5db-dc0b-4ac9-a0e1-d16c2690f9ef");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "d509125a-a0c7-45c3-99db-228538be7b31");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "98d0590d-c182-40a3-bac5-92dec5671a93");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "6bb6d919-7d9b-4fa7-9303-2d9b709ec9d1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "5320d765-b0d0-43a1-8b1d-fa0297c650e3");

            migrationBuilder.InsertData(
                table: "ld_type_maintenance",
                columns: new[] { "id", "nom_type_maintenance" },
                values: new object[] { 10, "Intervention métrologique (blocage de l'équipement)" });

            migrationBuilder.UpdateData(
                table: "ld_type_projet",
                keyColumn: "id",
                keyValue: 4,
                column: "nom_type_projet",
                value: "Industriel (cellules hébergés)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7a1d03bb-4e0d-42f4-a84b-33a46c3b626a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "f21b105a-a451-4c6c-96ec-a6dbb7f3d059");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "8aa28ffe-e733-4fa5-ba32-a6acb73d2e44");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "d2ba7649-019b-40ff-ac72-dcae631b5f32");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "69221ee4-d15e-4ff8-8c4e-4a8e2be57693");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "00628428-374a-4c1a-b7e1-2cb7270c80d5");

            migrationBuilder.UpdateData(
                table: "ld_type_projet",
                keyColumn: "id",
                keyValue: 4,
                column: "nom_type_projet",
                value: "Industriel (cellules hébergés");
        }
    }
}
