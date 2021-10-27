using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class AjoutMaintenance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ld_type_maintenance",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_type_maintenance = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ld_type_maintenance", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "maintenance",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type_maintenance = table.Column<string>(unicode: false, nullable: true),
                    code_operation = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    userID = table.Column<int>(nullable: false),
                    intervenant_externe = table.Column<bool>(nullable: false),
                    nom_intervenant_ext = table.Column<string>(unicode: false, nullable: true),
                    description_operation = table.Column<string>(unicode: false, nullable: true),
                    maintenance_supprime = table.Column<bool>(nullable: true),
                    date_suppression = table.Column<DateTime>(type: "datetime", nullable: true),
                    date_saisie = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintenance", x => x.id);
                    table.ForeignKey(
                        name: "FK_maintenance_utilisateur",
                        column: x => x.userID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "resa_maint_equip_adjacent",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_equipement = table.Column<string>(unicode: false, nullable: true),
                    maintenanceID = table.Column<int>(nullable: false),
                    date_debut = table.Column<DateTime>(type: "datetime", nullable: false),
                    date_fin = table.Column<DateTime>(type: "datetime", nullable: false),
                    zone_affectee = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resa_maint_equip_adjacent", x => x.id);
                    table.ForeignKey(
                        name: "FK_resa_maint_equip_adjacent_maintenance",
                        column: x => x.maintenanceID,
                        principalTable: "maintenance",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reservation_maintenance",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    equipementID = table.Column<int>(nullable: false),
                    maintenanceID = table.Column<int>(nullable: false),
                    date_debut = table.Column<DateTime>(type: "datetime", nullable: false),
                    date_fin = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservation_maintenance", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservation_maintenance_equipement",
                        column: x => x.equipementID,
                        principalTable: "equipement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservation_maintenance_maintenance",
                        column: x => x.maintenanceID,
                        principalTable: "maintenance",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "9abe7793-ec13-4d7a-b8ed-25dc06502085");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "44624102-35f4-4892-a5a2-da4c36d496db");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "5fa1713a-2014-4995-91e1-f26dc715079a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "0de0548d-aad9-4c4b-8bec-88b640108703");

            migrationBuilder.InsertData(
                table: "ld_type_maintenance",
                columns: new[] { "id", "nom_type_maintenance" },
                values: new object[,]
                {
                    { 1, "Maintenance curative (Panne)" },
                    { 2, "Maintenance préventive (Interne)" },
                    { 3, "Maintenance préventive (Externe)" },
                    { 4, "Amélioration" },
                    { 5, "Autre" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_maintenance_userID",
                table: "maintenance",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_resa_maint_equip_adjacent_maintenanceID",
                table: "resa_maint_equip_adjacent",
                column: "maintenanceID");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_maintenance_equipementID",
                table: "reservation_maintenance",
                column: "equipementID");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_maintenance_maintenanceID",
                table: "reservation_maintenance",
                column: "maintenanceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ld_type_maintenance");

            migrationBuilder.DropTable(
                name: "resa_maint_equip_adjacent");

            migrationBuilder.DropTable(
                name: "reservation_maintenance");

            migrationBuilder.DropTable(
                name: "maintenance");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "0e4be807-402d-4a73-81c0-83aecc6a390a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "12ca97d4-0299-4847-b5b1-4909ca954517");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "9ecc08f9-92b1-47d2-b85c-f1e09a2f8680");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "5e97878e-3722-4a43-a247-6fee11bfd497");
        }
    }
}
