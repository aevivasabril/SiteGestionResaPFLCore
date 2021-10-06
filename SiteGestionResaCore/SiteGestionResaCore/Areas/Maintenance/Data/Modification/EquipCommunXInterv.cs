using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Modification
{
    public class EquipCommunXInterv
    {
        public int IdInterv { get; set; }
        public string NomEquipement { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string ZoneAffectee { get; set; }    
    }
}
