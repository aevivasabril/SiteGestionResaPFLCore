using SiteGestionResaCore.Models.EquipementsReserves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.ResasUser
{
    public class ModifierEquipVM
    {
        public List<InfosResasEquipement> ListResas { get; set; }

        public int IdEssai { get; set; }

        public int IdResa { get; set; }
    }
}
