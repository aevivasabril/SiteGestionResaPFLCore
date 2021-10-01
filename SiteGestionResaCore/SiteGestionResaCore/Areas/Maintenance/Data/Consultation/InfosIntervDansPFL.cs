using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Consultation
{
    public class InfosIntervDansPFL
    {
        public int IdResMaint { get; set; } // Id reservation_maintenance
        public string CodeMaint { get; set; }
        public string TypeMaintenance { get; set; }
        public string DescriptifMaint { get; set; }
        public string OperateurPFL { get; set; }
        public string NomIntervExterne { get; set; }
        public string NomEquipement { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
    }
}
