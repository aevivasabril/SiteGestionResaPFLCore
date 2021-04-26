﻿using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteGestionResaCore.Areas.Reservation.Data
{

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

        #region Informations sur l'équipement concerné

        // id equipement calendrier view model
        public int idEquipement { get; set; }

        // nom equipement calendrier view model
        public string nomEquipement { get; set; }

        // num GMAO calendrier view model
        public string numGmaoEquipement { get; set; }

        // TODO: vois si ce serait utile (je pense que non!) zone id calendrier view model
        public int zoneIDEquipement { get; set; }

        #endregion  
        /// <summary>
        /// Date debut à afficher pour le planning équipement 
        /// </summary>
        [Required(ErrorMessage = "Le champ 'Du' est requis")]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode =true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Du : ")]
        public DateTime? DatePickerDu { get; set; }

        /// <summary>
        /// Date fin pour afficher le planning equipement 
        /// </summary>
        [Required(ErrorMessage = "Le champ 'Au' est requis")]
        [DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Au : ")]
        public DateTime? DatePickerAu { get; set; }

        /// <summary>
        /// Date debut pour créneau réservation
        /// </summary>
        [Required(ErrorMessage = "Le champ 'Date Debut' est requis")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Debut : ")]
        public DateTime? DateDebut { get; set; }

        /// <summary>
        /// Date fin pour créneau réservation
        /// </summary>
        [Required(ErrorMessage = "Le champ 'Date Fin' est requis")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Fin : ")]
        public DateTime? DateFin { get; set; }

        /// <summary>
        /// Définition créneau pour chaque datepicker (réservation)
        /// </summary>
        [Required(ErrorMessage = "La selection 'Matin' ou 'après-midi' pour la date début est requis")]
        [Display(Name = "Créneau début")]
        public string DatePickerDebut_Matin { get; set; }

        [Required(ErrorMessage = "La selection 'Matin' ou 'après-midi' pour la date fin est requis")]
        [Display(Name = "Créneau fin")]
        public string DatePickerFin_Matin { get; set; }


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