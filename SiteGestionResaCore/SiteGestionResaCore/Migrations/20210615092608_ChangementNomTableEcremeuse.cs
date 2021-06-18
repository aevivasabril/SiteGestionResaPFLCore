using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class ChangementNomTableEcremeuse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "equipement",
                keyColumn: "id",
                keyValue: 182,
                column: "nomTabPcVue",
                value: "tab_UA_ECREM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ac058ed5-641d-4cf2-aefd-36c799692736");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "12ab29ee-00b3-41a7-a813-506611a95c1f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "b6f045b9-4403-4652-837f-23306763e2fa");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "ade00e0e-981e-4cac-9424-6fe54929408e");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 182,
                column: "nomTabPcVue",
                value: "tab_UA_ECR");
        }
    }
}
