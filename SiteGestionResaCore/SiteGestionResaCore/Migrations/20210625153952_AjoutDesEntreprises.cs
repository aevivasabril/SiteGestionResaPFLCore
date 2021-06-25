using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutDesEntreprises : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "00df128a-11da-438c-a7a0-a27283035bf5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "d2ed9415-870e-4e80-a1e6-b809e0a1a222");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "d1b45a53-0740-4b55-94cb-52d1d0eb8b07");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "2ba09a7f-f97e-4ab6-bcf2-f3f27a42db76");

            migrationBuilder.UpdateData(
                table: "organisme",
                keyColumn: "id",
                keyValue: 3,
                column: "nom_organisme",
                value: "Sill");

            migrationBuilder.InsertData(
                table: "organisme",
                columns: new[] { "id", "nom_organisme" },
                values: new object[,]
                {
                    { 6, "Sodial" },
                    { 7, "Isigny sainte mère" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "organisme",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "organisme",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b4da822e-9a0c-4902-9fc0-8790fddc70f8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "858b24a5-0ff7-4454-980c-d1f3f6267531");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "9b356844-db66-4f14-bfa0-3bb3c43fb305");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "4e06d016-a571-460c-8ec6-185d4886604c");

            migrationBuilder.UpdateData(
                table: "organisme",
                keyColumn: "id",
                keyValue: 3,
                column: "nom_organisme",
                value: "Quescrem");
        }
    }
}
