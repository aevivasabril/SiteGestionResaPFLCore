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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<equipement>(entity =>
            {
                entity.Property(e => e.nom)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.numGmao)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.zone)
                    .WithMany(p => p.equipement)
                    .HasForeignKey(d => d.zoneID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_equipement_zone");
            });

            modelBuilder.Entity<essai>(entity =>
            {
                entity.Property(e => e.commentaire).IsUnicode(false);

                entity.Property(e => e.compte_userID).IsUnicode(false);

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

                entity.Property(e => e.quantite_produit)
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
                entity.Property(e => e.compte_userID).IsUnicode(false);

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
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}