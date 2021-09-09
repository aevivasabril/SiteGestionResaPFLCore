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
    }
}
