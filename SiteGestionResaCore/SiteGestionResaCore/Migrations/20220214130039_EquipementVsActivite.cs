using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class EquipementVsActivite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "list_activites");

            migrationBuilder.AddColumn<int>(
                name: "activiteID",
                table: "equipement",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "activite_pfl",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_activite = table.Column<string>(unicode: false, nullable: false),
                    type_documents = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activite_pfl", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "202a77ab-7e4c-4806-b0dd-1652074c5ec4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "f4ebf951-efc7-4140-9ddd-875c44032585");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "a8b1bbf1-2a95-417a-b3be-3a96adabd3e9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "f4c0ca64-38b1-4fb2-acf1-9d40acdd7876");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "9e50ef3d-630a-4db0-b81c-d57a689f016e");

            migrationBuilder.InsertData(
                table: "activite_pfl",
                columns: new[] { "id", "nom_activite", "type_documents" },
                values: new object[,]
                {
                    { 23, "Autres", "PC,M,E,W,A,R" },
                    { 22, "Processed Cheese", "PC,M,E,W,A,R" },
                    { 21, "Séchage", "PC,M,E,W,A,R" },
                    { 20, "Evaporation", "PC,M,E,W,A,R" },
                    { 19, "Autres filtrations", "PC,M,E,W,A,R" },
                    { 18, "Nanofiltration", "PC,M,E,W,A,R" },
                    { 17, "Autres pâtes", "PC,M,E,W,A,R" },
                    { 16, "Pâtes cuites", "PC,M,E,W,A,R" },
                    { 15, "Pâtes pressées", "PC,M,E,W,A,R" },
                    { 14, "Pâtes molles", "PC,M,E,W,A,R" },
                    { 12, "Maturation/stockage", "PC,M,E,W" },
                    { 11, "Ultrafiltration", "PC,M,E,W" },
                    { 10, "Homogénéisation", "PC,M,E,W" },
                    { 9, "Ecrémage", "PC,M,E,W" },
                    { 8, "Traitement thermique", "PC,M,E,W" },
                    { 7, "Microfiltration", "PC,M,E,W" },
                    { 6, "Stockage", "PC,M,E,W" },
                    { 5, "Matière première 3", "PC,M,E,W" },
                    { 4, "Matière première 2", "PC,M,E,W" },
                    { 3, "Matière première 1", "PC,M,E,W" },
                    { 2, "Ingrédients", "PC,M,E,W" },
                    { 13, "Pâtes fraîches", "PC,M,E,W,A,R" },
                    { 1, "Matières premières", "PC,M,E,W" }
                });

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 162,
                column: "activiteID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 163,
                column: "activiteID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 164,
                column: "activiteID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 165,
                column: "activiteID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 166,
                column: "activiteID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 167,
                column: "activiteID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 169,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 170,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 171,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 172,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 173,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 174,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 175,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 176,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 177,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 178,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 179,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 180,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 181,
                column: "activiteID",
                value: 9);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 182,
                column: "activiteID",
                value: 9);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 183,
                column: "activiteID",
                value: 6);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 184,
                column: "activiteID",
                value: 6);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 185,
                column: "activiteID",
                value: 6);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 186,
                column: "activiteID",
                value: 6);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 187,
                column: "activiteID",
                value: 6);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 188,
                column: "activiteID",
                value: 12);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 189,
                column: "activiteID",
                value: 12);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 190,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 191,
                column: "activiteID",
                value: 13);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 192,
                column: "activiteID",
                value: 13);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 193,
                column: "activiteID",
                value: 13);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 194,
                column: "activiteID",
                value: 6);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 195,
                column: "activiteID",
                value: 6);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 196,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 197,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 198,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 199,
                column: "activiteID",
                value: 6);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 200,
                column: "activiteID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 201,
                column: "activiteID",
                value: 6);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 202,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 203,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 204,
                column: "activiteID",
                value: 17);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 205,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 206,
                column: "activiteID",
                value: 10);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 207,
                column: "activiteID",
                value: 10);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 208,
                column: "activiteID",
                value: 10);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 209,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 210,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 211,
                column: "activiteID",
                value: 12);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 212,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 213,
                column: "activiteID",
                value: 19);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 214,
                column: "activiteID",
                value: 19);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 215,
                column: "activiteID",
                value: 19);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 216,
                column: "activiteID",
                value: 18);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 217,
                column: "activiteID",
                value: 19);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 218,
                column: "activiteID",
                value: 19);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 219,
                column: "activiteID",
                value: 19);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 220,
                column: "activiteID",
                value: 19);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 221,
                column: "activiteID",
                value: 19);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 222,
                column: "activiteID",
                value: 8);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 223,
                column: "activiteID",
                value: 8);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 224,
                column: "activiteID",
                value: 19);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 225,
                column: "activiteID",
                value: 20);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 226,
                column: "activiteID",
                value: 19);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 227,
                column: "activiteID",
                value: 21);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 228,
                column: "activiteID",
                value: 21);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 229,
                column: "activiteID",
                value: 19);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 230,
                column: "activiteID",
                value: 19);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 231,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 232,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 233,
                column: "activiteID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 234,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 235,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 236,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 237,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 238,
                column: "activiteID",
                value: 9);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 239,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 240,
                column: "activiteID",
                value: 13);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 241,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 242,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 243,
                column: "activiteID",
                value: 8);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 244,
                column: "activiteID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 245,
                column: "activiteID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 246,
                column: "activiteID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 247,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 248,
                column: "activiteID",
                value: 8);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 250,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 251,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 252,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 253,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 254,
                column: "activiteID",
                value: 13);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 255,
                column: "activiteID",
                value: 8);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 256,
                column: "activiteID",
                value: 13);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 257,
                column: "activiteID",
                value: 13);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 258,
                column: "activiteID",
                value: 13);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 259,
                column: "activiteID",
                value: 6);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 260,
                column: "activiteID",
                value: 2);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 261,
                column: "activiteID",
                value: 13);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 262,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 263,
                column: "activiteID",
                value: 14);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 264,
                column: "activiteID",
                value: 23);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 265,
                column: "activiteID",
                value: 15);

            migrationBuilder.UpdateData(
                table: "equipement",
                keyColumn: "id",
                keyValue: 266,
                column: "activiteID",
                value: 22);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_equipement_activite_pfl",
                table: "equipement");

            migrationBuilder.DropTable(
                name: "activite_pfl");

            migrationBuilder.DropIndex(
                name: "IX_equipement_activiteID",
                table: "equipement");

            migrationBuilder.DropColumn(
                name: "activiteID",
                table: "equipement");

            migrationBuilder.CreateTable(
                name: "list_activites",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_activite = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    type_documents = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_list_activites", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "66187d40-d039-4085-a4c6-6bb68d75b587");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3fb19b1b-8d7d-4c46-9c00-782b823c8e02");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "6fced70f-6d06-4b7a-9f4c-1f541213d9e5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "6e697aee-4bf4-40e8-9a87-3716e59d6a86");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "68f99e50-9e80-47de-b3a1-37e84408e1b0");

            migrationBuilder.InsertData(
                table: "list_activites",
                columns: new[] { "id", "nom_activite", "type_documents" },
                values: new object[,]
                {
                    { 23, "Autres", "PC,M,E,W,A,R" },
                    { 22, "Processed Cheese", "PC,M,E,W,A,R" },
                    { 21, "Séchage", "PC,M,E,W,A,R" },
                    { 20, "Evaporation", "PC,M,E,W,A,R" },
                    { 19, "Autres filtrations", "PC,M,E,W,A,R" },
                    { 18, "Nanofiltration", "PC,M,E,W,A,R" },
                    { 17, "Autres pâtes", "PC,M,E,W,A,R" },
                    { 16, "Pâtes cuites", "PC,M,E,W,A,R" },
                    { 15, "Pâtes pressées", "PC,M,E,W,A,R" },
                    { 14, "Pâtes molles", "PC,M,E,W,A,R" },
                    { 12, "Maturation/stockage", "PC,M,E,W" },
                    { 11, "Ultrafiltration", "PC,M,E,W" },
                    { 10, "Homogénéisation", "PC,M,E,W" },
                    { 9, "Ecrémage", "PC,M,E,W" },
                    { 8, "Traitement thermique", "PC,M,E,W" },
                    { 7, "Microfiltration", "PC,M,E,W" },
                    { 6, "Stockage", "PC,M,E,W" },
                    { 5, "Matière première 3", "PC,M,E,W" },
                    { 4, "Matière première 2", "PC,M,E,W" },
                    { 3, "Matière première 1", "PC,M,E,W" },
                    { 2, "Ingrédients", "PC,M,E,W" },
                    { 13, "Pâtes fraîches", "PC,M,E,W,A,R" },
                    { 1, "Matières premières", "PC,M,E,W" }
                });
        }
    }
}
