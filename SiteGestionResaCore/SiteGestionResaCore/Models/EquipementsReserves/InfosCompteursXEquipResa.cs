using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Models.EquipementsReserves
{
    public class InfosCompteursXEquipResa
    {
        public int IdCompt { get; set; }
        public string NomCompteur { get; set; }
        public string NomEquipAssocie { get; set; }
        public int IdEquipAssocie { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public bool IsDataReady { get; set; } // s'il y à des données à récupérer
        public int IdResa { get; set; }
    }
}
