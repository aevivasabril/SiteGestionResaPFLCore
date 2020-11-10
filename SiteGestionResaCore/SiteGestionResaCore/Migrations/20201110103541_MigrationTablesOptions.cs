using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class MigrationTablesOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "bbb72bf9-3e52-422c-bcbd-abfafd505f81");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "7adf2ea6-e13c-4e61-a682-54b36d657016");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "58f16c3d-deb7-42e7-85f9-7d1fb6ae634f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "def9dcf4-8a0b-4c05-94c4-471d303b87f5");

            migrationBuilder.InsertData(
                table: "ld_destination",
                columns: new[] { "id", "nom_destination" },
                values: new object[,]
                {
                    { 1, "Non connu (sans dégustation)" },
                    { 2, "Plan HACCP" },
                    { 3, "Test sensoriel" }
                });

            migrationBuilder.InsertData(
                table: "ld_financement",
                columns: new[] { "id", "nom_financement" },
                values: new object[,]
                {
                    { 3, "STLO" },
                    { 1, "Public" },
                    { 2, "Privé" }
                });

            migrationBuilder.InsertData(
                table: "ld_produit_in",
                columns: new[] { "id", "nom_produit_in" },
                values: new object[,]
                {
                    { 1, "Autre" },
                    { 2, "Lait" },
                    { 3, "Lactoserum" },
                    { 4, "Babeurre" }
                });

            migrationBuilder.InsertData(
                table: "ld_provenance",
                columns: new[] { "id", "nom_provenance" },
                values: new object[,]
                {
                    { 1, "Régional" },
                    { 2, "National" },
                    { 3, "International" },
                    { 4, "Européen" }
                });

            migrationBuilder.InsertData(
                table: "ld_provenance_produit",
                columns: new[] { "id", "nom_provenance_produit" },
                values: new object[,]
                {
                    { 8, "Earl Lemarchand" },
                    { 7, "Earl Lorret" },
                    { 4, "St Malo" },
                    { 5, "Montauban" },
                    { 3, "Retiers" },
                    { 2, "Non connu" },
                    { 6, "Noyal" },
                    { 1, "Autre" }
                });

            migrationBuilder.InsertData(
                table: "ld_type_projet",
                columns: new[] { "id", "nom_type_projet" },
                values: new object[,]
                {
                    { 1, "Non connu" },
                    { 2, "Recherche" },
                    { 3, "Formation/Stage" },
                    { 4, "Industriel (cellules hébergés" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ld_destination",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ld_destination",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ld_destination",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ld_financement",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ld_financement",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ld_financement",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ld_produit_in",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ld_produit_in",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ld_produit_in",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ld_produit_in",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ld_provenance",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ld_provenance",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ld_provenance",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ld_provenance",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ld_provenance_produit",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ld_provenance_produit",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ld_provenance_produit",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ld_provenance_produit",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ld_provenance_produit",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ld_provenance_produit",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ld_provenance_produit",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ld_provenance_produit",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ld_type_projet",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ld_type_projet",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ld_type_projet",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ld_type_projet",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "644dbaad-602b-4e3d-ae41-9dfecc0f0101");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "04346adc-257e-4e07-a001-48123fd3e466");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "58b590d4-929f-4f3a-a0ae-42fa789980e0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "b1deddb2-d31f-42c9-bb3a-dd0dec18d467");
        }
    }
}
