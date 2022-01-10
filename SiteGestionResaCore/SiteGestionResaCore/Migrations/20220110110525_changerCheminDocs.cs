using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class changerCheminDocs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "fe501f86-c3a3-4a65-b208-cfb547ee1431");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "ca0c2e89-26f3-4759-8b53-c20103a412d3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "08c2cfc9-24ff-459b-af0c-b2816c13fd8f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "25d7955a-2633-4b28-8fe8-c14b95abc7d8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "e1da98bd-da28-42bf-955a-67bce9e864f3");

            migrationBuilder.UpdateData(
                table: "doc_qualite",
                keyColumn: "id",
                keyValue: 1,
                column: "chemin_document",
                value: "D:\\SiteReservation2021\\smq-site-resa\\doc_qualite\\politique-qualité.pdff");

            migrationBuilder.UpdateData(
                table: "doc_qualite",
                keyColumn: "id",
                keyValue: 2,
                column: "chemin_document",
                value: "D:\\SiteReservation2021\\smq-site-resa\\doc_qualite\\certificat-lrqa.pdf");

            migrationBuilder.UpdateData(
                table: "doc_qualite",
                keyColumn: "id",
                keyValue: 3,
                column: "chemin_document",
                value: "D:\\SiteReservation2021\\smq-site-resa\\doc_qualite\\organigramme.pdf");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "2c82ddef-a8ba-4728-9a28-4accd3ef5f41");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e6ae8621-03f0-48dd-b714-780f6ce0eb35");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "6d4e18f9-1bdb-4cfe-8f61-bf29d92cf2e4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "a1e716c0-e104-49ea-9fcc-d5b6f23b609f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "f9ba2cfd-72de-48a4-b330-c9587286bfc4");

            migrationBuilder.UpdateData(
                table: "doc_qualite",
                keyColumn: "id",
                keyValue: 1,
                column: "chemin_document",
                value: "M:\\PFL\\smq-pfl\\smq-site-resa\\politique-qualité.pdf");

            migrationBuilder.UpdateData(
                table: "doc_qualite",
                keyColumn: "id",
                keyValue: 2,
                column: "chemin_document",
                value: "M:\\PFL\\smq-pfl\\smq-site-resa\\certificat-lrqa.pdf");

            migrationBuilder.UpdateData(
                table: "doc_qualite",
                keyColumn: "id",
                keyValue: 3,
                column: "chemin_document",
                value: "M:\\PFL\\smq-pfl\\smq-site-resa\\organigramme.pdf");
        }
    }
}
