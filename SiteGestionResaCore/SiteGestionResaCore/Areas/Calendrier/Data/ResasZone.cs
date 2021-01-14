using SiteGestionResaCore.Areas.Reservation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    /// <summary>
    /// Classe contenant les infos des réservations sur une zone
    /// </summary>
    public class ResasZone
    {
        public int IdZone { get; set; }

        public string NomZone { get; set; }

        // Liste avec l'occupation de la zone Du - Au (N jours). Je separe pour pouvoir remplir la table facilement
        public List<OccupationZonesParJour> ListeDispoZonesDuAu { get; set; }

        // Liste des équipements par zone (Du - Au) et 
        public List<EquipementVsResa> ListEquipementVsResa { get; set; }
    }
}
