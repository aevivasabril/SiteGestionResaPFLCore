using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Data.Stats
{
    public class EssaiXprod
    {
        public string NumProjet { get; set; }
        public string TitreProjet { get; set; }
        public int IdEssai { get; set; }
        public string TitreEssai { get; set; }
        public string AuteurEssai { get; set; }
        public string? PrecisionProd { get; set; }
        public int? QuantiteProd { get; set; }
        public string DestProdFinal { get; set; }
        public DateTime DateCreationEssai { get; set; }
    }
}
