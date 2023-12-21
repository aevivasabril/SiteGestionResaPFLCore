using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class RajoutColonnesXInterventions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "actions_realisees",
                table: "reservation_maintenance",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "interv_fini",
                table: "reservation_maintenance",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "actions_realisees",
                table: "resa_maint_equip_adjacent",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "interv_fini",
                table: "resa_maint_equip_adjacent",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "efb1c38a-5991-48f5-9d99-4f508b14f946");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "92f14221-5303-4bbe-936f-0681de6e2077");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "11eb9ee3-593b-4ec1-9f36-b789f0a84181");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "d4ec7463-7261-4a5b-9c50-8b494eda470b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "83d6dc93-0c2c-4261-8655-6c996c1b8738");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "3f5cedba-39aa-4f2d-803e-c51de943d7ca");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "actions_realisees",
                table: "reservation_maintenance");

            migrationBuilder.DropColumn(
                name: "interv_fini",
                table: "reservation_maintenance");

            migrationBuilder.DropColumn(
                name: "actions_realisees",
                table: "resa_maint_equip_adjacent");

            migrationBuilder.DropColumn(
                name: "interv_fini",
                table: "resa_maint_equip_adjacent");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "42a8b14b-dd91-411e-ab53-ac783e69b52c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "2ac800db-53f3-403f-b80c-009d5c0d3bbb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "614b40b5-86cb-40fe-b7cb-5f43dc04ab17");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "6ee87879-1d3a-48cf-b686-23247d83aa22");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "721cc604-bde1-47a0-87a3-ede7bee57c5a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "818256d0-57e8-4468-b271-c80fcac68d46");
        }
    }
}
