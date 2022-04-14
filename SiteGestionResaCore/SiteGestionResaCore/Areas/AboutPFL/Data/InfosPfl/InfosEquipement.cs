using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data
{
    public class InfosEquipement
    {
        // id equipement calendrier view model
        public int IdEquipement { get; set; }
        // nom equipement calendrier view model
        public string NomEquipement { get; set; }
        // num GMAO calendrier view model
        public string NumGmaoEquipement { get; set; }
        public bool FicheMetrologieDispo { get; set; }

        #region Infos sur la fiche materiel si elle existe
        public bool FicheMaterielDispo { get; set; }
        public int IdFicheMat { get; set; }
        public string NomFicheMat { get; set; }
        public DateTime DateModif { get; set; }
        #endregion
    }
}
