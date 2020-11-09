using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "026c4c89-61ed-41e9-8aea-3043b7436498", "Admin", "ADMIN" },
                    { 2, "5fd92704-2a91-4925-9334-40c6594fab81", "Utilisateur", "UTILISATEUR" },
                    { 3, "cfa2e30f-3f42-4203-b7ec-01a9d2ad08bd", "MainAdmin", "MAINADMIN" },
                    { 4, "63c57bd8-829c-4eef-82d6-31f7afedc894", "Logistic", "LOGISTIC" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
