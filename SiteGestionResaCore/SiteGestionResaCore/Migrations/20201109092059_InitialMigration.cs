using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ld_destination",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_destination = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ld_destination", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ld_financement",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_financement = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ld_financement", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ld_produit_in",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_produit_in = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ld_produit_in", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ld_provenance",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_provenance = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ld_provenance", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ld_provenance_produit",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_provenance_produit = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ld_provenance_produit", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ld_type_projet",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_type_projet = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ld_type_projet", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organisme",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_organisme = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organisme", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "zone",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_zone = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zone", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    nom = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    prenom = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    organismeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_utilisateur_organisme",
                        column: x => x.organismeID,
                        principalTable: "organisme",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "projet",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titre_projet = table.Column<string>(unicode: false, nullable: false),
                    num_projet = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    type_projet = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    financement = table.Column<string>(unicode: false, nullable: true),
                    organismeID = table.Column<int>(nullable: true),
                    mailRespProjet = table.Column<string>(unicode: false, nullable: true),
                    provenance = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    description_projet = table.Column<string>(unicode: false, nullable: true),
                    date_creation = table.Column<DateTime>(type: "datetime", nullable: false),
                    compte_userID = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projet", x => x.id);
                    table.ForeignKey(
                        name: "FK_projet_organisme",
                        column: x => x.organismeID,
                        principalTable: "organisme",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "equipement",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(unicode: false, nullable: false),
                    zoneID = table.Column<int>(nullable: false),
                    numGmao = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    mobile = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipement", x => x.id);
                    table.ForeignKey(
                        name: "FK_equipement_zone",
                        column: x => x.zoneID,
                        principalTable: "zone",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "essai",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    projetID = table.Column<int>(nullable: false),
                    compte_userID = table.Column<string>(unicode: false, nullable: true),
                    date_creation = table.Column<DateTime>(type: "datetime", nullable: false),
                    manipulateurID = table.Column<int>(nullable: false),
                    type_produit_entrant = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    precision_produit = table.Column<string>(unicode: false, nullable: true),
                    quantite_produit = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    provenance_produit = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    destination_produit = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    transport_stlo = table.Column<bool>(nullable: false),
                    status_essai = table.Column<string>(unicode: false, nullable: false),
                    commentaire = table.Column<string>(unicode: false, nullable: true),
                    repondu_enquete = table.Column<bool>(nullable: true),
                    date_validation = table.Column<DateTime>(type: "datetime", nullable: true),
                    resa_supprime = table.Column<bool>(nullable: true),
                    raison_suppression = table.Column<string>(unicode: false, nullable: true),
                    resa_refuse = table.Column<bool>(nullable: true),
                    raison_refus = table.Column<string>(unicode: false, nullable: true),
                    confidentialite = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    date_inf_confidentiel = table.Column<DateTime>(type: "datetime", nullable: true),
                    date_sup_confidentiel = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_essai", x => x.id);
                    table.ForeignKey(
                        name: "FK_essai_utilisateur",
                        column: x => x.manipulateurID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_essai_projet",
                        column: x => x.projetID,
                        principalTable: "projet",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reservation_projet",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    equipementID = table.Column<int>(nullable: false),
                    essaiID = table.Column<int>(nullable: false),
                    date_debut = table.Column<DateTime>(type: "datetime", nullable: false),
                    date_fin = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservation_projet", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservation_projet_equipement",
                        column: x => x.equipementID,
                        principalTable: "equipement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservation_projet_essai",
                        column: x => x.essaiID,
                        principalTable: "essai",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "organisme",
                columns: new[] { "id", "nom_organisme" },
                values: new object[,]
                {
                    { 1, "Inrae" },
                    { 2, "Agrocampus Ouest" },
                    { 3, "Quescrem" },
                    { 4, "Eurial" },
                    { 5, "Actalia" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_organismeID",
                table: "AspNetUsers",
                column: "organismeID");

            migrationBuilder.CreateIndex(
                name: "IX_equipement_zoneID",
                table: "equipement",
                column: "zoneID");

            migrationBuilder.CreateIndex(
                name: "IX_essai_manipulateurID",
                table: "essai",
                column: "manipulateurID");

            migrationBuilder.CreateIndex(
                name: "IX_essai_projetID",
                table: "essai",
                column: "projetID");

            migrationBuilder.CreateIndex(
                name: "IX_projet_organismeID",
                table: "projet",
                column: "organismeID");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_projet_equipementID",
                table: "reservation_projet",
                column: "equipementID");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_projet_essaiID",
                table: "reservation_projet",
                column: "essaiID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ld_destination");

            migrationBuilder.DropTable(
                name: "ld_financement");

            migrationBuilder.DropTable(
                name: "ld_produit_in");

            migrationBuilder.DropTable(
                name: "ld_provenance");

            migrationBuilder.DropTable(
                name: "ld_provenance_produit");

            migrationBuilder.DropTable(
                name: "ld_type_projet");

            migrationBuilder.DropTable(
                name: "reservation_projet");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "equipement");

            migrationBuilder.DropTable(
                name: "essai");

            migrationBuilder.DropTable(
                name: "zone");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "projet");

            migrationBuilder.DropTable(
                name: "organisme");
        }
    }
}
