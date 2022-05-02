using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutTableListeActivites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "list_activites",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_activite = table.Column<string>(unicode: false, nullable: false),
                    type_documents = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_list_activites", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "66187d40-d039-4085-a4c6-6bb68d75b587");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3fb19b1b-8d7d-4c46-9c00-782b823c8e02");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "6fced70f-6d06-4b7a-9f4c-1f541213d9e5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "6e697aee-4bf4-40e8-9a87-3716e59d6a86");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "68f99e50-9e80-47de-b3a1-37e84408e1b0");

            migrationBuilder.InsertData(
                table: "list_activites",
                columns: new[] { "id", "nom_activite", "type_documents" },
                values: new object[,]
                {
                    { 23, "Autres", "PC,M,E,W,A,R" },
                    { 22, "Processed Cheese", "PC,M,E,W,A,R" },
                    { 21, "Séchage", "PC,M,E,W,A,R" },
                    { 20, "Evaporation", "PC,M,E,W,A,R" },
                    { 19, "Autres filtrations", "PC,M,E,W,A,R" },
                    { 18, "Nanofiltration", "PC,M,E,W,A,R" },
                    { 17, "Autres pâtes", "PC,M,E,W,A,R" },
                    { 16, "Pâtes cuites", "PC,M,E,W,A,R" },
                    { 15, "Pâtes pressées", "PC,M,E,W,A,R" },
                    { 14, "Pâtes molles", "PC,M,E,W,A,R" },
                    { 12, "Maturation/stockage", "PC,M,E,W" },
                    { 11, "Ultrafiltration", "PC,M,E,W" },
                    { 10, "Homogénéisation", "PC,M,E,W" },
                    { 9, "Ecrémage", "PC,M,E,W" },
                    { 8, "Traitement thermique", "PC,M,E,W" },
                    { 7, "Microfiltration", "PC,M,E,W" },
                    { 6, "Stockage", "PC,M,E,W" },
                    { 5, "Matière première 3", "PC,M,E,W" },
                    { 4, "Matière première 2", "PC,M,E,W" },
                    { 3, "Matière première 1", "PC,M,E,W" },
                    { 2, "Ingrédients", "PC,M,E,W" },
                    { 13, "Pâtes fraîches", "PC,M,E,W,A,R" },
                    { 1, "Matières premières", "PC,M,E,W" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "list_activites");

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
        }
    }
}
