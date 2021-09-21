using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Maintenance
{
    public interface IFormulaireIntervDB
    {
        List<ld_type_maintenance> List_Type_Maintenances();
        Task<IList<utilisateur>> List_utilisateurs_logistiqueAsync();
        string CodeOperation();
        maintenance AjoutMaintenance(MaintenanceViewModel vm);
        bool EnregistrerIntervSansZone(EquipementSansZoneVM sansZoneVM, maintenance maint);
        List<utilisateur> ObtenirListUtilisateursSite();
        void AnnulerEssai(essai ess, string codeMaintenance);
        string ObtenirMailUser(int idUser);
        //bool AjoutInterventions(List<EquipementDansZone> listeDans, List<EquipementSansZoneVM> listSans, int MaintId);
        bool EnregistrerIntervsDansZone(List<EquipementDansZone> EquipDansZone, maintenance maint);
        essai UpdateEssai(int essId);

    }
}
