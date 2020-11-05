using SiteGestionResaCore.Data;
using SiteGestionResaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteGestionResaCore.Areas.Reservation.Data
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