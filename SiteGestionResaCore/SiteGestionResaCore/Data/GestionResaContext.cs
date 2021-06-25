﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SiteGestionResaCore.Data;

namespace SiteGestionResaCore.Data.Data
{
    public partial class GestionResaContext : IdentityDbContext<utilisateur, IdentityRole<int>, int>
    {
        public GestionResaContext()
        {
        }

        public GestionResaContext(DbContextOptions<GestionResaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<equipement> equipement { get; set; }
        public virtual DbSet<essai> essai { get; set; }
        public virtual DbSet<ld_destination> ld_destination { get; set; }
        public virtual DbSet<ld_financement> ld_financement { get; set; }
        public virtual DbSet<ld_produit_in> ld_produit_in { get; set; }
        public virtual DbSet<ld_provenance> ld_provenance { get; set; }
        public virtual DbSet<ld_provenance_produit> ld_provenance_produit { get; set; }
        public virtual DbSet<ld_type_projet> ld_type_projet { get; set; }
        public virtual DbSet<organisme> organisme { get; set; }
        public virtual DbSet<projet> projet { get; set; }
        public virtual DbSet<reservation_projet> reservation_projet { get; set; }
        public virtual DbSet<zone> zone { get; set; }
        public virtual DbSet<enquete> enquete { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<enquete>(entity =>
            {
                entity.Property(e => e.fichierReponse).IsUnicode(false);

                entity.Property(e => e.date_envoi_enquete).HasColumnType("datetime");

                entity.Property(e => e.date_premier_envoi).HasColumnType("datetime");

                entity.Property(e => e.date_reponse).HasColumnType("datetime");

                entity.HasOne(d => d.essai)
                    .WithOne(p => p.enquete)
                    .HasForeignKey<enquete>(d => d.essaiId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_enquete_essai");
            });

            modelBuilder.Entity<equipement>(entity =>
            {
                entity.Property(e => e.nomTabPcVue).IsUnicode(false);

                entity.Property(e => e.nom)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.numGmao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.zone)
                    .WithMany(p => p.equipement)
                    .HasForeignKey(d => d.zoneID)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_equipement_zone");
            });

            modelBuilder.Entity<essai>(entity =>
            {
                entity.Property(e => e.titreEssai).IsUnicode(false);

                entity.Property(e => e.confidentialite)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.date_creation).HasColumnType("datetime");

                entity.Property(e => e.date_inf_confidentiel).HasColumnType("datetime");

                entity.Property(e => e.date_sup_confidentiel).HasColumnType("datetime");

                entity.Property(e => e.date_validation).HasColumnType("datetime");

                entity.Property(e => e.destination_produit)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.precision_produit).IsUnicode(false);

                entity.Property(e => e.provenance_produit)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.raison_refus).IsUnicode(false);

                entity.Property(e => e.raison_suppression).IsUnicode(false);

                entity.Property(e => e.status_essai)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.type_produit_entrant)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.manipulateur)
                    .WithMany(p => p.essai)
                    .HasForeignKey(d => d.manipulateurID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_essai_utilisateur");

                entity.HasOne(d => d.projet)
                    .WithMany(p => p.essai)
                    .HasForeignKey(d => d.projetID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_essai_projet");
            });

