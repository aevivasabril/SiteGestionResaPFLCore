using System;

namespace SiteGestionResaCore.Areas.Reservation.Data.Validation
{
    public class InfosReservation
    {
        public string NomEquipement { get; set; }
        public string ZoneEquipement { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }

    }
}