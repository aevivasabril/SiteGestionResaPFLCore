using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    /// <summary>
    /// View model pour la vue CalendrierPFL
    /// </summary>
    public class CalenViewModel
    {
        /// <summary>
        /// Constructeur classe initialisant les Listes
        /// </summary>
        public CalenViewModel()
        {
            ListResasZone = new List<ResasZone>();
            JoursCalendrier = new List<JourCalendrier>();
        }

        // Liste contenant les infos sur les 17 zones pour la totalité des jours N (facilité d'affichage)
        public List<ResasZone> ListResasZone { get; set; }

        // Liste avec les jours à afficher uniquement (pour les headers) (N jours)
        public List<JourCalendrier> JoursCalendrier { get; set; }

        /// <summary>
        /// Date debut à afficher pour le planning PFL 
        /// </summary>
        public DateTime? DateDu { get; set; }

        /// <summary>
        /// Date fin à afficher pour le planning PFL 
        /// </summary>
        public DateTime? DateAu { get; set; }

        public InfosEquipementReserve InfosPopUpEquipement { get; set; }

        public int IdEssaiToShow { get; set; }

    }
}
