using SiteGestionResaCore.Areas.User.Data.ResasUser;
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

        // View model pour la vue partielle 
        public EquipVsDonneesVM EquipVsDonnees { get; set; }
    }
}
