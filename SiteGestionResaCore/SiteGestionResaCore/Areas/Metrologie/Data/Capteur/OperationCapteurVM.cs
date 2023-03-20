using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.Capteur
{
    public class OperationCapteurVM
    {
        public List<CapteurXAffichage> ListCapteurs { get; set; }
        public int IdCapteurXSupp { get; set; }
        public string NomCapteurXSupp { get; set; }

    }
}
