using SiteGestionResaCore.Data;
using System.Collections.Generic;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    #region Model view pour la vue "PlanZonesReservation"

    #endregion

    #region Model View pour la vue "EquipementsVsZone"
    /// <summary>
    /// View model qui répresente les équipements contenus dans une zone specifique
    /// </summary>
    public class EquipementsParZoneViewModel
    {
        /// <summary>
        /// Nom de la zone à reserver
        /// </summary>
        public string NomZone { get; set; }

        /*private List<equipement> _equipements;
        /// <summary>
        /// Liste des équipements par zone déterminée à complèter après click sur une des zones
        /// </summary>
        public List<equipement> Equipements
        {
            get
            {
                return _equipements;
            }
            set { _equipements = value; }
        }*/

        /// <summary>
        /// Id de la zone à réserver
        /// </summary>
        public int IdZone { get; set; }

        /// <summary>
        /// création du model child pour le passer à la vue partielle _CalendrierEquipement (tous les réservations par équipement)
        /// </summary>
        public List<CalendrierEquipChildViewModel> CalendrierChildVM { get; set; }

        public int IndiceChildModel { get; set; }

        public int IndiceResaEquipXChild { get; set; }

    }
    #endregion

}