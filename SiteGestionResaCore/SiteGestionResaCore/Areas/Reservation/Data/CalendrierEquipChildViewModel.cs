using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    #region Model View pour la vue "EquipementsVsZone"

    #endregion

    #region Model view pour les vues partielles "_Creneaux" et "_CalendrierEquipement"

    /// <summary>
    /// View model representant les infos sur les réservations d'un équipement specifique
    /// </summary>
    public class CalendrierEquipChildViewModel
    {
        /// <summary>
        /// informations détaillées pour chaque jour pour une semaine ou pour une durée déterminée par l'utilisateur
        /// </summary>
        public List<ReservationsJour> ListResas = new List<ReservationsJour>();

        /// <summary>
        /// Objet Equipement pour affichage et applications des opérations de réservation
        /// </summary>
        public equipement EquipementCalendrier { get; set; }

        /// <summary>
        /// Date debut à afficher pour le planning équipement 
        /// </summary>
        [Required]
        [Display(Name = "Du : ")]
        public DateTime? DatePickerDu { get; set; }

        /// <summary>
        /// Date fin pour afficher le planning equipement 
        /// </summary>
        [Required]
        [Display(Name = "Au : ")]
        public DateTime? DatePickerAu { get; set; }

        /// <summary>
        /// Date debut pour créneau réservation
        /// </summary>
        [Required]
        [Display(Name = "Date Debut : ")]
        public DateTime? DateDebut { get; set; }

        /// <summary>
        /// Date fin pour créneau réservation
        /// </summary>
        [Required]
        [Display(Name = "Date Fin : ")]
        public DateTime? DateFin { get; set; }

        /// <summary>
        /// Définition créneau pour chaque datepicker (réservation)
        /// </summary>
        [Required]
        [Display(Name = "Créneau début")]
        public bool? DatePickerDebut_Matin { get; set; }

        [Required]
        [Display(Name = "Créneau fin")]
        public bool? DatePickerFin_Matin { get; set; }


        private List<ReservationTemp> _resaEquipement = new List<ReservationTemp>();
        /// <summary>
        /// Reservation saisie pour l'équipement 
        /// </summary>
        public List<ReservationTemp> ResaEquipement
        {
            get
            {
                return _resaEquipement;
            }
            set { _resaEquipement = value; }
        }

    }
    #endregion

}