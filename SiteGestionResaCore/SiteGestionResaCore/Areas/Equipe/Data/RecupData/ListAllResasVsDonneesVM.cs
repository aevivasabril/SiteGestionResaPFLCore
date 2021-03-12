using SiteGestionResaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Equipe.Data.RecupData
{
    public class ListAllResasVsDonneesVM
    {
        public List<InfosResasAdm> AllResas { get; set; } // liste des réservations utilisateur

        #region Infos "Essai" pour affichage

        private ConsultInfosEssaiChildVM _consultInfosEssai = new ConsultInfosEssaiChildVM(); // Vue partielle _DisplayInfosEssai

        public ConsultInfosEssaiChildVM ConsultInfosEssai
        {
            get { return _consultInfosEssai; }
            set { _consultInfosEssai = value; }
        }

        #endregion

        // View model pour la vue partielle 
        public EquipVsDonneesVM EquipVsDonnees { get; set; }
    }
}
