using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    /// <summary>
    /// class contenant les informations principales sur une réservation (pour éviter d'utiliser la classe BDD)
    /// </summary>
    public class ReservationInfos
    {
        public string num_projet { get; set; }

        public string confidentialite { get; set; }

        public string titre_projet { get; set; }

        public string mailRespProjet { get; set; }
    }
}
