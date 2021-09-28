﻿using SiteGestionResaCore.Areas.Reservation.Data.Reservation;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Models;
using SiteGestionResaCore.Models.Maintenance;
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
            InfosResaMatin = new List<ReservationInfos>();
            InfosResaAprem = new List<ReservationInfos>();
            InfosIntervAprem = new List<InfosAffichageMaint>();
            InfosIntervMatin = new List<InfosAffichageMaint>();
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
        public List<ReservationInfos> InfosResaMatin { get; set; }

        /// <summary>
        /// Reservations pour le jour en question (Aprèm) 
        /// </summary>
        public List<ReservationInfos> InfosResaAprem { get; set; }

        public List<InfosAffichageMaint> InfosIntervMatin { get; set; }
        public List<InfosAffichageMaint> InfosIntervAprem { get; set; }

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