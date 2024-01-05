using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Data.Stats
{
    public class MaintenanceInfos
    {
        public string TypeMaintenance { get; set; }
        public string CodeMaintenance { get; set; }
        public string MailOperateur { get; set; }
        public string IntervenantExt { get; set; }
        public string? NomIntervExt { get; set; }
        public string DescripOperation { get; set; }
        public string? MaintSupprimee { get; set; }
        public DateTime? DateSuppression { get; set; }
        public string RaisonSuppression { get; set; }
        public string MaintTerminee { get; set; }
        public string ActionRealisee { get; set; }

        public string NomEquipement { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string ZoneAffectee { get; set; }

    }
}