            modelBuilder.Entity<ld_destination>(entity =>
            {
                entity.Property(e => e.nom_destination)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ld_financement>(entity =>
            {
                entity.Property(e => e.nom_financement)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ld_produit_in>(entity =>
            {
                entity.Property(e => e.nom_produit_in)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ld_provenance>(entity =>
            {
                entity.Property(e => e.nom_provenance)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ld_provenance_produit>(entity =>
            {
                entity.Property(e => e.nom_provenance_produit)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ld_type_projet>(entity =>
            {
                entity.Property(e => e.nom_type_projet)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<organisme>(entity =>
            {
                entity.Property(e => e.nom_organisme)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<projet>(entity =>
            {
                entity.Property(e => e.date_creation).HasColumnType("datetime");

                entity.Property(e => e.description_projet).IsUnicode(false);

                entity.Property(e => e.financement).IsUnicode(false);

                entity.Property(e => e.mailRespProjet).IsUnicode(false);

                entity.Property(e => e.num_projet)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.provenance)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.titre_projet)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.type_projet)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.organisme)
                    .WithMany(p => p.projet)
                    .HasForeignKey(d => d.organismeID)
                    .HasConstraintName("FK_projet_organisme");
            });

            modelBuilder.Entity<reservation_projet>(entity =>
            {
                entity.Property(e => e.date_debut).HasColumnType("datetime");

                entity.Property(e => e.date_fin).HasColumnType("datetime");

                entity.HasOne(d => d.equipement)
                    .WithMany(p => p.reservation_projet)
                    .HasForeignKey(d => d.equipementID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reservation_projet_equipement");

                entity.HasOne(d => d.essai)
                    .WithMany(p => p.reservation_projet)
                    .HasForeignKey(d => d.essaiID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_reservation_projet_essai");
            });

            modelBuilder.Entity<utilisateur>(entity =>
            {
                entity.Property(e => e.nom)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.prenom)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.organisme)
                    .WithMany(p => p.utilisateur)
                    .HasForeignKey(d => d.organismeID)
                    .HasConstraintName("FK_utilisateur_organisme");
            });

            modelBuilder.Entity<zone>(entity =>
            {
                entity.Property(e => e.nom_zone)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.HasMany(z => z.equipement).WithOne(e=>e.zone).HasForeignKey(e=>e.zoneID);
               
            });

            modelBuilder.Entity<organisme>().HasData(new organisme[] { new organisme{ nom_organisme = "Inrae", id = 1}, new organisme { nom_organisme = "Agrocampus Ouest", id = 2 },
                new organisme { nom_organisme = "Sill", id = 3 }, new organisme{ nom_organisme = "Eurial", id = 4}, new organisme{ nom_organisme = "Actalia", id = 5}, 
                new organisme{ nom_organisme = "Sodial", id = 6}, new organisme{ nom_organisme = "Isigny sainte mère", id = 7} });

            modelBuilder.Entity<IdentityRole<int>>().HasData(new IdentityRole<int>[] { new IdentityRole<int> { Name="Admin", Id = 1, NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString()},
                new IdentityRole<int> { Name = "Utilisateur", Id = 2 , NormalizedName="UTILISATEUR", ConcurrencyStamp = Guid.NewGuid().ToString()}, 
                new IdentityRole<int> { Name = "MainAdmin", Id = 3, NormalizedName="MAINADMIN", ConcurrencyStamp = Guid.NewGuid().ToString() }, 
                new IdentityRole<int> { Name="Logistic", Id = 4, NormalizedName="LOGISTIC", ConcurrencyStamp = Guid.NewGuid().ToString()} });

            modelBuilder.Entity<zone>().HasData(new zone[] { new zone { id = 1, nom_zone = "Concentration & Sechage" }, new zone { id = 2, nom_zone = "Dépotage & Stockage" },
                new zone { id = 3, nom_zone = "Préparation des laits" }, new zone { id = 4, nom_zone = "Membranes" }, new zone { id = 5, nom_zone = "Pâtes molles moulage" },
                new zone { id = 6, nom_zone = "Pâtes molles tranchage" }, new zone { id = 7, nom_zone = "Pâtes préssées cuites" }, new zone { id = 8, nom_zone = "Innovation" }, 
                new zone { id = 9, nom_zone = "Salle Stephan" }, new zone { id = 10, nom_zone = "Saumurage" }, new zone { id = 11, nom_zone = "Labo" }, 
                new zone { id = 12, nom_zone = "Salle alimentaire Ap5" }, new zone { id = 13, nom_zone = "Salle alimentaire Ap6" }, new zone { id = 14, nom_zone = "Hâloir Ap7" },
                new zone { id = 15, nom_zone = "Salle alimentaire Ap8" }, new zone { id = 16, nom_zone = "Salle alimentaire Ap9" }, new zone { id = 17, nom_zone = "Equipements mobiles" } });

            modelBuilder.Entity<equipement>().HasData(new equipement[] { new equipement { id = 162, nom = "Balance Arpège 150k", zoneID = 5, numGmao = "BAL0002", mobile = false }, new equipement { id = 163, nom = "Balance 32 Kg (KA32s)", zoneID = 9, numGmao = "BAL0003", mobile = false }, 
                new equipement { id = 164, nom = "Balance 300Kg (ID2 + KCS300)", zoneID = 3, numGmao = "BAL0004", mobile = false }, new equipement { id = 165, nom = "Balance OHAUS 2 Kg (Scout Pro SPU2001)", zoneID = 16, numGmao = "BAL0011", mobile = true }, 
                new equipement { id = 166, nom = "Balance HBM 60 Kg (WE2110)", zoneID = 1, numGmao = "BAL0054", mobile = true }, new equipement { id = 167, nom = "Balance 60Kg PRECIA MOLEN (X112-A)", zoneID = 17, numGmao = "BAL0057", mobile = true }, 
                new equipement { id = 168, nom = "Analyseur humidité METTLER TOLEDO 71 g (HE73/01)", zoneID = 11, numGmao = "BAL0059", mobile = false }, new equipement { id = 169, nom = "Brassoires PM", zoneID = 6, numGmao = "BRAS0001", mobile = false }, 
                new equipement { id = 170, nom = "Mélangeur cuiseur stéphan", zoneID = 9, numGmao = "CUISMEL0001", mobile = false }, new equipement { id = 171, nom = "Echangeur récupérateur", zoneID = 17, numGmao = "ECH0001", mobile = true }, 
                new equipement { id = 172, nom = "Echangeur avec pompe centrifuge (regulation chaud/froid)", zoneID = 17, numGmao = "ECH0002", mobile = true }, new equipement { id = 173, nom = "Thermorégulateur vulcatherm (membrane)", zoneID = 17, numGmao = "ECH0004", mobile = true },
                new equipement { id = 174, nom = "Thermorégulateur vulcatherm (séchage)", zoneID = 17, numGmao = "ECH0005", mobile = true }, new equipement { id = 175, nom = "Echangeur avec pompe centrifuge(bleu)", zoneID = 17, numGmao = "ECH0006", mobile = true }, 
                new equipement { id = 176, nom = "Echangeur avec pompe centrifuge", zoneID = 17, numGmao = "ECH0007", mobile = true }, new equipement { id = 177, nom = "Echangeur avec comptage", zoneID = 17, numGmao = "ECH0009", mobile = true },
                new equipement { id = 178, nom = "Echangeur à surface raclée Contherm (ESR)", zoneID = 4, numGmao = "ECH0010", mobile = false }, new equipement { id = 179, nom = "Armoire affinage AFV7HC Elimeca 1", zoneID = 16, numGmao = "ECLIM0001", mobile = false }, 
                new equipement { id = 180, nom = "Armoire affinage AFV7HC Elimeca 2", zoneID = 16, numGmao = "ECLIM0002", mobile = false }, new equipement { id = 181, nom = "Ecrémeuse ELECREM modèle 3 (150L/h)", zoneID = 17, numGmao = "ECREM0001", mobile = true }, 
                new equipement { id = 182, nom = "Ecrémeuse Westfalia EASYCREAM", zoneID = 3, numGmao = "ECREM0002", mobile = false, nomTabPcVue = "tab_UA_ECREM" }, new equipement { id = 183, nom = "Mini-cuve N°1 (150L)", zoneID = 17, numGmao = "ECUV0003", mobile = true },
                new equipement { id = 184, nom = "Mini-cuve N°6 (150L)", zoneID = 17, numGmao = "ECUV0004", mobile = true }, new equipement { id = 185, nom = "Mini-cuve N°2 (100L)", zoneID = 17, numGmao = "ECUV0005", mobile = true }, 
                new equipement { id = 186, nom = "Mini-cuve N°3 (100L)", zoneID = 17, numGmao = "ECUV0006", mobile = true }, new equipement { id = 187, nom = "Mini cuve 150L", zoneID = 17, numGmao = "ECUV0007", mobile = true }, 
                new equipement { id = 188, nom = "2 cuves maturation 500L", zoneID = 3, numGmao = "ECUV0010", mobile = false, nomTabPcVue = "tab_UA_MAT" }, new equipement { id = 189, nom = "Tank 1000L avec agitation et groupe froid", zoneID = 4, numGmao = "ECUV0012", mobile = false },
                new equipement { id = 190, nom = "Cuve PPC Châlon-Mégard 1000 litres", zoneID = 7, numGmao = "ECUV0014", mobile = false }, new equipement { id = 191, nom = "Table égouttage PM 1", zoneID = 5, numGmao = "ECUV0016", mobile = true },
                new equipement { id = 192, nom = "Table égouttage PM 2", zoneID = 5, numGmao = "ECUV0017", mobile = true }, new equipement { id = 193, nom = "Table égouttage PM 3", zoneID = 5, numGmao = "ECUV0018", mobile = true }, 
                new equipement { id = 194, nom = "Mini-cuve N°8 (150L)", zoneID = 17, numGmao = "ECUV0019", mobile = true }, new equipement { id = 195, nom = "Mini-cuve N°4 (100L)", zoneID = 17, numGmao = "ECUV0020", mobile = true }, 
                new equipement { id = 196, nom = "Mini-cuve de fabrication 1(2 cuves 10 litres et 20 litres)", zoneID = 7, numGmao = "ECUV0021", mobile = true }, new equipement { id = 197, nom = "Mini-cuve de fabrication 2(2 cuves 10 litres et 20 litres)", zoneID = 7, numGmao = "ECUV0022", mobile = true },
                new equipement { id = 198, nom = "Mini-cuve de fabrication 3(2 cuves 10 litres et 20 litres)", zoneID = 7, numGmao = "ECUV0023", mobile = true }, new equipement { id = 199, nom = "Tank GEA 550L avec agitation et groupe froid CVB", zoneID = 8, numGmao = "ECUV0025", mobile = false },
                new equipement { id = 200, nom = "Cuve 2000L avec agitateur", zoneID = 2, numGmao = "ECUV0026", mobile = false, nomTabPcVue= "tab_UA_CUV" }, new equipement { id = 201, nom = "Mini-cuve N°5 (150L)", zoneID = 17, numGmao = "ECUV0027", mobile = true }, 
                new equipement { id = 202, nom = "Machine emballage sous vide BRITEK SC800L", zoneID = 15, numGmao = "EMB0001", mobile = true }, new equipement { id = 203, nom = "Thermoscelleuse ERECAM semi-automatique dia:68/95/116", zoneID = 12, numGmao = "EMB0003", mobile = true },
                new equipement { id = 204, nom = "Chariot dosage ERECAM combidos 102T (doseuse)", zoneID = 12, numGmao = "EMB0004", mobile = true }, new equipement { id = 205, nom = "Etuve biocomcept BC240 FIRLABO", zoneID = 12, numGmao = "ETUV0039", mobile = true }, 
                new equipement { id = 206, nom = "Homogénéisateur 2 têtes RANNIE", zoneID = 3, numGmao = "HOMO0002", mobile = false }, new equipement { id = 207, nom = "Homogénéisateur 12/51H RANNIE", zoneID = 3, numGmao = "HOMO0003", mobile = false },
                new equipement { id = 208, nom = "Homogénéisateur Panda", zoneID = 13, numGmao = "HOMO0007", mobile = true }, new equipement { id = 209, nom = "Chariots porte-bassines PM (N°1)", zoneID = 6, numGmao = "MANUT002", mobile = true }, 
                new equipement { id = 210, nom = "Chariots porte-bassines PM (N°2)", zoneID = 6, numGmao = "MANUT0012", mobile = true }, new equipement { id = 211, nom = "Ensemble NEP", zoneID = 2, numGmao = "MLAV0016", mobile = false, nomTabPcVue = "tab_UA_NEP" }, 
                new equipement { id = 212, nom = "Système de moulage PM et basculeur", zoneID = 5, numGmao = "MOUL0001", mobile = false }, new equipement { id = 213, nom = "Pilote ultrafiltration TIA spirale", zoneID = 8, numGmao = "PILOT0001", mobile = false, nomTabPcVue = "tab_UA_SPI" }, 
                new equipement { id = 214, nom = "Pilote UF TIA/PALL 0,02u (JYG)", zoneID = 8, numGmao = "PILOT0002", mobile = false, nomTabPcVue= "tab_UA_UFMF" }, new equipement { id = 215, nom = "Pilote OI NF UF Prolab Milipore", zoneID = 8, numGmao = "PILOT0003", mobile = false }, 
                new equipement { id = 216, nom = "Pilote filtration engineering (OI et NF)", zoneID = 4, numGmao = "PILOT0004", mobile = false }, new equipement { id = 217, nom = "Pilote de microfiltration MFS1", zoneID = 8, numGmao = "PILOT0005", mobile = true }, 
                new equipement { id = 218, nom = "Pilote de microfiltration MFMG", zoneID = 3, numGmao = "PILOT0006", mobile = false, nomTabPcVue = "tab_UA_MFMG" }, new equipement { id = 219, nom = "Pilote de microfiltration MFS19", zoneID = 4, numGmao = "PILOT0007", mobile = false }, 
                new equipement { id = 220, nom = "Pilote de microfiltration GP7", zoneID = 4, numGmao = "PILOT0008", mobile = false, nomTabPcVue= "tab_UA_GP7" }, new equipement { id = 221, nom = "UF TAMI/tech-sep 8 kDa (13 m2)", zoneID = 8, numGmao = "PILOT0009", mobile = false }, 
                new equipement { id = 222, nom = "Stérilisateur pilote tubulaire électrique ACTINI", zoneID = 3, numGmao = "PILOT0010", mobile = false, nomTabPcVue = "tab_UA_ACT" }, new equipement { id = 223, nom = "Pilote de traitement thermique UHT-HTST Lab 25EDH", zoneID = 8, numGmao = "PILOT0011", mobile = false, nomTabPcVue = "tab_UA_MTH" }, 
                new equipement { id = 224, nom = "Pilote UF TAMI/Tia 8Kda mobile", zoneID = 8, numGmao = "PILOT0013", mobile = true }, new equipement { id = 225, nom = "Pilote évaporateur à flot tombant FF-1", zoneID = 1, numGmao = "PILOT0014", mobile = false, nomTabPcVue = "tab_UA_EVAA, tab_UA_EVAB" }, 
                new equipement { id = 226, nom = "Pilote de microfiltration P3", zoneID = 8, numGmao = "PILOT0015", mobile = false }, new equipement { id = 227, nom = "Pilote de sèchage mono-disperse", zoneID = 1, numGmao = "PILOT0016", mobile = false }, 
                new equipement { id = 228, nom = "Pilote tour de sèchage MINOR", zoneID = 1, numGmao = "PILOT0017", mobile = false, nomTabPcVue = "tab_UA_SEC" }, new equipement { id = 229, nom = "Pilote VALOBAB (MF et UF) SKID 12EO46", zoneID = 3, numGmao = "PILOT0018", mobile = false, nomTabPcVue= "tab_UA_VALO" }, 
                new equipement { id = 230, nom = "Pilote UF (optimal)", zoneID = 4, numGmao = "PILOT0019", mobile = false, nomTabPcVue = "tab_UA_OPTIMAL" }, new equipement { id = 231, nom = "Pompe centrifuge 20 à 30 m3/h", zoneID = 17, numGmao = "POMPE0002", mobile = true }, 
                new equipement { id = 232, nom = "Pompe PCM - 5 m3/h", zoneID = 17, numGmao = "POMPE0003", mobile = true }, new equipement { id = 233, nom = "Pompe disperseur de poudre - TRIBLENDER", zoneID = 17, numGmao = "POMPE0004", mobile = true }, 
                new equipement { id = 234, nom = "Pompe de transfert de lait 58L/min (bleue)", zoneID = 2, numGmao = "POMPE0006", mobile = true }, new equipement { id = 235, nom = "Presse à fromage verticale", zoneID = 7, numGmao = "PRES0002", mobile = false }, 
                new equipement { id = 236, nom = "Presse à fromage horizontale", zoneID = 7, numGmao = "PRES0003", mobile = false }, new equipement { id = 237, nom = "Tranche-caillé", zoneID = 6, numGmao = "TRAN0001", mobile = false }, 
                new equipement { id = 238, nom = "Ecrémeuse Elecrem (ACTALIA) 500 l/h", zoneID = 3, numGmao = "ACTALIA", mobile = false }, new equipement { id = 239, nom = "Camion collecte", zoneID = 2, numGmao = "", mobile = true }, 
                new equipement { id = 240, nom = "Bac de saumurage", zoneID = 10, numGmao = "ECUV0015", mobile = false }, new equipement { id = 241, nom = "3 cuves fromagerie 200 Litres", zoneID = 7, numGmao = "ACTALIA", mobile = false }, 
                new equipement { id = 242, nom = "Hotte PSM", zoneID = 12, numGmao = "", mobile = false }, new equipement { id = 243, nom = "Boucle de Traitement Thermique Bain-marie MEMMERT - Type WNE45 + Thermo Haake K35", zoneID = 8, numGmao = "PILOT0022", mobile = false }, 
                new equipement { id = 244, nom = "Thermocook", zoneID = 12, numGmao = "", mobile = true }, new equipement { id = 245, nom = "Balance OHAUS Ranger 3000 -30Kg- tour de sechage", zoneID = 1, numGmao = "BAL0068", mobile = true },
                new equipement { id = 246, nom = "Balance OHAUS Ranger 3000 -30Kg", zoneID = 12, numGmao = "BAL0074", mobile = true }, new equipement { id = 247, nom = "Balance PRECIA MOLEN 150 kg", zoneID = 7, numGmao = "BAL0073", mobile = true } });

            modelBuilder.Entity<ld_destination>().HasData(new ld_destination[] { new ld_destination { id = 1, nom_destination = "Non connu (sans dégustation)"}, 
                new ld_destination { id = 2, nom_destination = "Plan HACCP" },
                new ld_destination { id = 3, nom_destination = "Test sensoriel" } });

            modelBuilder.Entity<ld_financement>().HasData(new ld_financement[] { new ld_financement { id = 1, nom_financement = "Public" }, new ld_financement { id = 2, nom_financement = "Privé" },
                new ld_financement { id = 3, nom_financement = "STLO" } });

            modelBuilder.Entity<ld_produit_in>().HasData(new ld_produit_in[] { new ld_produit_in { id = 1, nom_produit_in = "Autre"}, new ld_produit_in { id = 2, nom_produit_in = "Lait" },
                new ld_produit_in { id = 3, nom_produit_in = "Lactoserum" }, new ld_produit_in { id = 4, nom_produit_in = "Babeurre"} });

            modelBuilder.Entity<ld_provenance>().HasData(new ld_provenance[] { new ld_provenance { id = 1, nom_provenance = "Régional" }, new ld_provenance { id = 2, nom_provenance = "National" },
                new ld_provenance { id = 3 , nom_provenance = "International"}, new ld_provenance { id = 4 , nom_provenance = "Européen"}});

            modelBuilder.Entity<ld_provenance_produit>().HasData(new ld_provenance_produit[] { new ld_provenance_produit { id = 1, nom_provenance_produit = "Autre"},
                new ld_provenance_produit { id = 2, nom_provenance_produit = "Non connu" }, new ld_provenance_produit { id = 3, nom_provenance_produit = "Retiers"}, 
                new ld_provenance_produit { id = 4, nom_provenance_produit = "St Malo"}, new ld_provenance_produit { id = 5, nom_provenance_produit = "Montauban"},
                new ld_provenance_produit { id = 6, nom_provenance_produit = "Noyal"}, new ld_provenance_produit { id = 7, nom_provenance_produit = "Earl Lorret"},
                new ld_provenance_produit { id = 8, nom_provenance_produit = "Earl Lemarchand"}});

            modelBuilder.Entity<ld_type_projet>().HasData(new ld_type_projet[] { new ld_type_projet { id = 1, nom_type_projet = "Non connu" }, new ld_type_projet { id = 2, nom_type_projet = "Recherche" }, 
                new ld_type_projet { id = 3, nom_type_projet = "Formation/Stage" }, new ld_type_projet { id = 4 , nom_type_projet = "Industriel (cellules hébergés"} });

            base.OnModelCreating(modelBuilder);
        }
    }
}