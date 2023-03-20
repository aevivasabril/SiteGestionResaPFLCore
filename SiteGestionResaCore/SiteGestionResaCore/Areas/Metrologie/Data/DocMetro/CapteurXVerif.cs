using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.DocMetro
{
    public class CapteurXVerif
    {
        public string CodeCapteur { get; set; }
        public string NomCapteur { get; set; }
        public DateTime DateDerniereVerif { get; set; }
        public string TypeVerif { get; set; }
        public DateTime DateProVerif { get; set; }
        public string Periodicite { get; set; }
    }
}
