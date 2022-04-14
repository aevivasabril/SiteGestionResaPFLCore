using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data.ModifEquip
{
    public class EquipsXModifVM
    {
        public List<InfosEquipement> ListeEquipements { get; set; }
        public string NomZone { get; set; }
        public int IdZone { get; set; }
        public int IdEquipement { get; set; }
    }
}
