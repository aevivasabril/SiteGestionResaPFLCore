using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Modification
{
    public class EquipPflXIntervention
    {
        public int Id { get; set; }
        public string NomEquipement { get; set; }
        public string NumGMAO { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public bool IsIntervFinie { get; set; }
    }
}
