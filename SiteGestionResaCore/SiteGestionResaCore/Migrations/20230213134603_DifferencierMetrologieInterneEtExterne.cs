using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class DifferencierMetrologieInterneEtExterne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "periodicite_metrologie",
                table: "capteur");

            migrationBuilder.RenameColumn(
                name: "date_prochaine_verif",
                table: "capteur",
                newName: "date_prochaine_verif_int");

            migrationBuilder.RenameColumn(
                name: "date_derniere_verif",
                table: "capteur",
                newName: "date_prochaine_verif_ext");

            migrationBuilder.AddColumn<DateTime>(
                name: "date_derniere_verif_ext",
                table: "capteur",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_derniere_verif_int",
                table: "capteur",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "periodicite_metrologie_ext",
                table: "capteur",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "periodicite_metrologie_int",
                table: "capteur",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "a667eef1-6807-4297-ae08-50d509a4e616");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "261a77a1-3c0c-4c87-b037-87b1a11861a0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "9eed2a0d-473e-4ce7-9347-df968148d601");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "b85f4d6f-0e59-4b2a-b9b5-9c6ceaa7578c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "4db2bbbd-7901-48bb-ace5-b2d87f8d90e0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "24040efd-d635-4dd1-8281-2bb11446356c");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_derniere_verif_ext",
                table: "capteur");

            migrationBuilder.DropColumn(
                name: "date_derniere_verif_int",
                table: "capteur");

            migrationBuilder.DropColumn(
                name: "periodicite_metrologie_ext",
                table: "capteur");

            migrationBuilder.DropColumn(
                name: "periodicite_metrologie_int",
                table: "capteur");

            migrationBuilder.RenameColumn(
                name: "date_prochaine_verif_int",
                table: "capteur",
                newName: "date_prochaine_verif");

            migrationBuilder.RenameColumn(
                name: "date_prochaine_verif_ext",
                table: "capteur",
                newName: "date_derniere_verif");

            migrationBuilder.AddColumn<double>(
                name: "periodicite_metrologie",
                table: "capteur",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e2c7f3f5-1ffe-4caf-b3d4-c4d8a78aeea3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "90f9fe35-009e-43c0-978b-15b90a05c914");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "c63ba489-314c-42cd-b7c5-c4f93bdd3439");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "0ddf12ee-f23f-4f75-afee-05468358e1ef");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "044a9918-4616-40a1-9920-d6e935690e50");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "b539b77d-f67c-4a0f-8b95-78e297a4c55b");
        }
    }
}
