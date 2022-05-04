using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data.AccesEntrepot
{
    public class DocXEssai
    {
        public int IdDoc { get; set; }
        public string NomDocument { get; set; }
        public string? NomEquipement { get; set; }
        public int? IdEquipement { get; set; }
        public string TypeActivite { get; set; }
        public string TypeDonnees { get; set; }
        public double TailleKo { get; set; }
    }
}
