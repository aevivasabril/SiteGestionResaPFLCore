using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutCleDocumentVsActivite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doc_essai_pgd_equipement",
                table: "doc_essai_pgd");

            migrationBuilder.AlterColumn<int>(
                name: "equipementID",
                table: "doc_essai_pgd",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "type_activiteID",
                table: "doc_essai_pgd",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "fbfb03d6-107c-4434-8304-a9b1dfeb0feb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "c99ce5b2-b6d4-403b-ac31-aedf423f18bd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "f502bb6c-7561-483e-91c9-e797d2f2a2da");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "6c7c57d3-20bc-448b-b483-890b450962e4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "29ba012a-69b6-4625-87ef-fe12a0dca269");

            migrationBuilder.CreateIndex(
                name: "IX_doc_essai_pgd_type_activiteID",
                table: "doc_essai_pgd",
                column: "type_activiteID");

            migrationBuilder.AddForeignKey(
                name: "FK_doc_essai_pgd_equipement",
                table: "doc_essai_pgd",
                column: "equipementID",
                principalTable: "equipement",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_doc_essai_pgd_activite_pfl",
                table: "doc_essai_pgd",
                column: "type_activiteID",
                principalTable: "activite_pfl",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doc_essai_pgd_equipement",
                table: "doc_essai_pgd");

            migrationBuilder.DropForeignKey(
                name: "FK_doc_essai_pgd_activite_pfl",
                table: "doc_essai_pgd");

            migrationBuilder.DropIndex(
                name: "IX_doc_essai_pgd_type_activiteID",
                table: "doc_essai_pgd");

            migrationBuilder.DropColumn(
                name: "type_activiteID",
                table: "doc_essai_pgd");

            migrationBuilder.AlterColumn<int>(
                name: "equipementID",
                table: "doc_essai_pgd",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_doc_essai_pgd_equipement",
                table: "doc_essai_pgd",
                column: "equipementID",
                principalTable: "equipement",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
