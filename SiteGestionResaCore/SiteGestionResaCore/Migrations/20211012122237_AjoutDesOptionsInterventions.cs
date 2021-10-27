using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutDesOptionsInterventions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "0a33afae-c71d-4ba0-bb9d-5eefc6eef8b8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "2e4b7d67-c8cc-421f-b9bb-efc12ac8ddb5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "1329455c-1211-40c9-a871-9266eb8f807c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "044176ee-659b-4d5c-99bf-4b06f8128972");

            migrationBuilder.UpdateData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 1,
                column: "nom_type_maintenance",
                value: "Maintenance curative (Dépannage)");

            migrationBuilder.InsertData(
                table: "ld_type_maintenance",
                columns: new[] { "id", "nom_type_maintenance" },
                values: new object[,]
                {
                    { 5, "Equipement en panne" },
                    { 6, "Maintenance curative (Dépannage sans blocage zone)" },
                    { 7, "Maintenance préventive (Interne sans blocage de zone)" },
                    { 8, "Maintenance préventive (Externe sans blocage de zone)" },
                    { 9, "Amélioration (sans blocage de zone)" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 9);

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
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 1,
                column: "nom_type_maintenance",
                value: "Maintenance curative (Panne)");
        }
    }
}
