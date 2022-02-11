using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutTableTypeDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "type_document",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_document = table.Column<string>(unicode: false, nullable: false),
                    identificateur = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_type_document", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b3a45f43-e7ff-4d6f-9a81-eb212b5e0d84");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "2600f53c-4e9d-4e6d-8a07-339d21282327");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "6e103f2e-eeaf-4636-901e-749ac72a3a37");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "d0311783-6a63-4337-819d-e45b882397a7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "099c3c91-5598-462e-9916-e3ccb99ff411");

            migrationBuilder.InsertData(
                table: "type_document",
                columns: new[] { "id", "identificateur", "nom_document" },
                values: new object[,]
                {
                    { 1, "PC", "Données Physico-chimiques" },
                    { 2, "M", "Données Microbiologiques" },
                    { 3, "R", "Données Rhéologiques" },
                    { 4, "E", "Tableau excel recapitulatif" },
                    { 5, "W", "Compte rendu Word" },
                    { 6, "A", "Autre format" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "type_document");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e716b2ce-1aa6-4924-9759-0c4692652bba");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "d3f5fc48-65aa-422a-9db9-6f6f2744ef82");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "9a2d3a83-40e4-412d-9680-e6b68e7addbd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "7e21fd65-c07e-40d2-9fd5-838c00ea528a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "43b71b2e-22a7-4314-984c-2fce9233f3d0");
        }
    }
}
