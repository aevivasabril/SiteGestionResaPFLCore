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

namespace SiteGestionResaCore.Areas.Metrologie.Data.Capteur
{
    public class CapteurDB: ICapteurDB
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext contextDb;
        private readonly ILogger<CapteurDB> logger;

        public CapteurDB(
            GestionResaContext contextDb,
            ILogger<CapteurDB> logger)
        {
            this.contextDb = contextDb;
            this.logger = logger;
        }

        public List<CapteurXAffichage> ObtenirListCapteurs()
        {
            List<CapteurXAffichage> listCapteur = new List<CapteurXAffichage>();
            var list = contextDb.capteur.ToList();

            foreach (var l in list)
            {
                equipement eq = contextDb.equipement.First(e => e.id == l.equipementID);
                CapteurXAffichage cap = new CapteurXAffichage
                {
                    idCapteur = l.id,
                    NomCapteur = l.nom_capteur,
                    NomPilote = eq.nom, 
                    CodeCapteur = l.code_capteur
                };

                listCapteur.Add(cap);
            }

            return listCapteur;
        }

        public List<equipement> ObtListEquipements()
        {
            return contextDb.equipement.Where(e => e.equip_delete != true).ToList();
        }

        public bool AjouterCapteur(string NomCapteur, string CodeCapteur, int SelectedPiloteID, DateTime DateProchaineVerifInt, DateTime DateProchaineVerifExt,
                    DateTime DateDerniereVerifInt, DateTime DateDerniereVerifExt, double periodInt, double periodExt, bool CapteurConforme, 
                    double EmtCapteur, double FacteurCorrectif, string unite, string comment)
        {
            capteur capt = new capteur();
            equipement equip = contextDb.equipement.First(e=>e.id == SelectedPiloteID);
            if(DateDerniereVerifInt.Year == 0001)
            {
                DateDerniereVerifInt = DateDerniereVerifInt.AddYears(1999); // SQL accepte des dates à partir de l'année 1753 sinon on a une erreur,
                                                                    // mais j'ajoute quelques années car sinon c'est chiant à choisir sur le calendrier à partir de 1700 et quelque
            }
            if (DateDerniereVerifExt.Year == 0001)
            {
                DateDerniereVerifExt = DateDerniereVerifExt.AddYears(1999); // SQL accepte des dates à partir de l'année 1753 sinon on a une erreur,
                                                                            // mais j'ajoute quelques années car sinon c'est chiant à choisir sur le calendrier à partir de 1700 et quelque
            }
            if (DateProchaineVerifInt.Year == 0001)
            {
                DateProchaineVerifInt = DateProchaineVerifInt.AddYears(1999); // SQL accepte des dates à partir de l'année 1753 sinon on a une erreur,
                                                                            // mais j'ajoute quelques années car sinon c'est chiant à choisir sur le calendrier à partir de 1700 et quelque
            }
            if (DateProchaineVerifExt.Year == 0001)
            {
                DateProchaineVerifExt = DateProchaineVerifExt.AddYears(1999); // SQL accepte des dates à partir de l'année 1753 sinon on a une erreur,
                                                                            // mais j'ajoute quelques années car sinon c'est chiant à choisir sur le calendrier à partir de 1700 et quelque
            }

            capt = new capteur
            {
                nom_capteur = NomCapteur,
                code_capteur = CodeCapteur,
                equipementID = SelectedPiloteID,
                date_prochaine_verif_int = DateProchaineVerifInt,
                date_prochaine_verif_ext = DateProchaineVerifExt,
                date_derniere_verif_ext = DateDerniereVerifExt,
                date_derniere_verif_int = DateDerniereVerifInt,
                periodicite_metrologie_int = periodInt,
                periodicite_metrologie_ext = periodExt,
                capteur_conforme = CapteurConforme,
                emt_capteur = EmtCapteur,
                facteur_correctif = FacteurCorrectif, 
                unite_mesure = unite,
                commentaire = comment
            };

            try
            {
                contextDb.capteur.Add(capt);
                contextDb.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                logger.LogError("", "problème lors de la sauvegarde du capteur: " + e.ToString());
                return false;
            }
        }

