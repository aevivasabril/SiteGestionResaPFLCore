using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Areas.Maintenance.Data.Maintenance;
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
        public MaintenanceInfos ObtenirInfosMaint(string CodeIntervention)
        {
            var mainte = context.maintenance.First(m => m.code_operation.Equals(CodeIntervention));
            MaintenanceInfos infos = new MaintenanceInfos
            {
                IdMaint = mainte.id,
                NomIntervenantExt = mainte.nom_intervenant_ext,
                CodeOperation = mainte.code_operation,
                DescriptionOperation = mainte.description_operation,
                EmailAuteur = context.Users.First(u => u.Id == mainte.userID).Email,
                TypeMaintenance = mainte.type_maintenance
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
            //return false;
        }
        /// <summary>
        /// Retourne la liste des équipements communs dont l'id maintenance est égal à IdMaint
        /// </summary>
        /// <param name="IdMaint"></param>
        /// <returns></returns>
        public List<EquipCommunXInterv> ListMaintAdj(int IdMaint)
        {
            List<EquipCommunXInterv> List = new List<EquipCommunXInterv>();
            var listIntervCommun = context.resa_maint_equip_adjacent.Where(r => r.maintenanceID == IdMaint).ToList();

            foreach (var maint in listIntervCommun)
            {
                EquipCommunXInterv equipCommunXInterv = new EquipCommunXInterv
                {
                    DateDebut = maint.date_debut,
                    DateFin = maint.date_fin,
                    IdInterv = maint.id,
                    NomEquipement = maint.nom_equipement,
                    ZoneAffectee = maint.zone_affectee
                };

                List.Add(equipCommunXInterv);
            }

            return List;
        }

        public List<EquipPflXIntervention> ListIntervPFL(int IdMaint)
        {
            List<EquipPflXIntervention> List = new List<EquipPflXIntervention>();

            var listMaintPfl = context.reservation_maintenance.Where(r => r.maintenanceID == IdMaint).ToList();

            foreach(var maint in listMaintPfl)
            {
                EquipPflXIntervention equipPflXIntervention = new EquipPflXIntervention 
                {
                    DateDebut = maint.date_debut,
                    DateFin = maint.date_fin,
                    Id = maint.id,
                    NomEquipement = context.equipement.First(e=>e.id == maint.equipementID).nom
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

        public List<utilisateur> ObtenirListUtilisateursSite()
        {
            return context.Users.Distinct().ToList();
        }

        public maintenance ObtenirMaintenance(int IdresaEquipComm)
        {
            var resa = context.resa_maint_equip_adjacent.First(r => r.id == IdresaEquipComm);
            return context.maintenance.First(m => m.id == resa.maintenanceID);
        }

        public resa_maint_equip_adjacent ObtenirIntervEquiComm(int IdresaEquipComm)
        {
            return context.resa_maint_equip_adjacent.First(r => r.id == IdresaEquipComm);
        }
    }
}
