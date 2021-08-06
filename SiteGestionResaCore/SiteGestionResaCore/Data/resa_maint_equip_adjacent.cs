using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public class resa_maint_equip_adjacent
    {
        public int id { get; set; }
        public string nom_equipement { get; set; }
        public int maintenanceID { get; set; }
        public DateTime date_debut { get; set; }
        public DateTime date_fin { get; set; }
        public string zone_affectee { get; set; }

        public virtual maintenance maintenance { get; set; }
    }
}
