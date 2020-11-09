using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteGestionResaCore.Migrations
{
    public partial class MigrationInitial : Migration
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
                    zoneID = table.Column<int>(nullable: true),
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
                        onDelete: ReferentialAction.SetNull);
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
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "3e37fefe-c76b-48ce-a0a3-0bf3ebcd23dd", "Admin", "ADMIN" },
                    { 2, "19ef18fa-29cb-4f21-99cb-43a6b2334eb9", "Utilisateur", "UTILISATEUR" },
                    { 3, "d7352a83-2b79-4e87-8d4a-bf84ee723b69", "MainAdmin", "MAINADMIN" },
                    { 4, "8461f932-a89a-4f73-85c6-aed542489c29", "Logistic", "LOGISTIC" }
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

            migrationBuilder.InsertData(
                table: "zone",
                columns: new[] { "id", "nom_zone" },
                values: new object[,]
                {
                    { 15, "Salle alimentaire Ap8" },
                    { 14, "Hâloir Ap7" },
                    { 13, "Salle alimentaire Ap6" },
                    { 12, "Salle alimentaire Ap5" },
                    { 11, "Labo" },
                    { 10, "Saumurage" },
                    { 9, "Salle Stephan" },
                    { 4, "Membranes" },
                    { 7, "Pâtes préssées cuites" },
                    { 6, "Pâtes molles tranchage" },
                    { 5, "Pâtes molles moulage" },
                    { 16, "Salle alimentaire Ap9" },
                    { 3, "Préparation des laits" },
                    { 2, "Dépotage & Stockage" },
                    { 1, "Concentration & Sechage" },
                    { 8, "Innovation" },
                    { 17, "Equipements mobiles" }
                });

            migrationBuilder.InsertData(
                table: "equipement",
                columns: new[] { "id", "mobile", "nom", "numGmao", "zoneID" },
                values: new object[,]
                {
                    { 166, true, "Balance HBM 60 Kg (WE2110)", "BAL0054", 1 },
                    { 202, true, "Machine emballage sous vide BRITEK SC800L", "EMB0001", 15 },
                    { 208, true, "Homogénéisateur Panda", "HOMO0007", 13 },
                    { 244, true, "Thermocook", "", 12 },
                    { 242, false, "Hotte PSM", "", 12 },
                    { 205, true, "Etuve biocomcept BC240 FIRLABO", "ETUV0039", 12 },
                    { 204, true, "Chariot dosage ERECAM combidos 102T (doseuse)", "EMB0004", 12 },
                    { 203, true, "Thermoscelleuse ERECAM semi-automatique dia:68/95/116", "EMB0003", 12 },
                    { 168, false, "Analyseur humidité METTLER TOLEDO 71 g (HE73/01)", "BAL0059", 11 },
                    { 240, false, "Bac de saumurage", "ECUV0015", 10 },
                    { 170, false, "Mélangeur cuiseur stéphan", "CUISMEL0001", 9 },
                    { 163, false, "Balance 32 Kg (KA32s)", "BAL0003", 9 },
                    { 243, false, "Boucle de Traitement Thermique Bain-marie MEMMERT - Type WNE45 + Thermo Haake K35", "PILOT0022", 8 },
                    { 226, false, "Pilote de microfiltration P3", "PILOT0015", 8 },
                    { 224, true, "Pilote UF TAMI/Tia 8Kda mobile", "PILOT0013", 8 },
                    { 223, false, "Pilote de traitement thermique UHT-HTST Lab 25EDH", "PILOT0011", 8 },
                    { 221, false, "UF TAMI/tech-sep 8 kDa (13 m2)", "PILOT0009", 8 },
                    { 217, true, "Pilote de microfiltration MFS1", "PILOT0005", 8 },
                    { 165, true, "Balance OHAUS 2 Kg (Scout Pro SPU2001)", "BAL0011", 16 },
                    { 215, false, "Pilote OI NF UF Prolab Milipore", "PILOT0003", 8 },
                    { 179, false, "Armoire affinage AFV7HC Elimeca 1", "ECLIM0001", 16 },
                    { 167, true, "Balance 60Kg PRECIA MOLEN (X112-A)", "BAL0057", 17 },
                    { 231, true, "Pompe centrifuge 20 à 30 m3/h", "POMPE0002", 17 },
                    { 201, true, "Mini-cuve N°5 (150L)", "ECUV0027", 17 },
                    { 195, true, "Mini-cuve N°4 (100L)", "ECUV0020", 17 },
                    { 194, true, "Mini-cuve N°8 (150L)", "ECUV0019", 17 },
                    { 187, true, "Mini cuve 150L", "ECUV0007", 17 },
                    { 186, true, "Mini-cuve N°3 (100L)", "ECUV0006", 17 },
                    { 185, true, "Mini-cuve N°2 (100L)", "ECUV0005", 17 },
                    { 184, true, "Mini-cuve N°6 (150L)", "ECUV0004", 17 },
                    { 183, true, "Mini-cuve N°1 (150L)", "ECUV0003", 17 },
                    { 181, true, "Ecrémeuse ELECREM modèle 3 (150L/h)", "ECREM0001", 17 },
                    { 177, true, "Echangeur avec comptage", "ECH0009", 17 },
                    { 176, true, "Echangeur avec pompe centrifuge", "ECH0007", 17 },
                    { 175, true, "Echangeur avec pompe centrifuge(bleu)", "ECH0006", 17 },
                    { 174, true, "Thermorégulateur vulcatherm (séchage)", "ECH0005", 17 },
                    { 173, true, "Thermorégulateur vulcatherm (membrane)", "ECH0004", 17 },
                    { 172, true, "Echangeur avec pompe centrifuge (regulation chaud/froid)", "ECH0002", 17 },
                    { 171, true, "Echangeur récupérateur", "ECH0001", 17 },
                    { 180, false, "Armoire affinage AFV7HC Elimeca 2", "ECLIM0002", 16 },
                    { 232, true, "Pompe PCM - 5 m3/h", "POMPE0003", 17 },
                    { 214, false, "Pilote UF TIA/PALL 0,02u (JYG)", "PILOT0002", 8 },
                    { 199, false, "Tank GEA 550L avec agitation et groupe froid CVB", "ECUV0025", 8 },
                    { 178, false, "Echangeur à surface raclée Contherm (ESR)", "ECH0010", 4 },
                    { 238, false, "Ecrémeuse Elecrem (ACTALIA) 500 l/h", "ACTALIA", 3 },
                    { 229, false, "Pilote VALOBAB (MF et UF) SKID 12EO46", "PILOT0018", 3 },
                    { 222, false, "Stérilisateur pilote tubulaire électrique ACTINI", "PILOT0010", 3 },
                    { 218, false, "Pilote de microfiltration MFMG", "PILOT0006", 3 },
                    { 207, false, "Homogénéisateur 12/51H RANNIE", "HOMO0003", 3 },
                    { 206, false, "Homogénéisateur 2 têtes RANNIE", "HOMO0002", 3 },
                    { 188, false, "2 cuves maturation 500L", "ECUV0010", 3 },
                    { 182, false, "Ecrémeuse Westfalia EASYCREAM", "ECREM0002", 3 },
                    { 164, false, "Balance 300Kg (ID2 + KCS300)", "BAL0004", 3 },
                    { 239, true, "Camion collecte", "", 2 },
                    { 234, true, "Pompe de transfert de lait 58L/min (bleue)", "POMPE0006", 2 },
                    { 211, false, "Ensemble NEP", "MLAV0016", 2 },
                    { 200, false, "Cuve 2000L avec agitateur", "ECUV0026", 2 },
                    { 228, false, "Pilote tour de sèchage MINOR", "PILOT0017", 1 },
                    { 227, false, "Pilote de sèchage mono-disperse", "PILOT0016", 1 },
                    { 225, false, "Pilote évaporateur à flot tombant FF-1", "PILOT0014", 1 },
                    { 189, false, "Tank 1000L avec agitation et groupe froid", "ECUV0012", 4 },
                    { 213, false, "Pilote ultrafiltration TIA spirale", "PILOT0001", 8 },
                    { 216, false, "Pilote filtration engineering (OI et NF)", "PILOT0004", 4 },
                    { 220, false, "Pilote de microfiltration GP7", "PILOT0008", 4 },
                    { 241, false, "3 cuves fromagerie 200 Litres", "ACTALIA", 7 },
                    { 236, false, "Presse à fromage horizontale", "PRES0003", 7 },
                    { 235, false, "Presse à fromage verticale", "PRES0002", 7 },
                    { 198, true, "Mini-cuve de fabrication 3(2 cuves 10 litres et 20 litres)", "ECUV0023", 7 },
                    { 197, true, "Mini-cuve de fabrication 2(2 cuves 10 litres et 20 litres)", "ECUV0022", 7 },
                    { 196, true, "Mini-cuve de fabrication 1(2 cuves 10 litres et 20 litres)", "ECUV0021", 7 },
                    { 190, false, "Cuve PPC Châlon-Mégard 1000 litres", "ECUV0014", 7 },
                    { 237, false, "Tranche-caillé", "TRAN0001", 6 },
                    { 210, true, "Chariots porte-bassines PM (N°2)", "MANUT0012", 6 },
                    { 209, true, "Chariots porte-bassines PM (N°1)", "MANUT002", 6 },
                    { 169, false, "Brassoires PM", "BRAS0001", 6 },
                    { 212, false, "Système de moulage PM et basculeur", "MOUL0001", 5 },
                    { 193, true, "Table égouttage PM 3", "ECUV0018", 5 },
                    { 192, true, "Table égouttage PM 2", "ECUV0017", 5 },
                    { 191, true, "Table égouttage PM 1", "ECUV0016", 5 },
                    { 162, false, "Balance Arpège 150k", "BAL0002", 5 },
                    { 230, false, "Pilote UF (optimal)", "PILOT0019", 4 },
                    { 219, false, "Pilote de microfiltration MFS19", "PILOT0007", 4 },
                    { 233, true, "Pompe disperseur de poudre - TRIBLENDER", "POMPE0004", 17 }
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
