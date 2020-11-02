using SiteGestionResaCore.Data;
using SiteReservationGestionPFL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteReservationGestionPFL.Areas.Reservation.Data
{
    /// <summary>
    /// Classe répresentant un essai et son propiètaire
    /// </summary>
    public class EssaiUtilisateur
    {
        /// <summary>
        /// Manipulateur essai
        /// </summary>
        public utilisateur user { get; set; }

        /// <summary>
        /// Essai appartenant à un "user"
        /// </summary>
        public essai CopieEssai { get; set; }
    }
}