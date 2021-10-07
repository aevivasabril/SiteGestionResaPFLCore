using SiteGestionResaCore.Areas.Maintenance.Data.Maintenance;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Modification
{
    public interface IModifMaintenanceDB
    {
        MaintenanceInfos ObtenirInfosMaint(string CodeIntervention);
        bool NumMaintExist(string CodeIntervention);
        List<EquipCommunXInterv> ListMaintAdj(int IdMaint);
        List<EquipPflXIntervention> ListIntervPFL(int IdMaint);
        DateTime ChangeDateFinEquipCommun(int IdResaCommun, DateTime NewDateFin);
        List<utilisateur> ObtenirListUtilisateursSite();
        maintenance ObtenirMaintenanceXInterv(int IdresaEquipComm);
        resa_maint_equip_adjacent ObtenirIntervEquiComm(int IdresaEquipComm);
        reservation_maintenance ObtenirIntervEquipPfl(int IdresaEquipPfl);
        maintenance ObtenirMaintenanceXIntervPFl(int IdresaEquipPfl);
        void AnnulerEssai(essai ess, string codeMaint);
        string ObtenirMailUser(int iduser);
        Task<IList<utilisateur>> List_utilisateurs_logistiqueAsync();
        void ChangeDateFinEquipPFL(int IdResaPfl, DateTime NewDateFin);
        bool ModifZoneDisponibleXIntervention(DateTime datedebut, DateTime datefin, int idEquipement, int Idmaintenance);
    }
}
