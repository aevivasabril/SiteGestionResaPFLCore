﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Maintenance
{
    /// <summary>
    /// Class Serializable (ouverture d'une session pour stocker des données en local) utilisée lors de la réservation des équipements 
    /// </summary>
    [Serializable]
    public class MaintenanceViewModel
    {
        #region Informations formulaire
        /// <summary>
        /// Id d'un intervenant maintenance
        /// </summary>
        [Required]
        [Display(Name = "Intervenant (*)")]
        [Range(1, 100, ErrorMessage = "Sélectionnez un intervenant maintenance")]
        public int SelectedIntervenantID { get; set; }

        public IEnumerable<SelectListItem> IntervenantItem { get; set; }

        /// <summary>
        /// Code maintenance généré automatiquement dans le code. Le generer avant le remplissage du formulaire
        /// </summary>
        [Display(Name = "Code Maintenance automatique: ")]
        public string CodeMaintenance { get; set; }

        /// <summary>
        /// Id d'un item de la liste type intervention maintenance
        /// </summary>
        [Required]
        [Display(Name = "Type d'intervention (*): ")]
        [Range(1, 100, ErrorMessage = "Sélectionnez un type intervention")]
        public int SelectedInterventionId { get; set; }

        public IEnumerable<SelectListItem> InterventionItem { get; set; }

        /// <summary>
        /// string à true ou false pour indiquer si l'intervenant est externe
        /// </summary>
        [Required(ErrorMessage = "Champ 'Intervenant externe' requis")]
        [Display(Name = "Intervenant externe (*): ")]
        public string IntervenantExterne { get; set; }

        /// <summary>
        /// String pour rajouter le nom de la societé externe
        /// </summary>
        /// Vérifier si le "Intervenant externe" est à True et si oui forcer la saisie de ce champ
        [Display(Name = "Nom de la société (*): ")]
        public string NomSociete { get; set; }


        /// <summary>
        /// String pour rajouter la description de l'opération maintenance
        /// </summary>
        [Required(ErrorMessage = "Veuillez ajouter une description de l'opération")]
        [Display(Name = "Description intervention (*): ")]
        public string DescriptionInter { get; set; }

        #endregion

        #region Equipements adjacents

        /// <summary>
        /// String pour rajouter le nom d'un équipement adjacent
        /// </summary>
        [Display(Name = "Nom equipement (*): ")]
        public string NomEquipement { get; set; }

        /// <summary>
        /// String pour rajouter le nom d'un équipement adjacent
        /// </summary>
        [Display(Name = "Zone Affectée (*): ")]
        public string ZoneAffecte { get; set; }

        #endregion

        /// <summary>
        /// String contenant le message à envoyer par mail pour comunniquer une panne
        /// </summary>
        [Display(Name = "Message mail (*): ")]
        public string MailMessage { get; set; }
    }

}
