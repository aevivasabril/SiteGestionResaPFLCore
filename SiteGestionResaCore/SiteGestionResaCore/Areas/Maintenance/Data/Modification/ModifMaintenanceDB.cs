﻿/*
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
using SiteGestionResaCore.Areas.Maintenance.Data.Maintenance;
using SiteGestionResaCore.Areas.Reservation.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Modification
{
    public class ModifMaintenanceDB: IModifMaintenanceDB
    {
        private readonly GestionResaContext context;
        private readonly ILogger<ModifMaintenanceDB> logger;
        private readonly UserManager<utilisateur> userManager;

        public ModifMaintenanceDB(
            GestionResaContext context,
            ILogger<ModifMaintenanceDB> logger,
            UserManager<utilisateur> userManager)
        {
            this.context = context;
            this.logger = logger;
            this.userManager = userManager;
        }
        /// <summary>
        /// Obtenir infos sur la maintenance à partir de son code operation INTERV00000
        /// </summary>
        /// <param name="CodeIntervention">code INTERV12345</param>
        /// <returns></returns>
        public MaintenanceInfos ObtenirInfosMaint(string CodeIntervention)
        {
            string MaintSupprimee = "";
            var mainte = context.maintenance.First(m => m.code_operation.Equals(CodeIntervention));

            if (mainte.maintenance_supprime == null)
                MaintSupprimee = "NON";
            else
                MaintSupprimee = "OUI";

            MaintenanceInfos infos = new MaintenanceInfos
            {
                IdMaint = mainte.id,
                NomIntervenantExt = mainte.nom_intervenant_ext,
                CodeOperation = mainte.code_operation,
                DescriptionOperation = mainte.description_operation,
                EmailAuteur = context.Users.First(u => u.Id == mainte.userID).Email,
                TypeMaintenance = mainte.type_maintenance,
                MaintenanceSupprime = MaintSupprimee
            };

            return infos;
        }

        /// <summary>
        /// Trouver l'intervention avec le code d'opération demandé mais il faut qu'elle ne soit pas supprimée
        /// </summary>
        /// <param name="CodeIntervention"></param>
        /// <returns></returns>
        public bool NumMaintExist(string CodeIntervention)
        {
            var mainte = context.maintenance.Where(m => m.code_operation == CodeIntervention).Where(s=>s.maintenance_supprime == null);
            if (mainte.Count() == 0)
                return false;
            else
                return true;
        }  

        /// <summary>
        /// Retourne la liste des équipements communs dont l'id maintenance est égal à IdMaint
        /// </summary>
        /// <param name="IdMaint"></param>
        /// <returns></returns>
        public List<EquipCommunXInterv> ListMaintAdj(int IdMaint)
        {
            List<EquipCommunXInterv> List = new List<EquipCommunXInterv>();
            bool finie = false;
            var listIntervCommun = context.resa_maint_equip_adjacent.Where(r => r.maintenanceID == IdMaint).ToList();

            // Voir si l'opération de maintenance est déjà fini
            /*var OpMaint = context.maintenance.First(m => m.id == IdMaint);
            if (OpMaint.maintenance_finie == false || OpMaint.maintenance_finie == null)
                finie = false;
            else
                finie = true;*/

            foreach (var maint in listIntervCommun)
            {
                if (maint.interv_fini == false || maint.interv_fini == null)
                    finie = false;
                else
                    finie = true;
                EquipCommunXInterv equipCommunXInterv = new EquipCommunXInterv
                {
                    DateDebut = maint.date_debut,
                    DateFin = maint.date_fin,
                    IdInterv = maint.id,
                    NomEquipement = maint.nom_equipement,
                    ZoneAffectee = maint.zone_affectee,
                    IsIntervFinie = finie
                };

                List.Add(equipCommunXInterv);
            }

            return List;
        }
        /// <summary>
        /// Liste des interventions par maintenance reservation_maintenance
        /// </summary>
        /// <param name="IdMaint">id maintenance</param>
        /// <returns></returns>
        public List<EquipPflXIntervention> ListIntervPFL(int IdMaint)
        {
            List<EquipPflXIntervention> List = new List<EquipPflXIntervention>();
            bool finie = false;
            var listMaintPfl = context.reservation_maintenance.Where(r => r.maintenanceID == IdMaint).ToList();

            // Voir si l'opération de maintenance est déjà fini
            /*var OpMaint = context.maintenance.First(m => m.id == IdMaint);
            if (OpMaint.maintenance_finie == false || OpMaint.maintenance_finie == null)
                finie = false;
            else
                finie = true;*/

            foreach (var maint in listMaintPfl)
            {
                if (maint.interv_fini == false || maint.interv_fini == null)
                    finie = false;
                else
                    finie = true;
                EquipPflXIntervention equipPflXIntervention = new EquipPflXIntervention 
                {
                    DateDebut = maint.date_debut,
                    DateFin = maint.date_fin,
                    Id = maint.id,
                    NomEquipement = context.equipement.First(e=>e.id == maint.equipementID).nom,
                    NumGMAO = context.equipement.First(e => e.id == maint.equipementID).numGmao,
                    IsIntervFinie = finie
                };

                List.Add(equipPflXIntervention);
            }
            return List;
        }

        /// <summary>
        /// Methode qui retourne l'ancienne date fin et met à jour l'intervention avec la nouvelle date 
        /// </summary>
        /// <param name="IdResaCommun"></param>
        /// <param name="NewDateFin"></param>
        /// <returns></returns>
        public DateTime ChangeDateFinEquipCommun(int IdResaCommun, DateTime NewDateFin)
        {
            resa_maint_equip_adjacent inter = context.resa_maint_equip_adjacent.First(r => r.id == IdResaCommun);
            var dateFin = inter.date_fin;
            inter.date_fin = NewDateFin;
            context.SaveChanges();

            return dateFin;
        }

        /// <summary>
        /// Obtenir liste des utilisateurs du site pour envoie des mails
        /// utilisateurs avec un compte actif!
        /// </summary>
        /// <returns></returns>
        public List<utilisateur> ObtenirListUtilisateursSite()
        {
            return context.Users.Where(u=>u.compteInactif != true).ToList();
        }

        /// <summary>
        /// Obtenir les infos maintenance à partir d'une des réservations sur un équipement commun
        /// </summary>
        /// <param name="IdresaEquipComm"></param>
        /// <returns></returns>
        public maintenance ObtenirMaintenanceXInterv(int IdresaEquipComm)
        {
            var resa = context.resa_maint_equip_adjacent.First(r => r.id == IdresaEquipComm);
            return context.maintenance.First(m => m.id == resa.maintenanceID);
        }

        /// <summary>
        /// Obtenir les interventions usr les équipements commun sur une maintenance
        /// </summary>
        /// <param name="IdresaEquipComm"> id resa équipement commun x intervention</param>
        /// <returns></returns>
        public resa_maint_equip_adjacent ObtenirIntervEquiComm(int IdresaEquipComm)
        {
            return context.resa_maint_equip_adjacent.First(r => r.id == IdresaEquipComm);
        }

        /// <summary>
        /// Obtenir intervention sur un équipement PFL à partir de son id
        /// </summary>
        /// <param name="IdresaEquipPfl"></param>
        /// <returns></returns>
        public reservation_maintenance ObtenirIntervEquipPfl(int IdresaEquipPfl)
        {
            return context.reservation_maintenance.First(r => r.id == IdresaEquipPfl);
        }

        /// <summary>
        /// Obtenir les infos maintenance à partir de l'id d'une intervention liée sur un des équipements pfl
        /// </summary>
        /// <param name="IdresaEquipPfl"></param>
        /// <returns></returns>
        public maintenance ObtenirMaintenanceXIntervPFl(int IdresaEquipPfl)
        {
            var resa = context.reservation_maintenance.First(r => r.id == IdresaEquipPfl);
            return context.maintenance.First(m => m.id == resa.maintenanceID);
        }

        /// <summary>
        /// Obtenir mail utilisateur propietaire d'un essai
        /// </summary>
        /// <param name="iduser"></param>
        /// <returns></returns>
        public string ObtenirMailUser(int iduser)
        {
            return context.Users.First(i => i.Id == iduser).Email;
        }

        /// <summary>
        /// Liste des utilisateurs dont le role égal logistic
        /// </summary>
        /// <returns></returns>
        public async Task<IList<utilisateur>> List_utilisateurs_logistiqueAsync()
        {
            return await userManager.GetUsersInRoleAsync("Logistic");
        }

        /// <summary>
        /// Changer la date fin pour une intervetion équipement PFL
        /// </summary>
        /// <param name="IdResaPfl"></param>
        /// <param name="NewDateFin"></param>
        public void ChangeDateFinEquipPFL(int IdResaPfl, DateTime NewDateFin)
        {
            reservation_maintenance resa = context.reservation_maintenance.First(r => r.id == IdResaPfl);
            resa.date_fin = NewDateFin;
            context.SaveChanges();
        }

        /// <summary>
        /// Vérifier s'une zone est dispo pour modification des date fin!
        /// toutes les interventions sauf les pannes (maintenance curative)
        /// </summary>
        /// <param name="dateDebut"></param>
        /// <param name="dateFin"></param>
        /// <param name="idEquipement"></param>
        /// <param name="Idmaintenance"></param>
        /// <returns></returns>
        public bool ModifZoneDisponibleXIntervention(DateTime dateFin, reservation_maintenance interv)
        {
            bool resaOk = false;
            bool interOk = false;

            // Récupérer l'id zone pour l'équipement enquêté
            var zon = (from equip in context.equipement
                       where equip.id == interv.equipementID
                       select equip.zoneID.Value).First();

            #region disponibilité sur les essais

            // Vérifier la disponibilité sur les essais qu'ont au moins un materiel dans la même zone
            // J'ai enlevé la condition confidentiel car l'essai peut se derouler sauf si ça touche la même zone de mon intervention ou le même équipement
            var resas = (from essai in context.essai
                            from resa in context.reservation_projet
                            from equip in context.equipement
                            where essai.id == resa.essaiID
                            && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                            && (resa.equipement.zoneID == zon) 
                            && (((interv.date_debut >= resa.date_debut) || dateFin >= resa.date_debut)
                            && ((interv.date_debut <= resa.date_fin) || dateFin <= resa.date_fin))
                            select essai).Distinct().ToList();

            if (resas.Count() == 0)
                resaOk = true;           

            #endregion

            #region disponibilité sur les interventions

            // Vérifier qu'il n'y a pas autre maintenance sur ces dates et sur la même zone
            // vérifier qu'il ne s'agit pas de la même opération de maintenance
            var IntervZon = (from maint in context.maintenance
                             from resaMaint in context.reservation_maintenance
                             from equip in context.equipement
                             where maint.id == resaMaint.maintenanceID
                             && (maint.maintenance_supprime != true)
                             && (resaMaint.equipement.zoneID == zon) && (maint.id != interv.maintenanceID)
                             && (((interv.date_debut >= resaMaint.date_debut) || dateFin >= resaMaint.date_debut)
                             && ((interv.date_debut <= resaMaint.date_fin) || dateFin <= resaMaint.date_fin))
                             select maint).Distinct().ToList();

            if (IntervZon.Count() == 0)
                interOk = true;

            #endregion

            return (resaOk && interOk);
        }

        /// <summary>
        /// Vérifier si un équipement est disponible 
        /// </summary>
        /// <param name="dateDebut"></param>
        /// <param name="dateFin"></param>
        /// <param name="idEquipement"></param>
        /// <param name="Idmaintenance"></param>
        /// <returns></returns>
        public bool ModifEquipementDisponibleXIntervention(DateTime dateFin, reservation_maintenance interv)
        {
            bool resaOk = false;
            bool interOk = false;

            // Récupérer l'id zone pour l'équipement enquêté
            var zon = (from equip in context.equipement
                       where equip.id == interv.equipementID
                       select equip.zoneID.Value).First();

            #region disponibilité sur les essais

            var resas = (from essai in context.essai
                         from resa in context.reservation_projet
                         from equip in context.equipement
                         where essai.id == resa.essaiID
                         && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                             essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                         && (resa.equipementID == interv.equipementID)
                         && (((interv.date_debut >= resa.date_debut) || dateFin >= resa.date_debut)
                         && ((interv.date_debut <= resa.date_fin) || dateFin <= resa.date_fin))
                         select essai).Distinct().ToList();

            if (resas.Count() == 0)
                resaOk = true;

            #endregion

            #region disponibilité sur les interventions

            // Vérifier qu'il y a pas autre maintenance (non supprimée) sur ces dates et sur la même zone ou sur le même équipement

            var IntervEquip = (from maint in context.maintenance
                               from resaMaint in context.reservation_maintenance
                               from equip in context.equipement
                               where maint.id == resaMaint.maintenanceID
                               && (maint.maintenance_supprime != true) && (maint.id != interv.maintenanceID)
                               && ((maint.type_maintenance == "Equipement en panne (blocage équipement)")
                               || (maint.type_maintenance == "Maintenance curative (Dépannage sans blocage zone)")
                               || (maint.type_maintenance == "Maintenance préventive(Interne sans blocage de zone)")
                               || (maint.type_maintenance == "Maintenance préventive (Externe sans blocage de zone)")
                               || (maint.type_maintenance == "Amélioration (sans blocage de zone)"))
                               && (resaMaint.equipementID == interv.equipementID)
                               && (((interv.date_debut >= resaMaint.date_debut) || dateFin >= resaMaint.date_debut)
                               && ((interv.date_debut <= resaMaint.date_fin) || dateFin <= resaMaint.date_fin))
                               select maint).Distinct().ToList();

            if (IntervEquip.Count() == 0)
            {
                interOk = true;
                // uniquement la zone bloqué
                // "Maintenance curative (Dépannage)"
                // "Maintenance préventive (Interne)"
                // "Maintenance préventive (Externe)"
                // "Amélioration"
                var IntervZone = (from maint in context.maintenance
                                  from resaMaint in context.reservation_maintenance
                                  from equip in context.equipement
                                  where maint.id == resaMaint.maintenanceID
                                  && (maint.maintenance_supprime != true) && (maint.id != interv.maintenanceID)
                                  && ((maint.type_maintenance == "Maintenance curative (Dépannage avec blocage de zone)")
                                  || (maint.type_maintenance == "Maintenance préventive (Interne avec blocage de zone)")
                                  || (maint.type_maintenance == "Maintenance préventive (Externe avec blocage de zone)")
                                  || (maint.type_maintenance == "Amélioration (avec blocage de zone)"))
                                  && (resaMaint.equipement.zoneID == zon)
                                  && (((interv.date_debut >= resaMaint.date_debut) || dateFin >= resaMaint.date_debut)
                                  && ((interv.date_debut <= resaMaint.date_fin) || dateFin <= resaMaint.date_fin))
                                  select maint).Distinct().ToList();
                if (IntervZone.Count() == 0)
                {
                    interOk = true;
                    goto ENDT;
                }
                else
                {
                    interOk = false;
                    goto ENDT;
                }
            }
            else
            {
                interOk = false;
                goto ENDT;
            }

        #endregion

        ENDT:
            return (resaOk && interOk);
        }

        /// <summary>
        /// Méthode pour annuler une intervention maintenance
        /// </summary>
        /// <param name="IdMaintenance"></param>
        /// <param name="raisonSupp"></param>
        /// <returns></returns>
        public bool SupprimerMaintenance(int IdMaintenance, string raisonSupp)
        {
            bool SuppMaintOk = false;
            maintenance maint = context.maintenance.First(m => m.id == IdMaintenance);
            maint.maintenance_supprime = true;
            maint.date_suppression = DateTime.Now;
            maint.raison_suppression = raisonSupp;
            try
            {
                context.SaveChanges();
                SuppMaintOk = true;
            }
            catch(Exception e)
            {
                SuppMaintOk = false;
                logger.LogError(e, "Impossible de changer la maintenance N°" + IdMaintenance);
            }
            return SuppMaintOk;
        }

        public string ObtenirCodeIntervention(int IdMaintenance)
        {
            return context.maintenance.First(m => m.id == IdMaintenance).code_operation;
        }

        public maintenance ObtenirMaintenanceByID(int IdMaintenance)
        {
            return context.maintenance.First(m => m.id == IdMaintenance);
        }
        public string NomEquipement(int IdEquipement)
        {
            return context.equipement.First(e => e.id == IdEquipement).nom;
        }

        public essai ObtenirEssai(int resaID)
        {
            return context.essai.First(e => e.id == context.reservation_projet.First(r=>r.id == resaID).essaiID);
        }

        public bool SupprimerReservation(int IDresa)
        {
            try
            {
                reservation_projet resa = context.reservation_projet.First(r => r.id == IDresa);
                context.reservation_projet.Remove(resa);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la suppression réservation suite à intervention");
                return false;
            }
            return true;
        }

        public bool VerifDisponibilitEquipSurInterventions(DateTime dateFin, reservation_maintenance interv)
        {
            bool interOk = false;

            // Récupérer l'id zone pour l'équipement enquêté
            var zon = (from equip in context.equipement
                       where equip.id == interv.equipementID
                       select equip.zoneID.Value).First();

            #region disponibilité sur les interventions

            // Vérifier qu'il y a pas autre maintenance (non supprimée) sur ces dates et sur la même zone ou sur le même équipement
            // Et qu'il ne s'agit pas de la même intervention
            var IntervEquip = (from maint in context.maintenance
                               from resaMaint in context.reservation_maintenance
                               from equip in context.equipement
                               where maint.id == resaMaint.maintenanceID
                               && (maint.maintenance_supprime != true) && (maint.id != interv.maintenanceID)
                               && ((maint.type_maintenance == "Equipement en panne (blocage équipement)")
                               || (maint.type_maintenance == "Maintenance curative (Dépannage sans blocage zone)")
                               || (maint.type_maintenance == "Maintenance préventive(Interne sans blocage de zone)")
                               || (maint.type_maintenance == "Maintenance préventive (Externe sans blocage de zone)")
                               || (maint.type_maintenance == "Amélioration (sans blocage de zone)"))
                               && (resaMaint.equipementID == interv.equipementID)
                               && (((interv.date_debut >= resaMaint.date_debut) || dateFin >= resaMaint.date_debut)
                               && ((interv.date_debut <= resaMaint.date_fin) || dateFin <= resaMaint.date_fin))
                               select maint).Distinct().ToList();

            if (IntervEquip.Count() == 0)
            {
                interOk = true;
                // uniquement la zone bloqué
                // "Maintenance curative (Dépannage)"
                // "Maintenance préventive (Interne)"
                // "Maintenance préventive (Externe)"
                // "Amélioration"
                var IntervZone = (from maint in context.maintenance
                                  from resaMaint in context.reservation_maintenance
                                  from equip in context.equipement
                                  where maint.id == resaMaint.maintenanceID
                                  && (maint.maintenance_supprime != true) && (maint.id != interv.maintenanceID)
                                  && ((maint.type_maintenance == "Maintenance curative (Dépannage avec blocage de zone)")
                                  || (maint.type_maintenance == "Maintenance préventive (Interne avec blocage de zone)")
                                  || (maint.type_maintenance == "Maintenance préventive (Externe avec blocage de zone)")
                                  || (maint.type_maintenance == "Amélioration (avec blocage de zone)"))
                                  && (resaMaint.equipement.zoneID == zon)
                                  && (((interv.date_debut >= resaMaint.date_debut) || dateFin >= resaMaint.date_debut)
                                  && ((interv.date_debut <= resaMaint.date_fin) || dateFin <= resaMaint.date_fin))
                                  select maint).Distinct().ToList();
                if (IntervZone.Count() == 0)
                {
                    interOk = true;
                    goto ENDT;
                }
                else
                {
                    interOk = false;
                    goto ENDT;
                }
            }
            else
            {
                interOk = false;
                goto ENDT;
            }

        #endregion
        ENDT:
            return interOk;
        }

        public bool VerifDisponibilitZoneEquipSurInterventions(DateTime dateFin, reservation_maintenance interv)
        {
            bool interOk = false;

            // Récupérer l'id zone pour l'équipement enquêté
            var zon = (from equip in context.equipement
                       where equip.id == interv.equipementID
                       select equip.zoneID.Value).First();

            // Vérifier qu'il y a pas autre maintenance (non supprimée) sur ces dates et sur la même zone
            var IntervZon = (from maint in context.maintenance
                             from resaMaint in context.reservation_maintenance
                             from equip in context.equipement
                             where maint.id == resaMaint.maintenanceID
                             && (maint.maintenance_supprime != true) && (maint.id != interv.maintenanceID)
                             && (resaMaint.equipement.zoneID == zon)
                             && (((interv.date_debut >= resaMaint.date_debut) || dateFin >= resaMaint.date_debut)
                             && ((interv.date_debut <= resaMaint.date_fin) || dateFin <= resaMaint.date_fin))
                             select maint).Distinct().ToList();

            if (IntervZon.Count() == 0)
                interOk = true;

            return interOk;
        }

        public void UpdateStatusMaintenancePFLFinie(string action, int idMaint)
        {
            var resa = context.reservation_maintenance.First(r => r.id == idMaint);
            resa.interv_fini = true;
            resa.actions_realisees = action;
            context.SaveChanges();
        }

        public void UpdateStatusMaintenanceCommFinie(string action, int idMaint)
        {
            var resa = context.resa_maint_equip_adjacent.First(r => r.id == idMaint);
            resa.interv_fini = true;
            resa.actions_realisees = action;
            context.SaveChanges();
        }
    }
}
