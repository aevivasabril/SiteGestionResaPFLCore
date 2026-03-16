using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutTableCompteursEnergies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "compteurs_energies",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_compteur = table.Column<string>(unicode: false, nullable: true),
                    nomTabPcVue = table.Column<string>(unicode: false, nullable: false),
                    equipementID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compteurs_energies", x => x.id);
                    table.ForeignKey(
                        name: "FK_compteur_energies_equipement",
                        column: x => x.equipementID,
                        principalTable: "equipement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "841f80e1-39e4-47be-88ad-d7739a671d84");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "718cfdaa-fa01-4565-9ea3-4439635349c5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "7a980248-43a5-4e69-ac3a-727dca2d9d80");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "1465583b-a03f-482c-879d-748134717640");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "086b0371-075c-40bf-866e-13245ec56086");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "a9f6b385-4cd4-4943-961d-a28960ec7494");

            migrationBuilder.InsertData(
                table: "compteurs_energies",
                columns: new[] { "id", "equipementID", "nomTabPcVue", "nom_compteur" },
                values: new object[,]
                {
                    { 1, null, "tab_COMPT_GENERAL", "Compteur electrique general PFL" },
                    { 2, 225, "tab_COMPT_EVAPO", "Compteur electrique evapo-concentrateur" },
                    { 3, 223, "tab_COMPT_MTH", "Compteur electrique microthermics" },
                    { 4, 170, "tab_COMPT_STEPHAN", "Compteur electrique cuiseur Stephan" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_compteurs_energies_equipementID",
                table: "compteurs_energies",
                column: "equipementID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "compteurs_energies");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e10acce7-ff1f-4958-9c58-c260adb39c4d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "08bf8674-f958-42d9-bc0a-c4bb1497959c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "b67b4085-a6b0-4b53-aace-968c4a4b8433");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "05731273-0c4a-4a9a-ba92-6f8f5b41ece6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "ConcurrencyStamp",
                value: "8b133726-4ffc-4c00-8bad-7dc742605d31");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "ConcurrencyStamp",
                value: "656d4b65-733d-43c6-9d8a-40452ff75e83");
        }
    }
}
