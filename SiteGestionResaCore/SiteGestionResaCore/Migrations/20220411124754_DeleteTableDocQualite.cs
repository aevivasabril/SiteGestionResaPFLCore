using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class DeleteTableDocQualite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "doc_qualite");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "6e7bfd2c-c3a8-4d35-a177-d86758da66f8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "0f16180a-175c-4cc9-9bad-ea92bf91b106");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "639263cc-03cf-4dc9-a890-cb20d4a2bbfa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "68b1240d-f700-4c65-81a6-0cc895f8996d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "5cff1c39-036f-45a3-aafe-6bc4d5f5f5eb");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "doc_qualite",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chemin_document = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    description_doc = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    nom_document = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doc_qualite", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "be18dbba-bf28-417e-8d84-a691f8a87c5a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "0ffa9cc3-f857-4a4b-9cfa-76a43eb7386d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "d0220ab6-82f5-46f9-9bf0-855bee8091a2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "ae555134-82ac-43ac-9bae-25b3377180d0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "6b072013-bd9a-438f-83e2-6a4b959810d2");

            migrationBuilder.InsertData(
                table: "doc_qualite",
                columns: new[] { "id", "chemin_document", "description_doc", "nom_document" },
                values: new object[,]
                {
                    { 1, "D:\\SiteReservation2021\\smq-site-resa\\doc_qualite\\politique-qualité.pdf", null, "Politique qualité" },
                    { 2, "D:\\SiteReservation2021\\smq-site-resa\\doc_qualite\\certificat-lrqa.pdf", "Document de certification norme ISO 9001", "Certificat LRQA" },
                    { 3, "D:\\SiteReservation2021\\smq-site-resa\\doc_qualite\\organigramme.pdf", null, "Organigramme de la Plate-forme LAIT" }
                });
        }
    }
}
