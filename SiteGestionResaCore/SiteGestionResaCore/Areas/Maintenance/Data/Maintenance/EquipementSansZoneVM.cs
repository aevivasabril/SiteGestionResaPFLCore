using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Maintenance
{
    public class EquipementSansZoneVM
    {
        /// <summary>
        /// Description contenant une référence à l'équipement et le problème retrouvé
        /// </summary>

        public string DescriptionProbleme { get; set; }

        /// <summary>
        /// String contenant la description des zones affectées
        /// </summary>
        public string ZoneImpacte { get; set; }

        /// <summary>
        /// Date debut pour créneau maintenance avec heure selon créneau
        /// </summary>
        public DateTime DateDebut { get; set; }

        /// <summary>
        /// Date fin pour créneau réservation avec l'heure selon créneau
        /// </summary>
        public DateTime DateFin { get; set; }

        /// <summary>
        /// Définition créneau pour chaque datepicker (Maintenance)
        /// </summary>
        //public string DatePickerDebut_Matin { get; set; }

        //public string DatePickerFin_Matin { get; set; }
    }
}
