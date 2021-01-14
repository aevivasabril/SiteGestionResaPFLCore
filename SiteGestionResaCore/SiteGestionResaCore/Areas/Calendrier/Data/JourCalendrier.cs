using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    /// <summary>
    /// Class contenant les infos par jour et pour les zones
    /// </summary>
    public class JourCalendrier
    {
        public DateTime JourPourAffichage { get; set; }

        public string NomJour { get; set; }

    }
}
