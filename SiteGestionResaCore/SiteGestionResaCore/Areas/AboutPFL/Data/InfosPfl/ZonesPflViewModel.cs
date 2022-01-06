using SiteGestionResaCore.Areas.Reservation.Data;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Data
{
    public class ZonesPflViewModel
    {
        #region Sélection des zones pour réservation 

        public EnumZonesPfl EnumZones { get; set; }

        private List<zone> _zones;
        /// <summary>
        /// Liste des zones de la plateforme récupérés à partir de la base de données
        /// </summary>
        public List<zone> Zones
        {
            get { return _zones; }
            set { _zones = value; }
        }
        #endregion
    }
}
