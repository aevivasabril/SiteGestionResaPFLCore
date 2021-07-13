using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data.Reservation
{
    public class SousListeEquipements
    {
        // id equipement calendrier view model
        public int IdEquipement { get; set; }

        // nom equipement calendrier view model
        public string NomEquipement { get; set; }

        // num GMAO calendrier view model
        public string NumGmaoEquipement { get; set; }

        public bool IsEquipSelect { get; set; }
    }
}
