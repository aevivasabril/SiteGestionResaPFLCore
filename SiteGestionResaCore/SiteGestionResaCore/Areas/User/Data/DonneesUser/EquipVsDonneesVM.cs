using SiteGestionResaCore.Models.EquipementsReserves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.DonneesUser
{
    public class EquipVsDonneesVM
    {

        private List<InfosResasEquipement> _equipementsReserves = new List<InfosResasEquipement>();

        public List<InfosResasEquipement> EquipementsReserves
        {
            get { return _equipementsReserves; }
            set { _equipementsReserves = value; }
        }

        public int IdEssai { get; set; }
    }
}
