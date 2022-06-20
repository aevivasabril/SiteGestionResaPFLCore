using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Data.Stats
{
    public class EssaisXTypeProdVM
    {
        public string NomProduit { get; set; }
        public List<EssaiXprod> ListEssais { get; set; }

    }
}
