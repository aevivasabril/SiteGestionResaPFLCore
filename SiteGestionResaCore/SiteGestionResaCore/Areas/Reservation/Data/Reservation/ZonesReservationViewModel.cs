using SiteGestionResaCore.Data;
using System.Collections.Generic;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    #region Model view pour la vue "PlanZonesReservation"
    /// <summary>
    /// View model pour la vue de selection des zones pour réservation
    /// </summary>
    public class ZonesReservationViewModel
    {
        #region Sélection des zones pour réservation 

        public EnumZonesPfl EnumZones { get; set; }

        private List<zone> _zones;
        /// <summary>
        /// Liste des zones de la plateforme récupérés à partir de la base de données
        /// </summary>
        public List<zone> Zones
        {
            get { return _zones;}
            set { _zones = value; }
        }
        #endregion

        private List<EquipementsParZoneViewModel> _equipementsParZone = new List<EquipementsParZoneViewModel>();
        /// <summary>
        /// Liste des équipements par zone déterminée à complèter après click sur une des zones
        /// </summary>
        public List<EquipementsParZoneViewModel> EquipementsParZone
        {
            get
            {
                return _equipementsParZone;
            }
            set { _equipementsParZone = value; }
        }
    }
    #endregion

}