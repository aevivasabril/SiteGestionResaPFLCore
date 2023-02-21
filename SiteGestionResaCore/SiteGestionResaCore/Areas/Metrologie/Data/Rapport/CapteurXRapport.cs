using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.Rapport
{
    public class CapteurXRapport
    {
        public int idCapteur { get; set; }
        public string nomCapteur { get; set; }
        public string nomEquipement { get; set; }
        public string numGmao { get; set; }
        public string nom_document { get; set; }
        public DateTime dateVerif { get; set; }
        public string TypeRapport { get; set; }
        public int idRapport { get; set; }
    }
}
