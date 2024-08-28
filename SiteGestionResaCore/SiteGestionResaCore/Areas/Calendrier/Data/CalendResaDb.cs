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

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Areas.Reservation.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using SiteGestionResaCore.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    public class CalendResaDb: ICalendResaDb
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private readonly GestionResaContext resaDB;
        private readonly ILogger<CalendResaDb> logger;

        public CalendResaDb(
            GestionResaContext resaDB,
            ILogger<CalendResaDb> logger)
        {
            this.resaDB = resaDB;
            this.logger = logger;
        }

        public List<zone> ListeZones()
        {
            return resaDB.zone.ToList();
        }

        public List<equipement> ListeEquipements(int ZoneID)
        {
            return resaDB.equipement.Where(e => e.zoneID == ZoneID && e.equip_delete != true).Distinct().ToList();
        }

        public ResasEquipParJour ResasEquipementParJour(int IdEquipement, DateTime DateRecup)
        {
            #region Variables pour les essais

            ResasEquipParJour resasEquipTEMP = new ResasEquipParJour();
            ResasEquipParJour resasEquipsADJ = new ResasEquipParJour(); // Equipements non reservés qui sont dans une zone affectée par une réservation restreint (non réservés)
            ResasEquipParJour resasEquip = new ResasEquipParJour();
            DateTimeFormatInfo dateTimeFormats = null;
            reservation_projet ResaAGarder = new reservation_projet();     // On garde une des réservations de côté (peu importe laquelle car on a juste besoin d'accèder aux infos "essai")

            DateTime JourCalendrier;
            string NomJour;

            //essai[] SubInfosEssai = new essai[] { };
            List<essai> InfosEssai = new List<essai>();
            equipement Equipement = resaDB.equipement.First(x => x.id == IdEquipement);     // Equipement à enqueter

            // CORRECTION: initialisation des dateTime pour trouver les réservations se chevauchant (4 dates differentes)
            DateTime DatEnqDebMatin = DateRecup; // date debut matin
            DateTime DatEnqDebAprem = new DateTime(DateRecup.Year, DateRecup.Month, DateRecup.Day, 13, 0, 0, DateTimeKind.Local); // date début aprèm
            DateTime DatEnqFinMatin = new DateTime(DateRecup.Year, DateRecup.Month, DateRecup.Day, 12, 0, 0, DateTimeKind.Local); // date fin matin
            DateTime DatEnqFinAprem = new DateTime(DateRecup.Year, DateRecup.Month, DateRecup.Day, 18, 0, 0, DateTimeKind.Local); // date fin aprèm 

            // Si jour égal à samedi ou dimanche pas besoin de appliquer toute la méthode de recherche!
            // Traduire le nom du jour en cours de l'anglais au Français
            dateTimeFormats = new CultureInfo("fr-FR").DateTimeFormat;
            string jourName = DateRecup.ToString("dddd", dateTimeFormats);

            #endregion

            #region Variables pour les Interventions

            List<maintenance> InfosInterv = new List<maintenance>();

            #endregion

            #region Liste des essais uniquement pour la date en question

            if (jourName == "samedi" || jourName == "dimanche")
                goto ENDT;

            // Récupérer toutes les réservations validés ou en attente valid 
            InfosEssai = (from resa in resaDB.reservation_projet
                             from essa in resaDB.essai
                             where resa.essaiID == essa.id &&
                             (essa.status_essai == EnumStatusEssai.Validate.ToString() ||
                             essa.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                             && ((DatEnqDebMatin >= resa.date_debut || DatEnqDebAprem >= resa.date_debut) &&
                                (DatEnqFinMatin <= resa.date_fin || DatEnqFinAprem <= resa.date_fin))
                             select essa).Distinct().ToList();

            // Récupérer les essais où la date enquêté est bien dans la plage de déroulement
            /*foreach (var es in SubInfosEssai)
            {
                var res = resaDB.reservation_projet.Where(r => r.essaiID == es.id).ToList();
                foreach (var resEs in res)
                {
                    if ((DatEnqDebMatin >= resEs.date_debut || DatEnqDebAprem >= resEs.date_debut) &&
                        (DatEnqFinMatin <= resEs.date_fin || DatEnqFinAprem <= resEs.date_fin))
                    {
                        InfosEssai.Add(es);
                        break;
                    }
                }
            }*/

            foreach (var ess in InfosEssai)
            {
                InfosAffichageResa infosResa = new InfosAffichageResa
                {
                    IdEssai = ess.id,
                    StatusEssai = ess.status_essai,
                    Confidentialite = ess.confidentialite
                };
                
                switch (ess.confidentialite)
                {
                    case "Ouvert": // Dans ce cas: je regarde si mon équipement est concerné par cet essai et je bloque uniquement l'équipement

                        #region Confidentialité ouverte

                        resasEquipTEMP = ResaConfidentialiteOuverte(ess, infosResa, IdEquipement, DateRecup);

                        #endregion

                        break;
                    case "Restreint": // si "Restreint" il faut bloquer toute la zone, alors vérifier si les réservations sont dans la même zone que l'équipement et bloquer l'équipement 

                        #region Confidentialité "Restreint"
                        // séparer les équipements qui sont bloqués, des équipements qui sont dans la même zone et qui ne sont pas bloqués
                        resasEquipTEMP = ResaConfidentialiteRestreintEq(ess, infosResa, Equipement, DateRecup); // Bloque l'équipement qui est réservé en mode restreint
                        resasEquipsADJ = ResaConfidentialiteRestreintAdj(ess, infosResa, Equipement, DateRecup); // Bloque les equipements qui sont dans la zone restreint

                        #endregion

                        break;
                    case "Confidentiel": // Blocage de toute la plateforme sauf pour les salles alimentaires (5 zones) et la zone equipements mobiles

                        #region Confidentialité "confidentiel" 

                        if (Equipement.zoneID.Equals((int)EnumZonesPfl.SalleAp7A) || Equipement.zoneID.Equals((int)EnumZonesPfl.SalleAp7B) ||
                            Equipement.zoneID.Equals((int)EnumZonesPfl.SalleAp7C) || Equipement.zoneID.Equals((int)EnumZonesPfl.SalleAp5) ||
                            Equipement.zoneID.Equals((int)EnumZonesPfl.SalleAp6) || Equipement.zoneID.Equals((int)EnumZonesPfl.SalleAp8) ||
                            Equipement.zoneID.Equals((int)EnumZonesPfl.SalleAp9)) //TODO:  la zone equipements mobiles devrait être bloqué?
                        {
                            // Pour ces zones alors faire comment on fait pour les essai du type "Restreint" blocage uniquement de la zone "Confidentiel"
                            // séparer les équipements qui sont bloqués, des équipements qui sont dans la même zone et qui ne sont pas bloqués
                            resasEquipTEMP = ResaConfidentialiteRestreintEq(ess, infosResa, Equipement, DateRecup); // Bloque l'équipement qui est réservé en mode restreint
                            resasEquipsADJ = ResaConfidentialiteRestreintAdj(ess, infosResa, Equipement, DateRecup); // Bloque les equipements qui sont dans la zone restreint
                        }
                        else if (Equipement.zoneID.Equals((int)EnumZonesPfl.EquipMobiles)) // Cette zone n'est pas bloqué comme les autres, on bloque uniquement l'équipement peu importe la confidentialité
                        {
                            resasEquipTEMP = ResaConfidentialiteOuverte(ess, infosResa, IdEquipement, DateRecup);
                        }
                        else // Si équipement présent dans la zone PFL alors le bloquer selon la réservation
                        {
                            resasEquipTEMP = ResaConfidentialiteConf(ess, infosResa, Equipement, DateRecup);
                        }
                        #endregion
                        break;
                }
                // Stocker les valeurs rétrouvés pour cet essai 
                foreach (var res in resasEquipTEMP.ListResasMatin)
                {
                    resasEquip.ListResasMatin.Add(res);
                }
                // Stocker les valeurs rétrouvés pour cet essai 
                foreach (var res in resasEquipTEMP.ListResasAprem)
                {
                    resasEquip.ListResasAprem.Add(res);
                }

                // Stocker les valeurs rétrouvés pour cet essai des équipements dans une zone restreint
                foreach (var res in resasEquipsADJ.ListResasMatinAdjRest)
                {
                    if (!resasEquip.ListResasMatinAdjRest.Contains(res))
                    {
                        resasEquip.ListResasMatinAdjRest.Add(res);
                    }
                }
                // Stocker les valeurs rétrouvés pour cet essai 
                foreach (var res in resasEquipsADJ.ListResasApremAdjRest)
                {
                    if (!resasEquip.ListResasApremAdjRest.Contains(res))
                    {
                        resasEquip.ListResasApremAdjRest.Add(res);
                    }                   
                }
            }

            #endregion

            #region Informations interventions maintenance (bloquer toute la zone pour tous les types d'interventions) 

            InfosInterv = (from interMaint in resaDB.reservation_maintenance
                          from maint in resaDB.maintenance
                          where (interMaint.maintenanceID == maint.id)
                          && (maint.maintenance_supprime != true)
                          && ((DatEnqDebMatin >= interMaint.date_debut || DatEnqDebAprem >= interMaint.date_debut)
                          && (DatEnqFinMatin <= interMaint.date_fin || DatEnqFinAprem <= interMaint.date_fin))
                          select maint).Distinct().ToList();


            // Récupérer les essais où la date enquêté est bien dans la plage de déroulement
            /*foreach (var maint in maints)
            {
                var resaMaint = resaDB.reservation_maintenance.Where(r => r.maintenanceID == maint.id).ToList();
                foreach (var resEs in resaMaint)
                {
                    if ((DatEnqDebMatin >= resEs.date_debut || DatEnqDebAprem >= resEs.date_debut) &&
                        (DatEnqFinMatin <= resEs.date_fin || DatEnqFinAprem <= resEs.date_fin))
                    {
                        InfosInterv.Add(maint);
                        break;
                    }
                }
            }*/

            foreach(var m in InfosInterv)
            {
                InfosAffichageMaint affichageMaint = new InfosAffichageMaint
                {
                    IdMaint = m.id,
                    CodeOperation = m.code_operation,
                    DescriptionOperation = m.description_operation,
                    TypeMaintenance = m.type_maintenance
                };

                #region Determiner les créneaux des interventions selon leur type

                switch(m.type_maintenance)
                {
                    case "Equipement en panne (blocage équipement)":
                    case "Maintenance curative (Dépannage sans blocage zone)":
                    case "Maintenance préventive (Interne sans blocage de zone)":
                    case "Maintenance préventive (Externe sans blocage de zone)":
                    case "Amélioration (sans blocage de zone)":
                        resasEquipTEMP = IntervEquipParJourEquipement(m, affichageMaint, Equipement, DateRecup);
                        break;
                    case "Maintenance curative (Dépannage avec blocage de zone)":
                    case "Maintenance préventive (Interne avec blocage de zone)":
                    case "Maintenance préventive (Externe avec blocage de zone)":
                    case "Amélioration (avec blocage de zone)":
                        resasEquipTEMP = IntervEquipParJourZone(m, affichageMaint, Equipement, DateRecup);
                        break;
                }

                //resasEquipTEMP = IntervEquipParJourZone(m, affichageMaint, Equipement, DateRecup);

                // Stocker les valeurs rétrouves pour cet essai 
                foreach (var intMatin in resasEquipTEMP.InfosIntervMatin)
                {
                    resasEquip.InfosIntervMatin.Add(intMatin);
                }
                // Stocker les valeurs rétrouves pour cet essai 
                foreach (var intAprem in resasEquipTEMP.InfosIntervAprem)
                {
                    resasEquip.InfosIntervAprem.Add(intAprem);
                }
                #endregion
            }

        #endregion

            #region Gestion nom du jour et couleurs pour l'affichage

            ENDT:
                // Obtenir le nom du jour 
                JourCalendrier = DateRecup; // enregistrer la date en question
                NomJour = DateRecup.ToString("dddd", dateTimeFormats); // Reecrire le nom du jour car lors de l'appel de la méthode ResaConfidentialiteOuverte()
                // Resas est réinitialisé!
                // TODO: Requete vers la base de données pour obtenir toutes les réservations du type "maintenance" 
                // TODO: Requete vers la base de données pour obtenir toutes les réservations du type "métrologie" 

                if (NomJour != "samedi" && NomJour != "dimanche")
                {
                    // si un équipement reservé le matin
                    if (resasEquip.ListResasMatinAdjRest.Count() == 0 && resasEquip.ListResasMatin.Count() == 1)
                    {
                        if (resasEquip.ListResasMatin[0].StatusEssai == EnumStatusEssai.Validate.ToString())
                            resasEquip.CouleurMatin = "#fdc0be"; // rouge (validée et occupée)
                        else if (resasEquip.ListResasMatin[0].StatusEssai == EnumStatusEssai.WaitingValidation.ToString())
                            resasEquip.CouleurMatin = "#fbeed9";  // Couleur beige pour indiquer que la réservation est en attente
                    }

                    // si un equipement reservé l'aprèm
                    if (resasEquip.ListResasApremAdjRest.Count() == 0 && resasEquip.ListResasAprem.Count() == 1)
                    {
                        if (resasEquip.ListResasAprem[0].StatusEssai == EnumStatusEssai.Validate.ToString())
                            resasEquip.CouleurAprem = "#fdc0be"; // rouge (validée et occupée)
                        else if (resasEquip.ListResasAprem[0].StatusEssai == EnumStatusEssai.WaitingValidation.ToString())
                            resasEquip.CouleurAprem = "#fbeed9";  // Couleur beige pour indiquer que la réservation est en attente
                    }
   
                    // plus d'un équipement dans la zone restreint alors conflit
                    if (resasEquip.ListResasMatinAdjRest.Count() > 1 && resasEquip.ListResasMatin.Count() == 0 ) // équipements dans une zone restreint et sans autre réservation
                        resasEquip.CouleurMatin = "#ffd191"; // Indiquer un chevauchement des créneaux réservation (orange)

                    if (resasEquip.ListResasApremAdjRest.Count() > 1 && resasEquip.ListResasAprem.Count() == 0)
                        resasEquip.CouleurAprem = "#ffd191"; // Indiquer un chevauchement des créneaux réservation (orange)


                    // MATIN: si plus d'un équipement en zone restreint à ce jour et un équipement qui est réservé et qui est dans la zone alors priorité à l'équipement reservé
                    if (resasEquip.ListResasMatinAdjRest.Count() > 1 && resasEquip.ListResasMatin.Count() == 1) // équipements dans une zone restreint
                    {
                        if (resasEquip.ListResasMatin[0].StatusEssai == EnumStatusEssai.Validate.ToString())
                            resasEquip.CouleurMatin = "#fdc0be"; // rouge (validée et occupée)
                        else if (resasEquip.ListResasMatin[0].StatusEssai == EnumStatusEssai.WaitingValidation.ToString())
                            resasEquip.CouleurMatin = "#fbeed9"; // attente de validation
                    }

                    // APREM: si plus d'un équipement en zone restreint à ce jour et un équipement qui est réservé et qui est dans la zone alors priorité à l'équipement reservé
                    if (resasEquip.ListResasApremAdjRest.Count() > 1 && resasEquip.ListResasAprem.Count() == 1) // équipements dans une zone restreint
                    {
                        if (resasEquip.ListResasAprem[0].StatusEssai == EnumStatusEssai.Validate.ToString())
                            resasEquip.CouleurAprem = "#fdc0be"; // rouge (validée et occupée)
                        else if (resasEquip.ListResasAprem[0].StatusEssai == EnumStatusEssai.WaitingValidation.ToString())
                            resasEquip.CouleurAprem = "#fbeed9"; // attente de validation
                    }

                    if (resasEquip.ListResasMatinAdjRest.Count() == 1 && resasEquip.ListResasMatin.Count() == 0)                
                        resasEquip.CouleurMatin = "lightgray";  // Couleur gris pour indiquer que l'équipement est dans une zone restreint le matin
                    // si pas de chevauchement des résas alors vérifier le status projet
                    if (resasEquip.ListResasApremAdjRest.Count() == 1 && resasEquip.ListResasAprem.Count() == 0)
                        resasEquip.CouleurAprem = "lightgray";  // Couleur gris pour indiquer que l'équipement est dans une zone restreint l'aprèm

                    if (resasEquip.ListResasMatinAdjRest.Count() == 1 && resasEquip.ListResasMatin.Count() == 1)
                    {
                        if (resasEquip.ListResasMatin[0].StatusEssai == EnumStatusEssai.Validate.ToString())
                            resasEquip.CouleurMatin = "#fdc0be"; // rouge (validée et occupée)
                        else if (resasEquip.ListResasMatin[0].StatusEssai == EnumStatusEssai.WaitingValidation.ToString())
                            resasEquip.CouleurMatin = "#fbeed9"; // attente de validation
                    }
                    
                    if (resasEquip.ListResasApremAdjRest.Count() == 1 && resasEquip.ListResasAprem.Count() == 1)
                    {
                        if (resasEquip.ListResasAprem[0].StatusEssai == EnumStatusEssai.Validate.ToString())
                            resasEquip.CouleurAprem = "#fdc0be"; // rouge (validée et occupée)
                        else if (resasEquip.ListResasAprem[0].StatusEssai == EnumStatusEssai.WaitingValidation.ToString())
                            resasEquip.CouleurAprem = "#fbeed9"; // attente de validation
                    }


                    // Définition des couleurs maintenance (regne sur les essais) 
                    if (resasEquip.InfosIntervMatin.Count() == 1)
                    {
                        resasEquip.CouleurMatin = "#70cff0"; // blue opération maintenance en cours
                    }
                    if(resasEquip.InfosIntervAprem.Count() == 1)
                    {
                        resasEquip.CouleurAprem = "#70cff0"; // bleu opération maintenance en cours
                    }


                    // CODE COULEUR DISPO SUR: https://encycolorpedia.fr/
                    // Definir les couleurs de fond pour indiquer si le créneau est occupé ou pas
                    if (resasEquip.ListResasMatin.Count() == 0 && resasEquip.InfosIntervMatin.Count() == 0 && resasEquip.ListResasMatinAdjRest.Count() == 0) // si au moins une réservation le matin alors matinée occupée
                        resasEquip.CouleurMatin = "#c2e6e2"; // matin dispo (Vert)
                    if (resasEquip.ListResasAprem.Count() == 0 && resasEquip.InfosIntervAprem.Count() == 0 && resasEquip.ListResasApremAdjRest.Count() == 0) // si au moins une réservation l'aprèm alors aprèm occupée
                        resasEquip.CouleurAprem = "#c2e6e2"; // Aprèm libre (Vert)
                }
                else // si jour samedi ou dimanche alors mettre en fond gris
                {
                    resasEquip.CouleurMatin = "silver";
                    resasEquip.CouleurAprem = "silver";
                }

            #endregion
            resasEquip.IdEquipement = IdEquipement;
            resasEquip.Date = DateRecup;
            return resasEquip;
        }

        /// <summary>
        /// Ajout des réservations matin et aprèm s'il s'agit de la même zone
        /// </summary>
        /// <param name="maint"></param>
        /// <param name="infosAffichage"></param>
        /// <param name="Equipement"></param>
        /// <param name="DateRecup"></param>
        /// <returns></returns>
        public ResasEquipParJour IntervEquipParJourZone(maintenance maint, InfosAffichageMaint infosAffichage, equipement Equipement, DateTime DateRecup)
        {
            ResasEquipParJour EquipVsResa = new ResasEquipParJour();
            int EquipsMob = Convert.ToInt32(EnumZonesPfl.EquipMobiles);

            foreach (var resaInter in resaDB.reservation_maintenance.Where(r => r.maintenanceID == maint.id))
            {
                equipement equipementInterv = resaDB.equipement.Where(e => e.id == resaInter.equipementID).First();
                if(equipementInterv.zoneID.Value == Equipement.zoneID && Equipement.zoneID == EquipsMob) // si l'équipement objet du planning et l'équipement intervention sont dans la même zone equipement mobile 
                {
                    if(equipementInterv.id == Equipement.id)
                    {
                        // Bloquer l'équipement pour affichage calendrier
                        goto ACTION;
                    }
                    else
                    {
                        // pas d'action, on sauvegarde rien pour afficher sur le calendrier
                        goto ENDT;
                    }
                }
                if(equipementInterv.zoneID.Value == Equipement.zoneID && Equipement.zoneID != EquipsMob) // S'il s'agit d'une autre zone differente à la zone equipement mobile 
                {
                    // Bloquer l'équipement pour affichage calendrier 
                    goto ACTION;
                }
                else
                {
                    goto ENDT;
                }
            ACTION:
                    if ((DateTime.Parse(DateRecup.ToShortDateString()) >= DateTime.Parse(resaInter.date_debut.ToShortDateString()))
                        && (DateTime.Parse(DateRecup.ToShortDateString()) <= DateTime.Parse(resaInter.date_fin.ToShortDateString()))) // Si l'équipement à afficher est impliqué dans l'essai
                    {
                        // vérifier si l'essai n'est pas déjà dans la liste Matin
                        var EssaiDejaAjouteMatin = EquipVsResa.InfosIntervMatin.Any(e => e.IdMaint == maint.id);
                        var EssaiDejaAjouteAprem = EquipVsResa.InfosIntervAprem.Any(e => e.IdMaint == maint.id);

                        if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resaInter.date_debut.ToShortDateString())) // si dateResa égal à resa.date_debut
                        {
                            // Regarder pour définir le créneau
                            if (resaInter.date_debut.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                            {
                                if(!EssaiDejaAjouteAprem)
                                    EquipVsResa.InfosIntervAprem.Add(infosAffichage);
                                //Resas.InfosResaMatin.Add(null); // Matin vide
                            }
                            else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                // Vérifier si il s'agit d'une demi journée (juste l'aprèm)
                                if (resaInter.date_fin.Hour.Equals(12) && (resaInter.date_fin.ToShortDateString() == DateRecup.ToShortDateString())) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                {
                                    if (!EssaiDejaAjouteMatin)
                                        EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                                    //Resas.InfosResaMatin.Add(null); // Matin vide
                                }
                                else  // si l'heure de fin est 18h alors on rajoute sur les 2
                                {
                                    if (!EssaiDejaAjouteMatin)
                                        EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                                    if (!EssaiDejaAjouteAprem)
                                        EquipVsResa.InfosIntervAprem.Add(infosAffichage);
                                }
                            }
                        }
                        else if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resaInter.date_fin.ToShortDateString())) // si dateResa égal à resa.date_fin
                        {
                            // Regarder pour définir le créneau
                            if (resaInter.date_fin.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                            {
                                if (!EssaiDejaAjouteMatin)
                                    EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                            }
                            else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                if (!EssaiDejaAjouteMatin)
                                    EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                                if(!EssaiDejaAjouteAprem)
                                    EquipVsResa.InfosIntervAprem.Add(infosAffichage);
                            }
                        }
                        else // date à l'intérieur du seuil de réservation
                        {
                            // Ajouter cette résa sur le créneau matin et aprèm 
                            if (!EssaiDejaAjouteMatin)
                                EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                            if (!EssaiDejaAjouteAprem)
                                EquipVsResa.InfosIntervAprem.Add(infosAffichage);
                        }
                    }
                
            }
            ENDT:
            return EquipVsResa;
        }

        /// <summary>
        /// Ajout des réservations matin et aprèm s'il s'agit du même équipement
        /// </summary>
        /// <param name="maint"></param>
        /// <param name="infosAffichage"></param>
        /// <param name="Equipement"></param>
        /// <param name="DateRecup"></param>
        /// <returns></returns>
        public ResasEquipParJour IntervEquipParJourEquipement(maintenance maint, InfosAffichageMaint infosAffichage, equipement Equipement, DateTime DateRecup)
        {
            ResasEquipParJour EquipVsResa = new ResasEquipParJour();

            foreach (var resaInter in resaDB.reservation_maintenance.Where(r => r.maintenanceID == maint.id))
            {
                if (Equipement.id == resaInter.equipementID) // si l'équipement objet du "planning" est le même alors il devra être bloqué
                {
                    if ((DateTime.Parse(DateRecup.ToShortDateString()) >= DateTime.Parse(resaInter.date_debut.ToShortDateString()))
                        && (DateTime.Parse(DateRecup.ToShortDateString()) <= DateTime.Parse(resaInter.date_fin.ToShortDateString()))) // Si l'équipement à afficher est impliqué dans l'essai
                    {
                        // vérifier si l'essai n'est pas déjà dans la liste Matin
                        var EssaiDejaAjouteMatin = EquipVsResa.InfosIntervMatin.Any(e => e.IdMaint == maint.id);
                        var EssaiDejaAjouteAprem = EquipVsResa.InfosIntervAprem.Any(e => e.IdMaint == maint.id);

                        if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resaInter.date_debut.ToShortDateString())) // si dateResa égal à resa.date_debut
                        {
                            // Regarder pour définir le créneau
                            if (resaInter.date_debut.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                            {
                                if (!EssaiDejaAjouteAprem)
                                    EquipVsResa.InfosIntervAprem.Add(infosAffichage);
                                //Resas.InfosResaMatin.Add(null); // Matin vide
                            }
                            else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                // Vérifier si il s'agit d'une demi journée (juste l'aprèm)
                                if (resaInter.date_fin.Hour.Equals(12) && (resaInter.date_fin.ToShortDateString() == DateRecup.ToShortDateString())) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                {
                                    if (!EssaiDejaAjouteMatin)
                                        EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                                    //Resas.InfosResaMatin.Add(null); // Matin vide
                                }
                                else  // si l'heure de fin est 18h alors on rajoute sur les 2
                                {
                                    if (!EssaiDejaAjouteMatin)
                                        EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                                    if (!EssaiDejaAjouteAprem)
                                        EquipVsResa.InfosIntervAprem.Add(infosAffichage);
                                }
                            }
                        }
                        else if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resaInter.date_fin.ToShortDateString())) // si dateResa égal à resa.date_fin
                        {
                            // Regarder pour définir le créneau
                            if (resaInter.date_fin.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                            {
                                if (!EssaiDejaAjouteMatin)
                                    EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                            }
                            else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                if (!EssaiDejaAjouteMatin)
                                    EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                                if (!EssaiDejaAjouteAprem)
                                    EquipVsResa.InfosIntervAprem.Add(infosAffichage);
                            }
                        }
                        else // date à l'intérieur du seuil de réservation
                        {
                            // Ajouter cette résa sur le créneau matin et aprèm 
                            if (!EssaiDejaAjouteMatin)
                                EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                            if (!EssaiDejaAjouteAprem)
                                EquipVsResa.InfosIntervAprem.Add(infosAffichage);
                        }
                    }
                }
            }
            return EquipVsResa;
        }

        public ResasEquipParJour ResaConfidentialiteOuverte(essai ess, InfosAffichageResa infosResa, int IdEquipement, DateTime DateRecup)
        {
            ResasEquipParJour EquipVsResa = new ResasEquipParJour();

            foreach (var resa in resaDB.reservation_projet.Where(r => r.essaiID == ess.id))
            {
                if ((resa.equipementID == IdEquipement) && (DateTime.Parse(DateRecup.ToShortDateString()) >= DateTime.Parse(resa.date_debut.ToShortDateString()))
                    && (DateTime.Parse(DateRecup.ToShortDateString()) <= DateTime.Parse(resa.date_fin.ToShortDateString()))) // Si l'équipement à afficher est impliqué dans l'essai
                {
                    if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resa.date_debut.ToShortDateString())) // si dateResa égal à resa.date_debut
                    {
                        // Regarder pour définir le créneau
                        if (resa.date_debut.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                        {
                            EquipVsResa.ListResasAprem.Add(infosResa);
                            //Resas.InfosResaMatin.Add(null); // Matin vide
                        }
                        else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                        {
                            // Vérifier si il s'agit d'une demi journée (juste l'aprèm)
                            if (resa.date_fin.Hour.Equals(12) && (resa.date_fin.ToShortDateString() == DateRecup.ToShortDateString()) ) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                            {
                                EquipVsResa.ListResasMatin.Add(infosResa);
                                //Resas.InfosResaMatin.Add(null); // Matin vide
                            }
                            else  // si l'heure de fin est 18h alors on rajoute sur les 2
                            {
                                EquipVsResa.ListResasMatin.Add(infosResa);
                                EquipVsResa.ListResasAprem.Add(infosResa);
                            }
                        }
                    }
                    else if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resa.date_fin.ToShortDateString())) // si dateResa égal à resa.date_fin
                    {
                        // Regarder pour définir le créneau
                        if (resa.date_fin.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                        {
                            EquipVsResa.ListResasMatin.Add(infosResa);
                        }
                        else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                        {
                            EquipVsResa.ListResasMatin.Add(infosResa);
                            EquipVsResa.ListResasAprem.Add(infosResa);
                        }
                    }
                    else // date à l'intérieur du seuil de réservation
                    {
                        // Ajouter cette résa sur le créneau matin et aprèm 
                        EquipVsResa.ListResasMatin.Add(infosResa);
                        EquipVsResa.ListResasAprem.Add(infosResa);
                    }
                }
            }
            return EquipVsResa;
        }

        /// <summary>
        /// Methode pour retrouver l'équipement bloqué par réservation restreint
        /// </summary>
        /// <param name="ess"></param>
        /// <param name="infosResa"></param>
        /// <param name="Equipement"></param>
        /// <param name="DateRecup"></param>
        /// <returns></returns>
        public ResasEquipParJour ResaConfidentialiteRestreintEq(essai ess, InfosAffichageResa infosResa, equipement Equipement, DateTime DateRecup)
        {
            ResasEquipParJour EquipVsResa = new ResasEquipParJour();

            var resas = resaDB.reservation_projet.Where(r => r.essaiID == ess.id);

            foreach (var resa in resas)
            {
                if (resaDB.equipement.Where(e => e.id == resa.equipementID).First().id == Equipement.id) // si l'équipement objet du "planning" est déjà réservé restreint
                {
                    if ( DateTime.Parse(DateRecup.ToShortDateString()) >= DateTime.Parse(resa.date_debut.ToShortDateString()) 
                        && DateTime.Parse(DateRecup.ToShortDateString()) <= DateTime.Parse(resa.date_fin.ToShortDateString()) )
                    {
                        #region vérifier si l'essai n'est pas déjà dans la liste Matin
                        // vérifier si l'essai n'est pas déjà dans la liste Matin
                        var EssaiDejaAjouteMatin = EquipVsResa.ListResasMatin.Any(e => e.IdEssai == ess.id);
                        var EssaiDejaAjouteAprem = EquipVsResa.ListResasAprem.Any(e => e.IdEssai == ess.id);

                        if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resa.date_debut.ToShortDateString())) // début
                        {
                            // Regarder pour définir le créneau
                            if (resa.date_debut.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                            {
                                if (!EssaiDejaAjouteAprem)
                                {
                                    EquipVsResa.ListResasAprem.Add(infosResa);
                                }
                            }
                            else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                // Regarder pour définir le créneau
                                if (resa.date_fin.Hour.Equals(12) && (resa.date_fin.ToShortDateString() == DateRecup.ToShortDateString())) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                {
                                    if (!EssaiDejaAjouteMatin)
                                    {
                                        EquipVsResa.ListResasMatin.Add(infosResa);
                                    }
                                    //Resas.InfosResaMatin.Add(null); // Matin vide
                                }
                                else // si l'heure de fin est 18h alors on rajoute sur les 2
                                {
                                    if (!EssaiDejaAjouteMatin)
                                    {
                                        EquipVsResa.ListResasMatin.Add(infosResa);
                                    }
                                    if (!EssaiDejaAjouteAprem)
                                    {
                                        EquipVsResa.ListResasAprem.Add(infosResa);
                                    }
                                }
                            }
                        }
                        else if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resa.date_fin.ToShortDateString())) // fin
                        {
                            // Regarder pour définir le créneau
                            if (resa.date_fin.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                            {
                                if (!EssaiDejaAjouteMatin)
                                {
                                    EquipVsResa.ListResasMatin.Add(infosResa);
                                }
                            }
                            else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                if (!EssaiDejaAjouteMatin)
                                {
                                    EquipVsResa.ListResasMatin.Add(infosResa);
                                }
                                if (!EssaiDejaAjouteAprem)
                                {
                                    EquipVsResa.ListResasAprem.Add(infosResa);
                                }
                            }
                        }
                        else
                        {
                            // Ajouter cette résa sur le créneau matin et aprèm 
                            if (!EssaiDejaAjouteMatin)
                            {
                                EquipVsResa.ListResasMatin.Add(infosResa);
                            }
                            if (!EssaiDejaAjouteAprem)
                            {
                                EquipVsResa.ListResasAprem.Add(infosResa);
                            }
                        }
                        #endregion
                    }
                }
            }
            return EquipVsResa;
        }

        public ResasEquipParJour ResaConfidentialiteRestreintAdj(essai ess, InfosAffichageResa infosResa, equipement Equipement, DateTime DateRecup)
        {
            ResasEquipParJour EquipVsResa = new ResasEquipParJour();

            var resas = resaDB.reservation_projet.Where(r => r.essaiID == ess.id);

            foreach (var resa in resas)
            {
                if (resaDB.equipement.Where(e => e.id == resa.equipementID).First().zoneID.Value == Equipement.zoneID && 
                    (resaDB.equipement.Where(e => e.id == resa.equipementID).First().id != Equipement.id)) // si l'équipement objet du "planning" est dans la zone d'une réservation restreint
                {
                    if (DateTime.Parse(DateRecup.ToShortDateString()) >= DateTime.Parse(resa.date_debut.ToShortDateString())
                        && DateTime.Parse(DateRecup.ToShortDateString()) <= DateTime.Parse(resa.date_fin.ToShortDateString()))
                    {
                        #region vérifier si l'essai n'est pas déjà dans la liste Matin
                        // vérifier si l'essai n'est pas déjà dans la liste Matin
                        var EssaiDejaAjouteMatin = EquipVsResa.ListResasMatinAdjRest.Any(e => e.IdEssai == ess.id);
                        var EssaiDejaAjouteAprem = EquipVsResa.ListResasApremAdjRest.Any(e => e.IdEssai == ess.id);

                        if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resa.date_debut.ToShortDateString())) // début
                        {
                            // Regarder pour définir le créneau
                            if (resa.date_debut.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                            {
                                if (!EssaiDejaAjouteAprem)
                                {
                                    EquipVsResa.ListResasApremAdjRest.Add(infosResa);
                                }
                            }
                            else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                // Regarder pour définir le créneau
                                if (resa.date_fin.Hour.Equals(12) && (resa.date_fin.ToShortDateString() == DateRecup.ToShortDateString())) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                {
                                    if (!EssaiDejaAjouteMatin)
                                    {
                                        EquipVsResa.ListResasMatinAdjRest.Add(infosResa);
                                    }
                                    //Resas.InfosResaMatin.Add(null); // Matin vide
                                }
                                else // si l'heure de fin est 18h alors on rajoute sur les 2
                                {
                                    if (!EssaiDejaAjouteMatin)
                                    {
                                        EquipVsResa.ListResasMatinAdjRest.Add(infosResa);
                                    }
                                    if (!EssaiDejaAjouteAprem)
                                    {
                                        EquipVsResa.ListResasApremAdjRest.Add(infosResa);
                                    }
                                }
                            }
                        }
                        else if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resa.date_fin.ToShortDateString())) // fin
                        {
                            // Regarder pour définir le créneau
                            if (resa.date_fin.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                            {
                                if (!EssaiDejaAjouteMatin)
                                {
                                    EquipVsResa.ListResasMatinAdjRest.Add(infosResa);
                                }
                            }
                            else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                if (!EssaiDejaAjouteMatin)
                                {
                                    EquipVsResa.ListResasMatinAdjRest.Add(infosResa);
                                }
                                if (!EssaiDejaAjouteAprem)
                                {
                                    EquipVsResa.ListResasApremAdjRest.Add(infosResa);
                                }
                            }
                        }
                        else
                        {
                            // Ajouter cette résa sur le créneau matin et aprèm 
                            if (!EssaiDejaAjouteMatin)
                            {
                                EquipVsResa.ListResasMatinAdjRest.Add(infosResa);
                            }
                            if (!EssaiDejaAjouteAprem)
                            {
                                EquipVsResa.ListResasApremAdjRest.Add(infosResa);
                            }
                        }
                        #endregion
                    }
                }
            }
            return EquipVsResa;
        }

        /// <summary>
        /// Planning équipement si essai "Confidentiel"
        /// </summary>
        /// <param name="ess"></param>
        /// <param name="infosResa"></param>
        /// <param name="Equipement"></param>
        /// <param name="DateRecup"></param>
        /// <returns></returns>
        public ResasEquipParJour ResaConfidentialiteConf(essai ess, InfosAffichageResa infosResa, equipement Equipement, DateTime DateRecup)
        {
            ResasEquipParJour EquipVsResa = new ResasEquipParJour();
            int ApCinq = Convert.ToInt32(EnumZonesPfl.SalleAp5);
            int ApSix = Convert.ToInt32(EnumZonesPfl.SalleAp6);
            int ApSeptA = Convert.ToInt32(EnumZonesPfl.SalleAp7A);
            int ApSeptB = Convert.ToInt32(EnumZonesPfl.SalleAp7B);
            int ApSeptC = Convert.ToInt32(EnumZonesPfl.SalleAp7C);
            int ApHuit = Convert.ToInt32(EnumZonesPfl.SalleAp8);
            int ApNeuf = Convert.ToInt32(EnumZonesPfl.SalleAp9);
            var resas = resaDB.reservation_projet.Where(r => r.essaiID == ess.id);

            foreach (var resa in resas)
            {
                var equip = resaDB.equipement.First(e => e.id == resa.equipementID);
                // Equipement dans la zone PFL
                if (!equip.zoneID.Equals((int)EnumZonesPfl.SalleAp7A) && !equip.zoneID.Equals((int)EnumZonesPfl.SalleAp7B) && !equip.zoneID.Equals((int)EnumZonesPfl.SalleAp7C) &&
                    !equip.zoneID.Equals((int)EnumZonesPfl.SalleAp5) && !equip.zoneID.Equals((int)EnumZonesPfl.SalleAp6) && !equip.zoneID.Equals((int)EnumZonesPfl.SalleAp8) &&
                    !equip.zoneID.Equals((int)EnumZonesPfl.SalleAp9))
                {
                    if (DateTime.Parse(DateRecup.ToShortDateString()) >= DateTime.Parse(resa.date_debut.ToShortDateString())
                    && DateTime.Parse(DateRecup.ToShortDateString()) <= DateTime.Parse(resa.date_fin.ToShortDateString()))
                    {
                        // vérifier si l'essai n'est pas déjà dans la liste Matin
                        var EssaiDejaAjouteMatin = EquipVsResa.ListResasMatin.Any(e => e.IdEssai == ess.id);
                        var EssaiDejaAjouteAprem = EquipVsResa.ListResasAprem.Any(e => e.IdEssai == ess.id);

                        if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resa.date_debut.ToShortDateString())) // début
                        {
                            // Regarder pour définir le créneau
                            if (resa.date_debut.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                            {
                                if (!EssaiDejaAjouteAprem)
                                {
                                    EquipVsResa.ListResasAprem.Add(infosResa);
                                    //Resas.InfosResaMatin.Add(null); // Matin vide
                                }
                            }
                            else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                // Regarder pour définir le créneau
                                if (resa.date_fin.Hour.Equals(12) && (resa.date_fin.ToShortDateString() == DateRecup.ToShortDateString())) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                {
                                    if (!EssaiDejaAjouteMatin)
                                    {
                                        EquipVsResa.ListResasMatin.Add(infosResa);
                                    }
                                    //Resas.InfosResaMatin.Add(null); // Matin vide
                                }
                                else // si l'heure de fin est 18h alors on rajoute sur les 2
                                {
                                    if (!EssaiDejaAjouteMatin)
                                    {
                                        EquipVsResa.ListResasMatin.Add(infosResa);
                                    }
                                    if (!EssaiDejaAjouteAprem)
                                    {
                                        EquipVsResa.ListResasAprem.Add(infosResa);
                                    }
                                }
                            }
                        }
                        else if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resa.date_fin.ToShortDateString())) // fin
                        {
                            // Regarder pour définir le créneau
                            if (resa.date_fin.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                            {
                                if (!EssaiDejaAjouteMatin)
                                {
                                    EquipVsResa.ListResasMatin.Add(infosResa);
                                }
                            }
                            else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                if (!EssaiDejaAjouteMatin)
                                {
                                    EquipVsResa.ListResasMatin.Add(infosResa);
                                }
                                if (!EssaiDejaAjouteAprem)
                                {
                                    EquipVsResa.ListResasAprem.Add(infosResa);
                                }
                            }
                        }
                        else
                        {
                            // Ajouter cette résa sur le créneau matin et aprèm 
                            if (!EssaiDejaAjouteMatin)
                            {
                                EquipVsResa.ListResasMatin.Add(infosResa);
                            }
                            if (!EssaiDejaAjouteAprem)
                            {
                                EquipVsResa.ListResasAprem.Add(infosResa);
                            }
                        }
                    }
                }                           
            }
            return EquipVsResa;
        }

        public InfosEquipementReserve ObtenirInfosResa(int IdEssai)
        {
            InfosEquipementReserve Infos = new InfosEquipementReserve();
            essai essai = ObtenirEssai(IdEssai);

            projet pr = ObtenirProjetEssai(essai);
            // si essai confidentiel copier uniquement le mail du responsable projet
            if(essai.confidentialite == EnumConfidentialite.Confidentiel.ToString())
            {
                Infos = new InfosEquipementReserve { MailResponsablePj = pr.mailRespProjet, MailAuteurResa = resaDB.Users.Find(essai.compte_userID).Email, 
                    Confidentialite = essai.confidentialite, IdEssai = essai.id };
            }
            else
            {
                Infos = new InfosEquipementReserve { IdEssai = essai.id, MailAuteurResa = resaDB.Users.Find(essai.compte_userID).Email, 
                    MailResponsablePj = pr.mailRespProjet, NumeroProjet = pr.num_projet, TitreProjet = pr.titre_projet, Confidentialite = essai.confidentialite };
            }

            return Infos;
        }

        public InfosAffichageMaint ObtenirInfosInter(int IdMaint)
        {
            maintenance maint = resaDB.maintenance.First(m => m.id == IdMaint);
            InfosAffichageMaint infosMaint = new InfosAffichageMaint
            {
                IdMaint = IdMaint,
                CodeOperation = maint.code_operation,
                DescriptionOperation = maint.description_operation,
                MailOperateur = resaDB.Users.First(u=>u.Id == maint.userID).Email,
                TypeMaintenance = maint.type_maintenance
            };
            return infosMaint;
        }
        public essai ObtenirEssai(int IdEssai)
        {
            return resaDB.essai.First(e => e.id == IdEssai);
        }

        public projet ObtenirProjetEssai(essai essai)
        {
            return resaDB.projet.First(p => p.id == essai.projetID);
        }
    }
}
