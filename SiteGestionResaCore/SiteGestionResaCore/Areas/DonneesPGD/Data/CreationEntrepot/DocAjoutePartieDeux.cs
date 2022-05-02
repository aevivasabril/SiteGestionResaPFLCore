using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data
{
    public class DocAjoutePartieDeux
    {
        public int IdEquipement { get; set; }
        public string NomEquipement { get; set; }
        public string TypeDonnees { get; set; }
        public int IdTypeDonnees { get; set; }
        public byte[] ContenuDoc { get; set; }
        public string NomDocument { get; set; }
        public int IdActivite { get; set; }
        public string NomActivite { get; set; }
        public int idReservation { get; set; }
    }
}
