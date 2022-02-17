using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjouterTableDocEssaiXPgd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "doc_essai_pgd",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contenu_document = table.Column<byte[]>(unicode: false, nullable: false),
                    nom_document = table.Column<string>(unicode: false, nullable: false),
                    equipementID = table.Column<int>(nullable: false),
                    essaiID = table.Column<int>(nullable: false),
                    type_documentID = table.Column<int>(nullable: false),
                    date_creation = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doc_essai_pgd", x => x.id);
                    table.ForeignKey(
                        name: "FK_doc_essai_pgd_equipement",
                        column: x => x.equipementID,
                        principalTable: "equipement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_doc_essai_pgd_essai",
                        column: x => x.essaiID,
                        principalTable: "essai",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_doc_essai_pgd_type_document",
                        column: x => x.type_documentID,
                        principalTable: "type_document",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c1e92d53-c07b-42e7-aa0c-c0e48b046b52");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "85f61371-d4f9-4bfd-a9e5-a29b5a089809");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "d6066292-91ee-4e0d-8272-6cfb1289b2c0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "ef550ef7-7bb0-41e3-a6ab-2de194aa4101");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "be1e56da-6c1b-4357-9734-bddd9660bd47");

            migrationBuilder.CreateIndex(
                name: "IX_doc_essai_pgd_equipementID",
                table: "doc_essai_pgd",
                column: "equipementID");

            migrationBuilder.CreateIndex(
                name: "IX_doc_essai_pgd_essaiID",
                table: "doc_essai_pgd",
                column: "essaiID");

            migrationBuilder.CreateIndex(
                name: "IX_doc_essai_pgd_type_documentID",
                table: "doc_essai_pgd",
                column: "type_documentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "doc_essai_pgd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "202a77ab-7e4c-4806-b0dd-1652074c5ec4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "f4ebf951-efc7-4140-9ddd-875c44032585");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "a8b1bbf1-2a95-417a-b3be-3a96adabd3e9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "f4c0ca64-38b1-4fb2-acf1-9d40acdd7876");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "9e50ef3d-630a-4db0-b81c-d57a689f016e");
        }
    }
}
