using SiteGestionResaCore.Models.EquipementsReserves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Models
{
    public class EquipVsDonneesVM
    {

        private List<InfosResasEquipement> _equipementsReserves = new List<InfosResasEquipement>();

        public List<InfosResasEquipement> EquipementsReserves
        {
            get { return _equipementsReserves; }
            set { _equipementsReserves = value; }
        }

        private List<InfosCompteursXEquipResa> _compteursVsEquips;

        public List<InfosCompteursXEquipResa> CompteursVsEquips
        {
            get { return _compteursVsEquips; }
            set { _compteursVsEquips = value; }
        }


        public string TitreEssai { get; set; }
    }
}
