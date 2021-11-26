using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutTextBlocageZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "17d4a605-e326-460c-92f3-b5291f8e8be8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "02d2a5cc-cf65-4b00-abcb-6605c1bdf0cc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "64cde9eb-562d-4ac2-b2f7-c345a5403a00");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "6ca15c37-7f30-4f18-b4b2-685bbfd49e67");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "7ac98a74-05e8-44b2-a49c-4ff514812f01");

            migrationBuilder.UpdateData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 1,
                column: "nom_type_maintenance",
                value: "Maintenance curative (Dépannage avec blocage de zone)");

            migrationBuilder.UpdateData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 2,
                column: "nom_type_maintenance",
                value: "Maintenance préventive (Interne avec blocage de zone)");

            migrationBuilder.UpdateData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 3,
                column: "nom_type_maintenance",
                value: "Maintenance préventive (Externe avec blocage de zone)");

            migrationBuilder.UpdateData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 4,
                column: "nom_type_maintenance",
                value: "Amélioration (avec blocage de zone)");

            migrationBuilder.UpdateData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 5,
                column: "nom_type_maintenance",
                value: "Equipement en panne (blocage équipement)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "590217a9-25f8-4a8e-9614-466edec3c131");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "95d81847-92ab-4e99-9ca0-cdcab644de27");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "fa99de9e-b3e2-492c-80e3-b6a7299af581");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "87eb7afa-ab1d-4946-b080-0cf2b66840f6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "7b1443b0-aa7e-4400-8176-5a4fe052c9a0");

            migrationBuilder.UpdateData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 1,
                column: "nom_type_maintenance",
                value: "Maintenance curative (Dépannage)");

            migrationBuilder.UpdateData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 2,
                column: "nom_type_maintenance",
                value: "Maintenance préventive (Interne)");

            migrationBuilder.UpdateData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 3,
                column: "nom_type_maintenance",
                value: "Maintenance préventive (Externe)");

            migrationBuilder.UpdateData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 4,
                column: "nom_type_maintenance",
                value: "Amélioration");

            migrationBuilder.UpdateData(
                table: "ld_type_maintenance",
                keyColumn: "id",
                keyValue: 5,
                column: "nom_type_maintenance",
                value: "Equipement en panne");
        }
    }
}
