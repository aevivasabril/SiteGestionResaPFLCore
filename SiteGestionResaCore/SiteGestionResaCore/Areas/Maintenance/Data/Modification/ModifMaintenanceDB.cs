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

            foreach (var maint in listIntervCommun)
            {
                if(maint.date_fin < DateTime.Today)
                    finie = true;
                else
                    finie = false;

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

            foreach(var maint in listMaintPfl)
            {
                if (maint.date_fin < DateTime.Today)
                    finie = true;
                else
                    finie = false;
                EquipPflXIntervention equipPflXIntervention = new EquipPflXIntervention 
                {
                    DateDebut = maint.date_debut,
                    DateFin = maint.date_fin,
                    Id = maint.id,
                    NomEquipement = context.equipement.First(e=>e.id == maint.equipementID).nom,
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
        /// </summary>
        /// <returns></returns>
        public List<utilisateur> ObtenirListUtilisateursSite()
        {
            return context.Users.Distinct().ToList();
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
        /// Mettre à jour un essai pour l'annuler
        /// </summary>
        /// <param name="ess">essai</param>
        /// <param name="codeMaint">code operation maintenances</param>
        public void AnnulerEssai(essai ess, string codeMaint)
        {
            var essa = context.essai.First(e => e.id == ess.id);
            essa.resa_refuse = true;
            essa.status_essai = EnumStatusEssai.Refuse.ToString();
            essa.raison_refus = "Essai annulé automatiquement suite à l'intervention N°: " + codeMaint;
            context.SaveChanges();
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
        /// Vérifier s'une zone est dispo pour midification des date fin!
        /// toutes les interventions sauf les pannes (maintenance curative)
        /// </summary>
        /// <param name="dateDebut"></param>
        /// <param name="dateFin"></param>
        /// <param name="idEquipement"></param>
        /// <param name="Idmaintenance"></param>
        /// <returns></returns>
        public bool ModifZoneDisponibleXIntervention(DateTime dateDebut, DateTime dateFin, int idEquipement, int Idmaintenance)
        {
            bool resaOk = false;
            bool interOk = false;

            int ApCinq = Convert.ToInt32(EnumZonesPfl.SalleAp5);
            int ApSix = Convert.ToInt32(EnumZonesPfl.SalleAp6);
            int ApSeptA = Convert.ToInt32(EnumZonesPfl.SalleAp7A);
            int ApSeptB = Convert.ToInt32(EnumZonesPfl.SalleAp7B);
            int ApSeptC = Convert.ToInt32(EnumZonesPfl.SalleAp7C);
            int ApHuit = Convert.ToInt32(EnumZonesPfl.SalleAp8);
            int ApNeuf = Convert.ToInt32(EnumZonesPfl.SalleAp9);

            // Récupérer l'id zone pour l'équipement enquêté
            var zon = (from equip in context.equipement
                       where equip.id == idEquipement
                       select equip.zoneID.Value).First();

            #region disponibilité sur les essais

            // Si l'équipement où l'on intervient est dans la PFL alors il faut vérifier qu'il n'y a pas des essais confidentiels aux mêmes dates
            // Vérifier qu'il y a pas autre maintenance sur ces dates et sur la même zone
            if (zon != ApCinq && zon != ApSix && zon != ApSeptA && zon != ApSeptB && zon != ApSeptC && zon != ApHuit && zon != ApNeuf)
            {
                // Vérifier la disponibilité sur les essais qu'ont au moins un materiel dans la même zone
                // J'ai enlevé la condition confidentiel car l'essai peut se derouler sauf si ça touche la même zone de mon intervention ou le même équipement
                var resasZon = (from essai in context.essai
                                from resa in context.reservation_projet
                                from equip in context.equipement
                                where essai.id == resa.essaiID
                                && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                    essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                                && (resa.equipement.zoneID == zon || resa.equipementID == idEquipement) //|| essai.confidentialite == EnumConfidentialite.Confidentiel.ToString())
                                && (resa.equipement.zoneID != ApCinq && resa.equipement.zoneID != ApSix && resa.equipement.zoneID != ApSeptA
                                && resa.equipement.zoneID != ApSeptB && resa.equipement.zoneID != ApSeptC
                                && resa.equipement.zoneID != ApHuit && resa.equipement.zoneID != ApNeuf)
                                && (((dateDebut >= resa.date_debut) || dateFin >= resa.date_debut)
                                && ((dateDebut <= resa.date_fin) || dateFin <= resa.date_fin))
                                select essai).Distinct().ToList();

                if (resasZon.Count() == 0)
                    resaOk = true;
            }
            else
            { // dans le cas des zones alimentaires alors on vérifie qu'il n'y a pas des essais dans la même zone de l'intervention
                var resasZon = (from essai in context.essai
                                from resa in context.reservation_projet
                                from equip in context.equipement
                                where essai.id == resa.essaiID
                                && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                    essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                                && (resa.equipement.zoneID == zon)
                                && (resa.equipement.zoneID == ApCinq || resa.equipement.zoneID == ApSix || resa.equipement.zoneID == ApSeptA
                                || resa.equipement.zoneID == ApSeptB || resa.equipement.zoneID == ApSeptC
                                || resa.equipement.zoneID == ApHuit || resa.equipement.zoneID == ApNeuf)
                                && (((dateDebut >= resa.date_debut) || dateFin >= resa.date_debut)
                                && ((dateDebut <= resa.date_fin) || dateFin <= resa.date_fin))
                                select essai).Distinct().ToList();

                if (resasZon.Count() == 0)
                    resaOk = true;
            }

            #endregion

            #region disponibilité sur les interventions

            // Vérifier qu'il y a pas autre maintenance sur ces dates et sur la même zone
            // vérifier qu'il ne s'agit pas de la même opération de maintenance
            var IntervZon = (from maint in context.maintenance
                             from resaMaint in context.reservation_maintenance
                             from equip in context.equipement
                             where maint.id == resaMaint.maintenanceID
                             && (maint.maintenance_supprime != true)
                             && (resaMaint.equipement.zoneID == zon) && (maint.id != Idmaintenance)
                             && (((dateDebut >= resaMaint.date_debut) || dateFin >= resaMaint.date_debut)
                             && ((dateDebut <= resaMaint.date_fin) || dateFin <= resaMaint.date_fin))
                             select maint).Distinct().ToList();

            if (IntervZon.Count() == 0)
                interOk = true;

            #endregion

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
    }
}
