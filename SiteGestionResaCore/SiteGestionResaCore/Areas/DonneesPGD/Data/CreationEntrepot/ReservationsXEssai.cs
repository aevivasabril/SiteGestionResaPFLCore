using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data
{
    public class ReservationsXEssai
    {
        public int idResa { get; set; }
        public int IdEquipement { get; set; }
        public string NomEquipement { get; set; }
        public string FichierPcVue { get; set; }
        public string color { get; set; }
    }
}
