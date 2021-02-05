using SiteGestionResaCore.Areas.User.Data.ResasUser;
using SiteGestionResaCore.Models.EquipementsReserves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.DonneesUser
{
    public class ListResasDonneesVM
    {
        public List<InfosResa> ResasUser { get; set; } // liste des réservations utilisateur

        #region Infos "Essai" pour affichage

        private ConsultInfosEssaiChilVM _consultInfosEssai = new ConsultInfosEssaiChilVM(); // Vue partielle _DisplayInfosEssai

        public ConsultInfosEssaiChilVM ConsultInfosEssai
        {
            get { return _consultInfosEssai; }
            set { _consultInfosEssai = value; }
        }

        #endregion

        private List<InfosResasEquipement> _equipementsReserves = new List<InfosResasEquipement>();

        public List<InfosResasEquipement> EquipementsReserves
        {
            get { return _equipementsReserves; }
            set { _equipementsReserves = value; }
        }


    }
}