        public capteur ObtenirCapteur(int id)
        {
            return contextDb.capteur.First(c => c.id == id);
        }

        public bool SupprimerCapteur(int idCapteur)
        {
            bool isOk = false;
            capteur capt = new capteur();
            try
            {
                capt = contextDb.capteur.First(d => d.id == idCapteur);
                contextDb.capteur.Remove(capt);
                contextDb.SaveChanges();
                isOk = true;
            }
            catch (Exception e)
            {
                isOk = false;
                logger.LogError("", "Problème pour supprimer le capteur: " + capt.nom_capteur + "Erreur: " + e.ToString());
            }

            return isOk;
        }

        public equipement ObtenirEquipement(int idEquipement)
        {
            return contextDb.equipement.First(e => e.id == idEquipement);
        }

        public bool UpdatePeriodiciteInt(capteur capt, double periodicite)
        {
            try
            {
                capt.periodicite_metrologie_int = periodicite;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ de la periodicité interne d'un capteur");
                return false;
            }
            return true;
        }

        public bool UpdatePeriodiciteExt(capteur capt, double periodicite)
        {
            try
            {
                capt.periodicite_metrologie_ext = periodicite;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ de la periodicité externe d'un capteur");
                return false;
            }
            return true;
        }

        public bool UpdateDateProVerifInt(capteur capt, DateTime dateverif)
        {
            try
            {
                capt.date_prochaine_verif_int = dateverif;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ de la date prochaine vérification interne");
                return false;
            }
            return true;
        }

        public bool UpdateDateProVerifExt(capteur capt, DateTime dateverif)
        {
            try
            {
                capt.date_prochaine_verif_ext = dateverif;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ de la date prochaine vérification externe");
                return false;
            }
            return true;
        }

        public bool UpdateDateDerniereVerifInt(capteur capt, DateTime dateverif)
        {
            try
            {
                capt.date_derniere_verif_int = dateverif;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ de la date dernière vérification interne");
                return false;
            }
            return true;
        }

        public bool UpdateDateDerniereVerifExt(capteur capt, DateTime dateverif)
        {
            try
            {
                capt.date_derniere_verif_ext = dateverif;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ de la date dernière vérification externe");
                return false;
            }
            return true;
        }

        public bool UpdateFacteur(capteur capt, double facteur)
        {
            try
            {
                capt.facteur_correctif = facteur;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ du facteur de correction");
                return false;
            }
            return true;
        }

        public bool UpdateEMT(capteur capt, double emt)
        {
            try
            {
                capt.emt_capteur = emt;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ de l'EMT capteur");
                return false;
            }
            return true;
        }

        public bool UpdateCodeCapteur(capteur capt, string code)
        {
            try
            {
                capt.code_capteur = code;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ du code capteur");
                return false;
            }
            return true;
        }

       public bool UpdateNomCapteur(capteur capt, string nom)
        {
            try
            {
                capt.nom_capteur = nom;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ du nom capteur");
                return false;
            }
            return true;
        }

        public bool UpdateConformite(capteur capt, bool conformite)
        {
            try
            {
                capt.capteur_conforme = conformite;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ conformité capteur");
                return false;
            }
            return true;
        }

        public double FacteurCorrectif(capteur capt)
        {
            return contextDb.capteur.First(c => c.id == capt.id).facteur_correctif.Value;
        }

        public bool UpdateUnite(capteur capt, string unite)
        {
            try
            {
                capt.unite_mesure = unite;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ unité capteur");
                return false;
            }
            return true;
        }

        public bool UpdateCommentaire(capteur capt, string comment)
        {
            try
            {
                capt.commentaire = comment;
                contextDb.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ commentaire capteur");
                return false;
            }
            return true;
        }
    }
}
