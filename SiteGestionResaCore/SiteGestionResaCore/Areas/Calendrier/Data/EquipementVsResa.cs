using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    /// <summary>
    /// Classe contenant les infos sur les réservations equipements DU AU (N jours)
    /// </summary>
    public class EquipementVsResa
    {
        public List<ResasEquipParJour> ListResasDuAu { get; set; }

        public string NomEquipement { get; set; }

        public int IdEquipement { get; set; }

    }
}
