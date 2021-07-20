using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutModifDesSallesAlimentaires : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b7312677-b749-406e-ae29-3a3def80f7b6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "bd4a7bcc-4e12-4c9a-8a7c-9bf0c7c3e0ba");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "cfe04865-b29d-4484-bc14-0b3c8a17a1b8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "72103ce0-f2f5-49c7-87c9-93aebf2bd142");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 204,
                column: "zoneID",
                value: 9);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 208,
                column: "zoneID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 244,
                columns: new[] { "nom", "numGmao", "zoneID" },
                values: new object[] { "Balance OHAUS Ranger 3000 -30Kg- tour de sechage", "BAL0068", 1 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 245,
                columns: new[] { "nom", "numGmao", "zoneID" },
                values: new object[] { "Balance OHAUS Ranger 3000 -30Kg", "BAL0074", 16 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 246,
                columns: new[] { "nom", "numGmao", "zoneID" },
                values: new object[] { "Balance PRECIA MOLEN 150 kg", "BAL0073", 7 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 247,
                columns: new[] { "nom", "numGmao", "zoneID" },
                values: new object[] { "Tablette Latitude 7212 Dell", "", 17 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 248,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Thermomix", 12 });

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "mobile", "nom", "nomTabPcVue", "numGmao", "zoneID" },
                values: new object[,]
                {
                    { 253, false, "Salle AP9", null, "CHF014", 16 },
                    { 252, false, "Salle AP8", null, "CHF012", 15 },
                    { 251, false, "Salle AP6", null, "CHF013", 13 },
                    { 250, false, "Salle AP5", null, "CHF011", 12 },
                    { 254, false, "Bac de saumurage 800 lts", null, "", 10 },
                    { 249, false, "Congelateur", null, "CONG0013", 16 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 249);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 250);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 252);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 253);

            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 254);

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

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 204,
                column: "zoneID",
                value: 12);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 208,
                column: "zoneID",
                value: 13);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 244,
                columns: new[] { "nom", "numGmao", "zoneID" },
                values: new object[] { "Thermocook", "", 12 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 245,
                columns: new[] { "nom", "numGmao", "zoneID" },
                values: new object[] { "Balance OHAUS Ranger 3000 -30Kg- tour de sechage", "BAL0068", 1 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 246,
                columns: new[] { "nom", "numGmao", "zoneID" },
                values: new object[] { "Balance OHAUS Ranger 3000 -30Kg", "BAL0074", 12 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 247,
                columns: new[] { "nom", "numGmao", "zoneID" },
                values: new object[] { "Balance PRECIA MOLEN 150 kg", "BAL0073", 7 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 248,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Tablette Latitude 7212 Dell", 17 });
        }
    }
}
