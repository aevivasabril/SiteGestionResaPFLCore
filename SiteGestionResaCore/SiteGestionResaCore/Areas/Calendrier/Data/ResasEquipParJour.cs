using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    /// <summary>
    /// Classe répresentant les infos réservation pour un équipement et pour un jour X
    /// </summary>
    public class ResasEquipParJour
    {
        /// <summary>
        /// Constructeur classe initialisant les Listes
        /// </summary>
        public ResasEquipParJour()
        {
            ListResasMatin = new List<InfosAffichageResa>();
            ListResasAprem = new List<InfosAffichageResa>();
            InfosIntervAprem = new List<InfosAffichageMaint>();
            InfosIntervMatin = new List<InfosAffichageMaint>();
        }

        /// <summary>
        /// Informations détaillées pour chaque jour pour une semaine ou pour une durée déterminée par l'utilisateur
        /// </summary>
        /// J'ai réutilisé cette classe ReservationsJour car elle convient parfaitement
        public List<InfosAffichageResa> ListResasMatin { get; set; }

        public List<InfosAffichageResa> ListResasAprem { get; set; }

        public List<InfosAffichageMaint> InfosIntervMatin { get; set; }

        public List<InfosAffichageMaint> InfosIntervAprem { get; set; }

        /// <summary>
        /// Couleur de fond selon occupation (Matin)
        /// </summary>
        public string CouleurMatin { get; set; }

        /// <summary>
        /// Couleur de fond selon occupation (Aprem)
        /// </summary>
        public string CouleurAprem { get; set; }
    }
}
