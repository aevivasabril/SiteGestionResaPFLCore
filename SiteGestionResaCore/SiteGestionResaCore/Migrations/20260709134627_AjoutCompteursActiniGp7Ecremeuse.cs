using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutCompteursActiniGp7Ecremeuse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "1fd285e6-5182-4b27-a723-b86a39b660a0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1bac3e72-90a7-44f9-b8c4-cbc3c0a3febf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "8e85f4c0-c4ce-4599-bc4f-99fdec1bbc97");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "91a66cc8-47c7-405b-9176-57e880734e9c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "507393aa-7036-47db-a6f2-3ebade5f3fd8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "068b93e8-04b3-4067-88b1-14e1900bca26");

            migrationBuilder.InsertData(
                table: "compteurs_energies",
                columns: new[] { "id", "equipementID", "nomTabPcVue", "nom_compteur" },
                values: new object[,]
                {
                    { 5, 222, "tab_COMPT_ACTINI", "Compteur electrique actini" },
                    { 6, 182, "tab_COMPT_ECREMEUSE", "Compteur electrique ecremeuse" },
                    { 7, 220, "tab_COMPT_GP7", "Compteur electrique GP7" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "compteurs_energies",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "compteurs_energies",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "compteurs_energies",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "8bfd7de4-3c4d-4ab9-b634-0c08dc521bf5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a1d3f310-c290-4eea-87ca-37a5329cdd9b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "2cc463db-4ba8-4088-874c-af895836604d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "1e4fd97e-b1f6-4446-9e7f-905433786391");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "c626f160-caa1-44b7-9d13-ed7cab6a88fa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "932877f0-c392-4b23-8c88-e96fa62973f9");
        }
    }
}
