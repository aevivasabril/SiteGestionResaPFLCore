using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data
{
    public class DocAjoutePartieUn
    {
        public byte[] data { get; set; }
        public int TypeActiviteID { get; set; }
        public string TypeActivite { get; set; }
        public string NomDocument { get; set; }
        public int TypeDonneesID { get; set; }
        public string TypeDonnees { get; set; }
        public double TailleKo { get; set; }
    }
}
