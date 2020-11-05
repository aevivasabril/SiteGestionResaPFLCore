using SiteGestionResaCore.Data;
using SiteGestionResaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    /// <summary>
    /// Classe qui permet d'intégrer toutes les données sur les réservations d'un jour specifique (destination vers le calendrier)
    /// </summary>
    public class ReservationsJour
    {
        /// <summary>
        /// Constructeur classe initialisant les Listes
        /// </summary>
        public  ReservationsJour ()
        {
            InfosResaMatin = new List<reservation_projet>();
            InfosResaAprem = new List<reservation_projet>();
        }

        /// <summary>
        /// Jour du calendrier à  interroger
        /// </summary>
        public DateTime JourResa { get; set; }

        /// <summary>
        /// Nom du jour en français
        /// </summary>
        public string NomJour { get; set; }

        /// <summary>
        /// Reservations pour le jour en question (Matin) 
        /// </summary>
        public List<reservation_projet> InfosResaMatin { get; set; }

        /// <summary>
        /// Reservations pour le jour en question (Aprèm) 
        /// </summary>
        public List<reservation_projet> InfosResaAprem { get; set; }

        // TODO: Créer le tableau !!!! maintenance pour le jour en question 
        //maintenance [] InfosMaintenance { get; set; }

        // TODO: Créer le tableau !!!! métrologie pour le jour en question
        // metrologie [] InfosMetrologie { get; set; }

        /// <summary>
        /// Couleur de fond selon occupation (Matin)
        /// </summary>
        public string CouleurFondMatin { get; set; }

        /// <summary>
        /// Couleur de fond selon occupation (Aprem)
        /// </summary>
        public string CouleurFondAprem { get; set; }

    }
}