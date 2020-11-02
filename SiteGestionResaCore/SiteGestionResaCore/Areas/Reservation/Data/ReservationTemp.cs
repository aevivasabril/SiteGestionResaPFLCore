using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteReservationGestionPFL.Areas.Reservation.Data
{
    /// <summary>
    /// class permettant de sauvegarder uniquement les dates d'une réservation
    /// </summary>
    public class ReservationTemp
    {
        /// <summary>
        /// Date début réservation
        /// </summary>
        public DateTime date_debut { get; set; }

        /// <summary>
        /// Date fin réservation
        /// </summary>
        public DateTime date_fin { get; set; }
    }
}