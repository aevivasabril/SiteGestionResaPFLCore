using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Maintenance
{
    public class AjoutEquipementsViewModel
    {
        public string OuvrirEquipSansZone { get; set; }
        /// <summary>
        /// Description contenant une référence à l'équipement et le problème retrouvé
        /// </summary>
        [Display(Name = "Décrivez l'équipement + le problème: ")]
        public string DescriptionProbleme { get; set; }

        /// <summary>
        /// String contenant la description des zones affectées
        /// </summary>
        [Display(Name = "Saissisez les zones affectées. (Ex: PFL + Salles alimentaires)")]
        public string ZoneImpacte { get; set; }

        /// <summary>
        /// Date debut pour créneau maintenance
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
        /// Définition créneau pour chaque datepicker (Maintenance)
        /// </summary>
        [Required(ErrorMessage = "La sélection 'Matin' ou 'après-midi' pour la date début est requis")]
        [Display(Name = "Créneau début")]
        public string DatePickerDebut_Matin { get; set; }

        [Required(ErrorMessage = "La sélection 'Matin' ou 'après-midi' pour la date fin est requis")]
        [Display(Name = "Créneau fin")]
        public string DatePickerFin_Matin { get; set; }

        public EquipementSansZoneVM EquipementSansZoneVM { get; set; }

        public List<EquipementSansZoneVM> ListEquipsSansZone { get; set; }

        public int IdPourSuppressionSansZone { get; set; }

    }
}
