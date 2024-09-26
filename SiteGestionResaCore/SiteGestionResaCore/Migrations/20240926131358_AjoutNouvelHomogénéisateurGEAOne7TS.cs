using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutNouvelHomogénéisateurGEAOne7TS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e0926106-b845-4f7c-8237-045b8af5a39b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e69bc742-b836-459f-82c5-d27725416031");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "b6a94ce4-4342-46d6-af77-fdf3d507862c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "3af42aaa-64c6-4b56-8651-ab82f9cb4771");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "996524a6-196f-4a77-9e68-3d80c64ba749");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "2c924600-357b-4135-8846-cae220fb50b9");

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "equip_delete", "mobile", "nom", "nomTabPcVue", "numGmao", "type_activites", "zoneID" },
                values: new object[] { 273, null, false, "Homogénéisateur GEA One 7TS ", null, "HOMO0009", "11", 3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 273);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ffd7539d-02aa-4fc4-93fd-6740c4898aa6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "d5d93965-5410-45eb-b756-d047206157bb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "b3581acd-ed67-4c38-8296-41daf1fb323c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "cbe30f72-e999-4f79-95eb-a65ea61d189b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "d25bf70c-0195-4402-9530-3720367821b4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "01792748-5f1d-4727-826c-8f6f909c2969");
        }
    }
}
