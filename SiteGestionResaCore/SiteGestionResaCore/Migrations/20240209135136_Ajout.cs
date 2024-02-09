using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class Ajout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "db906827-d001-40aa-83e9-4d8155552760");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "ea5f133c-37a7-4011-9ba4-951acd7bc1a9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "3f31a42c-e21c-402f-8cba-f6e6ec418130");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "704f814b-6256-41dd-bc65-4be1675c2725");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "1c576c3d-b704-4e26-8eb1-e58ca84435cc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "681872b3-06f6-444b-aebd-2149303f07cc");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 165,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Balance OHAUS 2 Kg (SALLE AP9)", 17 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 203,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Thermoscelleuse ERECAM semi-automatique dia:68/95/116 (SALLE AP6)", 17 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 245,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Balance OHAUS Ranger 3000 -30Kg (SALLE AP9)", 17 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 248,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Thermomix TM5 (SALLE AP5)", 17 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 260,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Balance OHAUS Ranger 3000 -30Kg (SALLE AP5)", 17 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 267,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Thermomix TM6 (SALLE AP9)", 17 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 269,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Bain-marie sans couvercle (SALLE AP6)", 17 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 270,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Bain-marie MEMMERT (avec couvercle, petite capacité)(SALLE AP9)", 17 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 271,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Bain-marie MEMMERT (avec couvercle, grande capacité) (SALLE AP8)", 17 });

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "equip_delete", "mobile", "nom", "nomTabPcVue", "numGmao", "type_activites", "zoneID" },
                values: new object[] { 272, null, true, "Agitateur BIOBLOCK type RZR 2000 Digital (SALLE AP8)", null, "AGIT0186", "17", 17 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 272);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "4e03bf23-9ec1-4183-8ae7-f0d712014fbe");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "d6cce723-0af8-44a0-9cc3-12e58a98d3cf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "4fa101b7-613c-4da0-9683-2ef252e3d544");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "fac24db9-c976-465e-83fe-3cf1d4873cc8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "79b6a585-8aff-4d61-a42f-fe5efa01299b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "6951d63c-9b6b-42fb-a88a-aa7cde9d5235");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 165,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Balance OHAUS 2 Kg (Scout Pro SPU2001)", 16 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 203,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Thermoscelleuse ERECAM semi-automatique dia:68/95/116", 12 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 245,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Balance OHAUS Ranger 3000 -30Kg", 16 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 248,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Thermomix TM5", 12 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 260,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Balance OHAUS Ranger 3000 -30Kg", 12 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 267,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Thermomix TM6", 16 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 269,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Bain-marie sans couvercle", 13 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 270,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Bain-marie MEMMERT (avec couvercle, petite capacité)", 16 });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 271,
                columns: new[] { "nom", "zoneID" },
                values: new object[] { "Bain-marie MEMMERT (avec couvercle, grande capacité)", 15 });
        }
    }
}
