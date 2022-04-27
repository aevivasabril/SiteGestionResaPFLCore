using SiteGestionResaCore.Areas.Reservation.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.ResasUser
{
    public class ModifDatesVM
    {
        /// <summary>
        /// informations détaillées pour chaque jour pour une semaine ou pour une durée déterminée par l'utilisateur
        /// </summary>
        public List<ReservationsJour> ListResas = new List<ReservationsJour>();

        public string NomEquipement { get; set; }
        public int IdReservation { get; set; }
        public int IdEquipement { get; set; }
        public int IdEssai { get; set; }

        [Required(ErrorMessage = "Le champ 'date debut' est requis")]
        [Display(Name = "Date debut")]
        public DateTime? DateDebutCreneau { get; set; }

        [Required(ErrorMessage = "Le champ 'date fin' est requis")]
        [Display(Name = "Date fin")]
        public DateTime? DateFinCreneau { get; set; }

        [Required(ErrorMessage = "Le champ 'date debut calendrier' est requis")]
        [Display(Name = "Date debut calendrier")]
        public DateTime? DateDebutCalend { get; set; }

        [Required(ErrorMessage = "Le champ 'date fin calendrier' est requis")]
        [Display(Name = "Date fin calendrier")]
        public DateTime? DateFinCalend { get; set; }

        /// <summary>
        /// Définition créneau pour chaque datepicker (réservation)
        /// </summary>
        [Required(ErrorMessage = "La sélection 'Matin' ou 'après-midi' pour la date début est requis")]
        [Display(Name = "Créneau début")]
        public string DatePickerDebut_Matin { get; set; }

        [Required(ErrorMessage = "La sélection 'Matin' ou 'après-midi' pour la date fin est requis")]
        [Display(Name = "Créneau fin")]
        public string DatePickerFin_Matin { get; set; }

        public int IndiceChildModel { get; set; }

        public int IndiceResaEquipXChild { get; set; }
    }
}
