/*
 * website developped to manage the dairy platform STLO operations  
 * Code by Anny VIVAS, inspired from the operationnal functioning of the ancien website developped by Bruno PERRET  
 * July 2021
 * website includes code from:
 *  DotNetZip library for dealing with zip, bsip and zlib from .net 
 *  Created by: Henrik/Dino Chiesa
 * 
 *  MailKit open source library for .NET mail-client 
 *  Created by:  Jeffrey Stedfast
 * 
 *  Microsoft.AspNetCore.Identity.EntityFrameworkCore, ASP.NET Core Identity provider that uses Entity Framework Core
 *  Created by: Microsoft
 *  
 *  Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation, Runtime compilation support for Razor views and Razor pages in ASP.NET Core MVC
 *  Created by: Microsoft
 *
 *  Microsoft.EntityFrameworkCore.Design, Shared design-time components for Entity Framework Core tools
 *  Created by: Microsoft
 *
 *  Microsoft.EntityFrameworkCore.SqlServer, Microsoft SQL Server database provider for Entity Framework Core
 *  Created by: Microsoft
 *
 *  Ncrontab, NCrontab is crontab for all .NET runtimes supported by .NET Standard 1.0. It provides parsing and formatting of crontab expressions as well as calculation of occurrences of time based on a schedule expressed in the crontab format
 *  Created by: Atif Aziz
 *   
 * This projet is released under the terms of the GNU general public license GPL version 3 or later:
 * availaible here: https://www.gnu.org/licenses/gpl-3.0-standalone.html
 * 
 * Copyright (c) 2021-2024 Anny Vivas
 */

using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data.ModifEquip
{
    public class EquipsToModifDB: IEquipsToModifDB
    {
        private readonly GestionResaContext context;
        private readonly ILogger<EquipsToModifDB> logger;

        public EquipsToModifDB(
            GestionResaContext context,
            ILogger<EquipsToModifDB> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public List<InfosEquipement> ListeEquipementsXZone(int idZone)
        {
            List<InfosEquipement> List = new List<InfosEquipement>();

            // Liste des équipements de la zone
            var query = context.equipement.Where(e => e.zoneID == idZone && e.equip_delete != true).ToList();

            foreach (var equip in query)
            {
                // Vérifier s'il existe une fiche materiel pour cet équipement
                var fiche = context.doc_fiche_materiel.FirstOrDefault(d => d.equipementID == equip.id);
                
                if (fiche == null)
                {
                    List.Add(new InfosEquipement
                    {
                        IdEquipement = equip.id,
                        NomEquipement = equip.nom,
                        NumGmaoEquipement = equip.numGmao,
                        FicheMaterielDispo = false
                    });
                }
                else
                {
                    List.Add(new InfosEquipement
                    {
                        IdEquipement = equip.id,
                        NomEquipement = equip.nom,
                        NumGmaoEquipement = equip.numGmao,
                        FicheMaterielDispo = true,
                        NomFicheMat = fiche.nom_document,
                        DateModif = fiche.date_modification,
                        IdFicheMat = fiche.id
                    });
                }              
            }
            return List;
        }

        public bool AjouterFicheXEquipement(int idEquipement, byte[] content, string nom)
        {
            bool IsOk = false;

            try
            {
                doc_fiche_materiel doc = new doc_fiche_materiel
                {
                    contenu_fiche = content,
                    date_modification = DateTime.Now,
                    nom_document = nom,
                    equipementID = idEquipement
                };
                context.doc_fiche_materiel.Add(doc);

                context.SaveChanges();
                IsOk = true;
            }
            catch(Exception e)
            {
                logger.LogError("Problème lors de l'écriture de la fiche materiel: " + nom + "dans la BDD: " + e.ToString());
                IsOk = false;
            }
            return IsOk;
        }

        public bool SupprimerFicheMat(int IdFiche)
        {
            bool isOk = false;
            doc_fiche_materiel doc = new doc_fiche_materiel();
            try
            {
                doc = context.doc_fiche_materiel.First(d => d.id == IdFiche);
                context.doc_fiche_materiel.Remove(doc);
                context.SaveChanges();
                isOk = true;
            }
            catch(Exception e)
            {
                isOk = false;
                logger.LogError("", "Problème pour supprimer la fiche materiel: " + doc.nom_document + "Erreur: " + e.ToString());
            }

            return isOk;
        }

        public bool ModifierFicheMat(int IdFiche, byte[] Contenu, string nomDoc)
        {
            bool IsOk = false;
            doc_fiche_materiel fiche = new doc_fiche_materiel();

            try
            {
                fiche = context.doc_fiche_materiel.First(f => f.id == IdFiche);
                fiche.contenu_fiche = Contenu;
                fiche.nom_document = nomDoc;
                fiche.date_modification = DateTime.Now;
                context.SaveChanges();
                IsOk = true;
            }
            catch(Exception e)
            {
                IsOk = false;
                logger.LogError("", "Problème pour modifier la fiche materiel: " + fiche.nom_document + ". Erreur: " + e.ToString());
            }
           
            return IsOk;
        }

        public string ObtNomEquipement(int IdEquipement)
        {
            return context.equipement.First(e => e.id == IdEquipement).nom;
        }

        public bool SupprimerEquipement(int IdEquipement)
        {
            bool IsOk = false;
            equipement equip = context.equipement.First(e => e.id == IdEquipement);
            try
            {
                equip.equip_delete = true;
                context.SaveChanges();
                IsOk = true;
            }
            catch (Exception e)
            {
                IsOk = false;
                logger.LogError("", "Problème pour supprimer l'équipement: " + equip.nom + ". Erreur: " + e.ToString());
            }
            return IsOk;
        }

        /*public bool ModifierNumGMAO(string numGmao, int idEquipement)
        {
            bool IsOk = false;
            equipement equip = new equipement();

            try
            {
                equip = context.equipement.First(e => e.id == idEquipement);
                equip.numGmao = numGmao;
                context.SaveChanges();
                IsOk = true;
            }
            catch(Exception e)
            {
                IsOk = false;
                logger.LogError("", "Problème pour modifier le numéro GMAO pour l'équipement: " + equip.nom + ". Erreur: " + e.ToString());
            }
            return IsOk;
        }*/
    }
}
