using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Models.EquipementsReserves
{
    public class InfosResasEquipement
    {
        public int IdResa { get; set; }
        public string NomEquipement { get; set; }
        public string ZoneEquipement { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
    }
}
