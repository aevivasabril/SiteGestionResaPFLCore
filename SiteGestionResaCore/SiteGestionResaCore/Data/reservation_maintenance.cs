using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public class reservation_maintenance
    {
        public int id { get; set; }
        public int equipementID { get; set; }
        public int maintenanceID { get; set; }
        public DateTime date_debut{ get; set; }
        public DateTime date_fin { get; set; }

        public virtual equipement equipement { get; set; }
        public virtual maintenance maintenance { get; set; }
    }
}
