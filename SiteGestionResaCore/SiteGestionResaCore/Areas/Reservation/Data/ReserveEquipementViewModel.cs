using SiteReservationGestionPFL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteReservationGestionPFL.Areas.Reservation.Data
{
    public class ReserveEquipementViewModel
    {
        /// <summary>
        /// équipement sur la liste des checkbox à sélectionner pour réservation
        /// </summary>
        public equipement equipement { get; set; }

        /// <summary>
        /// Boolean indiquant si l'équipement a été sélectionné
        /// </summary>
        //public bool EstSelectionne { get; set; }
    }
}