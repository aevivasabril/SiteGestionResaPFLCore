using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class MajTableEquipementTablesPcVue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "1beca1ef-7fe7-46cd-b68f-b8fce8364fa6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a97d4458-3e0d-4f26-9f56-7f918f73016b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "f952f25a-8044-43f5-8da1-6bd69f6e313b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "6699a482-e596-476b-881b-6d1d6e6f5865");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 182,
                column: "nomTabPcVue",
                value: "tab_UA_ECR");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 188,
                column: "nomTabPcVue",
                value: "tab_UA_MAT");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 200,
                column: "nomTabPcVue",
                value: "tab_UA_CUV");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 211,
                column: "nomTabPcVue",
                value: "tab_UA_NEP");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 213,
                column: "nomTabPcVue",
                value: "tab_UA_SPI");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 218,
                column: "nomTabPcVue",
                value: "tab_UA_MFMG");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 220,
                column: "nomTabPcVue",
                value: "tab_UA_GP7");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 222,
                column: "nomTabPcVue",
                value: "tab_UA_ACT");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 223,
                column: "nomTabPcVue",
                value: "tab_UA_MTH");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 225,
                column: "nomTabPcVue",
                value: "tab_UA_EVAA, tab_UA_EVAB");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 228,
                column: "nomTabPcVue",
                value: "tab_UA_SEC");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 229,
                column: "nomTabPcVue",
                value: "tab_UA_VALO");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 230,
                column: "nomTabPcVue",
                value: "tab_UA_OPTIMAL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c6da6c5a-1399-427c-93f7-ce68967c48ce");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e2467fa6-4c3e-4cb4-ae45-2cf161638300");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "751cd1fe-1a39-46cd-986f-143145f2792c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "7d2abe5c-a99f-4b15-8d09-5055e1a4b4e3");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 182,
                column: "nomTabPcVue",
                value: null);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 188,
                column: "nomTabPcVue",
                value: null);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 200,
                column: "nomTabPcVue",
                value: null);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 211,
                column: "nomTabPcVue",
                value: null);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 213,
                column: "nomTabPcVue",
                value: null);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 218,
                column: "nomTabPcVue",
                value: null);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 220,
                column: "nomTabPcVue",
                value: null);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 222,
                column: "nomTabPcVue",
                value: null);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 223,
                column: "nomTabPcVue",
                value: null);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 225,
                column: "nomTabPcVue",
                value: null);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 228,
                column: "nomTabPcVue",
                value: null);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 229,
                column: "nomTabPcVue",
                value: null);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 230,
                column: "nomTabPcVue",
                value: null);
        }
    }
}
