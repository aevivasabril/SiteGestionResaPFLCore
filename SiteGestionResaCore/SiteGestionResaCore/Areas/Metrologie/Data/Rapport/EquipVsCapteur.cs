using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.Rapport
{
    public class EquipVsCapteur
    {
        public int equipementId { get; set; }
        public string nomEquipement { get; set; }
        public string numGmao { get; set; }
        public int capteurId { get; set; }
        public string nomCapteur { get; set; }
    }
}
