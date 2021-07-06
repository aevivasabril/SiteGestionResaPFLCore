using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutTabletteDELL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ea6e02f1-b402-4f46-b04b-17bb5626983e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "f4948632-bd75-4e6c-97d0-1bef7fd49a86");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "0656e7e3-6c17-4def-b6ed-c6d687a10c1d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "16d66449-2869-4983-9a30-2bcd2878532b");

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "mobile", "nom", "nomTabPcVue", "numGmao", "zoneID" },
                values: new object[] { 248, true, "Tablette Latitude 7212 Dell", null, "", 17 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 248);

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
        }
    }
}
