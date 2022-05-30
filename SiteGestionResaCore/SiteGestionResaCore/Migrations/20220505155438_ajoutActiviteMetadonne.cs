using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class ajoutActiviteMetadonne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c4724648-48dd-4449-8c9d-44dcd65f4c48");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "9f28a94f-1328-42b6-a3f0-16e0536583d0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "d1206729-acf4-4a4e-89ff-fa6afeacfc04");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "9fcaa7bc-c950-4484-b0e5-209bea4ab120");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "c0da1e79-052e-412d-bd1c-363c9b639f50");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "nom_activite", "type_documents" },
                values: new object[] { "Métadonnées", "A,W" });

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 2,
                column: "nom_activite",
                value: "Matières premières");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 3,
                column: "nom_activite",
                value: "Ingrédients");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 4,
                column: "nom_activite",
                value: "Matière première 1");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 5,
                column: "nom_activite",
                value: "Matière première 2");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 6,
                column: "nom_activite",
                value: "Matière première 3");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 7,
                column: "nom_activite",
                value: "Stockage");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 8,
                column: "nom_activite",
                value: "Microfiltration");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 9,
                column: "nom_activite",
                value: "Traitement thermique");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 10,
                column: "nom_activite",
                value: "Ecrémage");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 11,
                column: "nom_activite",
                value: "Homogénéisation");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 12,
                column: "nom_activite",
                value: "Ultrafiltration");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "nom_activite", "type_documents" },
                values: new object[] { "Maturation/stockage", "PC,M,E,W" });

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 14,
                column: "nom_activite",
                value: "Pâtes fraîches");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 15,
                column: "nom_activite",
                value: "Pâtes molles");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 16,
                column: "nom_activite",
                value: "Pâtes pressées");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 17,
                column: "nom_activite",
                value: "Pâtes cuites");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 18,
                column: "nom_activite",
                value: "Autres pâtes");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 19,
                column: "nom_activite",
                value: "Nanofiltration");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 20,
                column: "nom_activite",
                value: "Autres filtrations");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 21,
                column: "nom_activite",
                value: "Evaporation");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 22,
                column: "nom_activite",
                value: "Séchage");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 23,
                column: "nom_activite",
                value: "Processed Cheese");

            migrationBuilder.InsertData(
                table: "activite_pfl",
                columns: new[] { "id", "nom_activite", "type_documents" },
                values: new object[] { 24, "Autres", "PC,M,E,W,A,R" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 24);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "6276df84-2589-4cfb-a2e9-eaba759c0946");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a6397e48-6ea2-4d6b-b8b1-3642640cc4e2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "b1a8c7dc-f43e-44bb-b89c-2eb01677c0d5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "39896873-7917-437d-8129-b2417a73a092");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "8307feff-8c15-4b25-872f-339435de5047");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "nom_activite", "type_documents" },
                values: new object[] { "Matières premières", "PC,M,E,W" });

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 2,
                column: "nom_activite",
                value: "Ingrédients");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 3,
                column: "nom_activite",
                value: "Matière première 1");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 4,
                column: "nom_activite",
                value: "Matière première 2");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 5,
                column: "nom_activite",
                value: "Matière première 3");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 6,
                column: "nom_activite",
                value: "Stockage");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 7,
                column: "nom_activite",
                value: "Microfiltration");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 8,
                column: "nom_activite",
                value: "Traitement thermique");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 9,
                column: "nom_activite",
                value: "Ecrémage");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 10,
                column: "nom_activite",
                value: "Homogénéisation");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 11,
                column: "nom_activite",
                value: "Ultrafiltration");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 12,
                column: "nom_activite",
                value: "Maturation/stockage");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "nom_activite", "type_documents" },
                values: new object[] { "Pâtes fraîches", "PC,M,E,W,A,R" });

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 14,
                column: "nom_activite",
                value: "Pâtes molles");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 15,
                column: "nom_activite",
                value: "Pâtes pressées");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 16,
                column: "nom_activite",
                value: "Pâtes cuites");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 17,
                column: "nom_activite",
                value: "Autres pâtes");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 18,
                column: "nom_activite",
                value: "Nanofiltration");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 19,
                column: "nom_activite",
                value: "Autres filtrations");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 20,
                column: "nom_activite",
                value: "Evaporation");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 21,
                column: "nom_activite",
                value: "Séchage");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 22,
                column: "nom_activite",
                value: "Processed Cheese");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 23,
                column: "nom_activite",
                value: "Autres");
        }
    }
}
