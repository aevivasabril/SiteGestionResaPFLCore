using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class ChangementTableEquipement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_equipement_activite_pfl",
                table: "equipement");

            migrationBuilder.DropIndex(
                name: "IX_equipement_activiteID",
                table: "equipement");

            migrationBuilder.DeleteData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 24);

            migrationBuilder.DropColumn(
                name: "activiteID",
                table: "equipement");

            migrationBuilder.AddColumn<string>(
                name: "type_activites",
                table: "equipement",
                unicode: false,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "6b9fbc40-2047-4c29-b2ec-8368e4728b6e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "ce38b4e7-b934-42b6-84c3-329ba9fe5bb1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "a5917fd7-b27f-40cb-819e-048b81f8de94");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "c1a4c58a-9fd6-4957-9581-9bd2cb9fb765");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "8c652731-8d72-40bb-b4b4-70d514b0b23e");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 8,
                column: "nom_activite",
                value: "Séparation membranaire");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 12,
                column: "nom_activite",
                value: "Maturation/stockage");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "nom_activite", "type_documents" },
                values: new object[] { "Fromage", "PC,M,E,W,A,R" });

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 14,
                column: "nom_activite",
                value: "Produits Frais");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 15,
                column: "nom_activite",
                value: "Autres produits");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 16,
                column: "nom_activite",
                value: "Evaporation");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 17,
                column: "nom_activite",
                value: "Séchage");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 18,
                column: "nom_activite",
                value: "Autres procèdes");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 162,
                column: "type_activites",
                value: "2,4,5,6,8,9,10,11,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 163,
                column: "type_activites",
                value: "2,3,4,5,6,8,9,10,11,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 164,
                column: "type_activites",
                value: "2,4,5,6,8,9,10,11,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 165,
                column: "type_activites",
                value: "2,3,4,5,6,8,9,10,11,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 166,
                column: "type_activites",
                value: "2,4,5,6,8,9,10,11,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 167,
                column: "type_activites",
                value: "2,3,4,5,6,8,9,10,11,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 169,
                column: "type_activites",
                value: "13");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 170,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 171,
                column: "type_activites",
                value: "2,4,5,6,8,9,10,13,14,15,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 172,
                column: "type_activites",
                value: "2,4,5,6,8,9,10,13,14,15,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 173,
                column: "type_activites",
                value: "2,4,5,6,8,9,10,13,14,15,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 174,
                column: "type_activites",
                value: "2,4,5,6,8,9,10,13,14,15,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 175,
                column: "type_activites",
                value: "2,4,5,6,8,9,10,13,14,15,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 176,
                column: "type_activites",
                value: "2,4,5,6,8,9,10,13,14,15,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 177,
                column: "type_activites",
                value: "2,4,5,6,8,9,10,13,14,15,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 178,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 179,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 180,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 181,
                column: "type_activites",
                value: "10");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 182,
                column: "type_activites",
                value: "10");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 183,
                column: "type_activites",
                value: "2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 184,
                column: "type_activites",
                value: "2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 185,
                column: "type_activites",
                value: "2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 186,
                column: "type_activites",
                value: "2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 187,
                column: "type_activites",
                value: "2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 188,
                column: "type_activites",
                value: "2,4,5,6,7,13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 189,
                column: "type_activites",
                value: "2,4,5,6,7,8,12");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 190,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 191,
                column: "type_activites",
                value: "13,14");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 192,
                column: "type_activites",
                value: "13,14");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 193,
                column: "type_activites",
                value: "13,14");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 194,
                column: "type_activites",
                value: "2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 195,
                column: "type_activites",
                value: "2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 196,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 197,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 198,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 199,
                column: "type_activites",
                value: "2,4,5,6,7,8,12");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 200,
                column: "type_activites",
                value: "2,4,5,6,7");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 201,
                column: "type_activites",
                value: "2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 202,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 203,
                column: "type_activites",
                value: "14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 204,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 205,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 206,
                column: "type_activites",
                value: "11");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 207,
                column: "type_activites",
                value: "11");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 208,
                column: "type_activites",
                value: "11");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 209,
                column: "type_activites",
                value: "13");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 210,
                column: "type_activites",
                value: "13");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 211,
                column: "type_activites",
                value: "2,4,5,6,7,9,10,12");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 212,
                column: "type_activites",
                value: "13");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 213,
                column: "type_activites",
                value: "8");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 214,
                column: "type_activites",
                value: "8");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 215,
                column: "type_activites",
                value: "8");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 216,
                column: "type_activites",
                value: "8");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 217,
                column: "type_activites",
                value: "8");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 218,
                column: "type_activites",
                value: "8");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 219,
                column: "type_activites",
                value: "8");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 220,
                column: "type_activites",
                value: "8");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 221,
                column: "type_activites",
                value: "8");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 222,
                column: "type_activites",
                value: "9");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 223,
                column: "type_activites",
                value: "9");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 224,
                column: "type_activites",
                value: "8");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 225,
                column: "type_activites",
                value: "16");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 226,
                column: "type_activites",
                value: "8");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 227,
                column: "type_activites",
                value: "17");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 228,
                column: "type_activites",
                value: "17");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 229,
                column: "type_activites",
                value: "8");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 230,
                column: "type_activites",
                value: "8");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 231,
                column: "type_activites",
                value: "18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 232,
                column: "type_activites",
                value: "18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 233,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 234,
                column: "type_activites",
                value: "18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 235,
                column: "type_activites",
                value: "13");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 236,
                column: "type_activites",
                value: "13");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 237,
                column: "type_activites",
                value: "13");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 238,
                column: "type_activites",
                value: "10");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 239,
                column: "type_activites",
                value: "2,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 240,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 241,
                column: "type_activites",
                value: "13,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 242,
                column: "type_activites",
                value: "14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 243,
                column: "type_activites",
                value: "11,13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 244,
                column: "type_activites",
                value: "16,17");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 245,
                column: "type_activites",
                value: "2,3,4,5,6,8,9,10,11,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 246,
                column: "type_activites",
                value: "2,3,4,5,6,8,9,10,11,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 247,
                column: "type_activites",
                value: "18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 248,
                column: "type_activites",
                value: "9,13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 250,
                column: "type_activites",
                value: "15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 251,
                column: "type_activites",
                value: "15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 252,
                column: "type_activites",
                value: "15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 253,
                column: "type_activites",
                value: "15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 254,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 255,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 256,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 257,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 258,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 259,
                column: "type_activites",
                value: "7");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 260,
                column: "type_activites",
                value: "2,3,4,5,6,8,9,10,11,13,14,15,16,17,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 261,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 262,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 263,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 264,
                column: "type_activites",
                value: "15,18");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 265,
                column: "type_activites",
                value: "13,14,15");

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 266,
                column: "type_activites",
                value: "13,14,15");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type_activites",
                table: "equipement");

            migrationBuilder.AddColumn<int>(
                name: "activiteID",
                table: "equipement",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "1f021397-4676-4ba4-bdae-197b6e88cc08");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "37cc3326-b4ec-4b32-bcab-664354a0dc37");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "daa9de19-a736-4bd0-89c0-ce78e3ed1dd3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "5e8e1567-3bd2-4ab0-b852-09051253b4dc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "f2309fb9-f92b-46e8-8f02-819e69ce4f57");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 8,
                column: "nom_activite",
                value: "Microfiltration");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 12,
                column: "nom_activite",
                value: "Ultrafiltration");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "nom_activite", "type_documents" },
                values: new object[] { "Maturation/stockage", "PC,M,E,W" });

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 14,
                column: "nom_activite",
                value: "Pâtes fraîches");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 15,
                column: "nom_activite",
                value: "Pâtes molles");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 16,
                column: "nom_activite",
                value: "Pâtes pressées");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 17,
                column: "nom_activite",
                value: "Pâtes cuites");

            migrationBuilder.UpdateData(
                table: "activite_pfl",
                keyColumn: "id",
                keyValue: 18,
                column: "nom_activite",
                value: "Autres pâtes");

            migrationBuilder.InsertData(
                table: "activite_pfl",
                columns: new[] { "id", "nom_activite", "type_documents" },
                values: new object[,]
                {
                    { 19, "Processed Cheese", "PC,M,E,W,A,R" },
                    { 23, "Séchage", "PC,M,E,W,A,R" },
                    { 22, "Evaporation", "PC,M,E,W,A,R" },
                    { 21, "Autres filtrations", "PC,M,E,W,A,R" },
                    { 20, "Nanofiltration", "PC,M,E,W,A,R" },
                    { 24, "Autres", "PC,M,E,W,A,R" }
                });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 162,
                column: "activiteID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 163,
                column: "activiteID",
                value: 3);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 164,
                column: "activiteID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 165,
                column: "activiteID",
                value: 3);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 166,
                column: "activiteID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 167,
                column: "activiteID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 169,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 170,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 179,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 180,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 181,
                column: "activiteID",
                value: 10);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 182,
                column: "activiteID",
                value: 10);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 183,
                column: "activiteID",
                value: 7);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 184,
                column: "activiteID",
                value: 7);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 185,
                column: "activiteID",
                value: 7);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 186,
                column: "activiteID",
                value: 7);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 187,
                column: "activiteID",
                value: 7);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 188,
                column: "activiteID",
                value: 13);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 189,
                column: "activiteID",
                value: 13);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 190,
                column: "activiteID",
                value: 16);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 191,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 192,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 193,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 194,
                column: "activiteID",
                value: 7);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 195,
                column: "activiteID",
                value: 7);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 196,
                column: "activiteID",
                value: 16);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 197,
                column: "activiteID",
                value: 16);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 198,
                column: "activiteID",
                value: 16);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 199,
                column: "activiteID",
                value: 7);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 200,
                column: "activiteID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 201,
                column: "activiteID",
                value: 7);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 202,
                column: "activiteID",
                value: 16);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 203,
                column: "activiteID",
                value: 16);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 204,
                column: "activiteID",
                value: 18);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 206,
                column: "activiteID",
                value: 11);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 207,
                column: "activiteID",
                value: 11);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 208,
                column: "activiteID",
                value: 11);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 209,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 210,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 211,
                column: "activiteID",
                value: 13);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 212,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 222,
                column: "activiteID",
                value: 9);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 223,
                column: "activiteID",
                value: 9);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 233,
                column: "activiteID",
                value: 3);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 235,
                column: "activiteID",
                value: 16);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 236,
                column: "activiteID",
                value: 16);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 237,
                column: "activiteID",
                value: 16);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 238,
                column: "activiteID",
                value: 10);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 240,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 241,
                column: "activiteID",
                value: 16);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 243,
                column: "activiteID",
                value: 9);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 244,
                column: "activiteID",
                value: 3);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 245,
                column: "activiteID",
                value: 3);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 246,
                column: "activiteID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 248,
                column: "activiteID",
                value: 9);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 254,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 255,
                column: "activiteID",
                value: 9);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 256,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 257,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 258,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 259,
                column: "activiteID",
                value: 7);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 260,
                column: "activiteID",
                value: 3);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 261,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 262,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 263,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 265,
                column: "activiteID",
                value: 16);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 171,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 172,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 173,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 174,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 175,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 176,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 177,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 178,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 205,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 213,
                column: "activiteID",
                value: 21);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 214,
                column: "activiteID",
                value: 21);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 215,
                column: "activiteID",
                value: 21);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 216,
                column: "activiteID",
                value: 20);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 217,
                column: "activiteID",
                value: 21);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 218,
                column: "activiteID",
                value: 21);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 219,
                column: "activiteID",
                value: 21);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 220,
                column: "activiteID",
                value: 21);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 221,
                column: "activiteID",
                value: 21);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 224,
                column: "activiteID",
                value: 21);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 225,
                column: "activiteID",
                value: 22);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 226,
                column: "activiteID",
                value: 21);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 227,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 228,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 229,
                column: "activiteID",
                value: 21);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 230,
                column: "activiteID",
                value: 21);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 231,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 232,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 234,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 239,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 242,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 247,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 250,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 251,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 252,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 253,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 264,
                column: "activiteID",
                value: 24);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 266,
                column: "activiteID",
                value: 23);

            migrationBuilder.CreateIndex(
                name: "IX_equipement_activiteID",
                table: "equipement",
                column: "activiteID");

            migrationBuilder.AddForeignKey(
                name: "FK_equipement_activite_pfl",
                table: "equipement",
                column: "activiteID",
                principalTable: "activite_pfl",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
