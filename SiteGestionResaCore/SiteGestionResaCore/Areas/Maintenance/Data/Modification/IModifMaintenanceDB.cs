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
        string ObtenirMailUser(int iduser);
        Task<IList<utilisateur>> List_utilisateurs_logistiqueAsync();
        void ChangeDateFinEquipPFL(int IdResaPfl, DateTime NewDateFin);
        bool ModifZoneDisponibleXIntervention(DateTime datefin, reservation_maintenance interv);
        bool ModifEquipementDisponibleXIntervention(DateTime datefin, reservation_maintenance interv);
        bool SupprimerMaintenance(int IdMaintenance, string raisonSupp);
        string ObtenirCodeIntervention(int IdMaintenance);
        maintenance ObtenirMaintenanceByID(int IdMaintenance);
        string NomEquipement(int IdEquipement);
        essai ObtenirEssai(int resaID);
        bool SupprimerReservation(int IDresa);
        bool VerifDisponibilitEquipSurInterventions(DateTime dateFin, reservation_maintenance interv);
        bool VerifDisponibilitZoneEquipSurInterventions(DateTime dateFin, reservation_maintenance interv);
        void UpdateStatusMaintenanceFinie(int idMaint);
    }
}
