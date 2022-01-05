using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AddDocsToTableDocQualite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "doc_qualite",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_document = table.Column<string>(unicode: false, nullable: false),
                    chemin_document = table.Column<string>(unicode: false, nullable: false),
                    description_doc = table.Column<string>(unicode: false, nullable: true)
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
                value: "fe440907-5f89-4d6d-a598-4b1238294d04");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "960e6ba6-93e6-4b43-a785-a1f4a1fbac87");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "90de1e55-9194-416f-be43-491e1091034f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "0bbfa23b-4879-4a61-aeb8-5ba4a4d3e52f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "626531d0-f75c-4034-9a7a-2747c4f5052b");

            migrationBuilder.InsertData(
                table: "doc_qualite",
                columns: new[] { "id", "chemin_document", "description_doc", "nom_document" },
                values: new object[,]
                {
                    { 1, "M:\\PFL\\smq-pfl\\smq-site-resa\\politique-qualité.pdf", null, "Politique qualité" },
                    { 2, "M:\\PFL\\smq-pfl\\smq-site-resa\\certificat-lrqa.pdf", "Document de certification norme ISO 9001", "Certificat LRQA" },
                    { 3, "M:\\PFL\\smq-pfl\\smq-site-resa\\organigramme.pdf", null, "Organigramme de la Plate-forme LAIT" },
                    { 4, "M:\\PFL\\smq-pfl\\smq-site-resa\\manuel-qualite.pdf", null, "Manuel Qualité" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "doc_qualite");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "196819d9-2e55-4a16-837d-1d9d8502dedd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "9bb178e8-b8fc-4925-990d-6c6134f50cd3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "a65559b6-b45e-420d-b8b6-ee0e0011fa48");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "f933fe52-fbcf-4f35-80ae-759510f61ae5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "c76536a4-48ab-4d4c-84f9-57bc0ca6ca26");
        }
    }
}
