using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class SuppressionEquipesISFPSM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ld_equipes_stlo",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ld_equipes_stlo",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c527e101-7ce2-467c-bb7a-0d638d82f270");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "618e284e-fae7-4371-b32a-4f578823afa9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "6e2329be-87bf-4155-8967-9a84d6b847c2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "79add88e-243c-4d41-a353-ae14c609a67a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "c91a235a-b0e4-4d39-91e8-54961c985145");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "7e496a9f-3f06-4af7-9ee6-d5bc98ffb7ab");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "3de771b1-027b-4b02-847e-3069b8f12c1b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3c8d8f22-1386-42c1-be1b-b66ff582ba17");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "88559142-d8e4-450a-bb94-db2c816d55e6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "3df95918-5219-460c-a6a1-2e62071358bb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "c6e5f317-0ba4-417c-aab4-de27cbf8a20b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "9ebe2bc7-47c0-4a8a-8398-2f314826f7b2");

            migrationBuilder.InsertData(
                table: "ld_equipes_stlo",
                columns: new[] { "id", "nom_equipe" },
                values: new object[,]
                {
                    { 3, "PSM" },
                    { 4, "ISF" }
                });
        }
    }
}
