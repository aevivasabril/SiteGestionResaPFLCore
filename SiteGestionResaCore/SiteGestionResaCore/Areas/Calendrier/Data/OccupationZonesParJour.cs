using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    /// <summary>
    /// Classe appart pour indiquer si une zone est occupée ou libre
    /// </summary>
    public class OccupationZonesParJour
    {
        public bool IsZoneOccupeMatin { get; set; } // Afficher si la zone est occupée ou disponible

        public bool IsZoneOccupeAprem { get; set; } // Afficher si la zone est occupée ou disponible
    }
}
