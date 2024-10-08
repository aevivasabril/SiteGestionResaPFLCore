﻿/*
 * website developped to manage the dairy platform STLO operations  
 * Code by Anny VIVAS, inspired from the operationnal functioning of the ancien website developped by Bruno PERRET  
 * July 2021
 * website includes code from:
 *  DotNetZip library for dealing with zip, bsip and zlib from .net 
 *  Created by: Henrik/Dino Chiesa
 * 
 *  MailKit open source library for .NET mail-client 
 *  Created by:  Jeffrey Stedfast
 * 
 *  Microsoft.AspNetCore.Identity.EntityFrameworkCore, ASP.NET Core Identity provider that uses Entity Framework Core
 *  Created by: Microsoft
 *  
 *  Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation, Runtime compilation support for Razor views and Razor pages in ASP.NET Core MVC
 *  Created by: Microsoft
 *
 *  Microsoft.EntityFrameworkCore.Design, Shared design-time components for Entity Framework Core tools
 *  Created by: Microsoft
 *
 *  Microsoft.EntityFrameworkCore.SqlServer, Microsoft SQL Server database provider for Entity Framework Core
 *  Created by: Microsoft
 *
 *  Ncrontab, NCrontab is crontab for all .NET runtimes supported by .NET Standard 1.0. It provides parsing and formatting of crontab expressions as well as calculation of occurrences of time based on a schedule expressed in the crontab format
 *  Created by: Atif Aziz
 *   
 * This projet is released under the terms of the GNU general public license GPL version 3 or later:
 * availaible here: https://www.gnu.org/licenses/gpl-3.0-standalone.html
 * 
 * Copyright (c) 2021-2024 Anny Vivas
 */

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    /// <summary>
    /// Class Serializable (ouverture d'une session pour stocker des données en local) utilisée lors de la réservation des équipements 
    /// </summary>
    [Serializable]
    public class FormulaireProjetViewModel
    {
        #region Copier un projet + essai

        #region Liste des essais correspondant au numéro projet

        // Numero du projet au format (12-345)
        [Display(Name = "Numéro de projet (*) (Ex format: 12-345 ou XX-123)")] // il faut respecter le numéro 
        [RegularExpression(@"^[A-Za-z0-9]{2}[-][0-9]{3}$", ErrorMessage = "Le numéro de projet ne respecte pas le format de la base action")]
        public string NumProjetXCopie { get; set; }

        /// <summary>
        /// boolean indiquant si un numéro de projet existe
        /// </summary>
        public bool ProjetValide { get; set; }

        /// <summary>
        /// Id de l'essai selectionné à partir du dropdownlist 
        /// </summary>
        [Display(Name = "Liste des Essais")]
        [Range(1, 100, ErrorMessage = "Sélectionnez un Essai")]
        public int SelectedEssaiId { get; set; }


        public IEnumerable<SelectListItem> EssaiItem { get; set; }

        #endregion

        #endregion

        #region champs "Projet"

        /// <summary>
        /// Titre du projet
        /// </summary>
        [Required(ErrorMessage = "Champ 'Titre projet' requis")]
        [Display(Name = "Titre Projet (*)")]
        [RegularExpression(@"^[\w][^\/;\\.!:*?,]*$", ErrorMessage = "Le format est incorrect, évitez les caractères suivants:  /;\\.!:*?,")]
        public string TitreProjet { get; set; }

        // Numero du projet au format (12-345)
        [Required(ErrorMessage = "Champ 'Numéro de projet' requis")]
        [Display(Name = "Numéro de projet (*) (Ex format: 12-345 ou XX-123)")] // il faut respecter le numéro 
        [RegularExpression(@"^[A-Za-z0-9]{2}[-][0-9]{3}$", ErrorMessage = "Le numéro de projet ne respecte pas le format de la base action")]
        public string NumProjet { get; set; }

        /// <summary>
        /// Description d'un projet 
        /// </summary>
        [Display(Name = "Description du projet")]
        public string DescriptionProjet { get; set; }

        #region Liste pour le type de projet

        /// <summary>
        /// Id d'un item selectionné pour le type projet
        /// </summary>
        [Display(Name = "Type de projet")]
        public int SelectTypeProjetId { get; set; }

        public IEnumerable<SelectListItem> TypeProjetItem { get; set; }

        #endregion

        #region Liste pour l'origine financement

        /// <summary>
        /// Id d'un item selectionnée pour le type de financement
        /// </summary>
        [Display(Name = "Origine Financement")]
        public int SelectFinancementId { get; set; }

        public IEnumerable<SelectListItem> TypefinancementItem { get; set;}

        #endregion

        #region Liste des organismes

        /// <summary>
        /// Id d'un item selectionné pour le organisme
        /// </summary>
        [Required]
        [Display(Name = "Nom de l'entreprise (*)")]
        [Range(1, 100, ErrorMessage = "Sélectionnez un organisme")]
        public int SelectedOrganId { get; set; }

        public IEnumerable<SelectListItem> OrganItem { get; set;}

        #endregion

        #region Liste des utilisateurs pour selection du responsable projet (uniquement utilisateurs avec un compte valide)

        /// <summary>
        /// Id d'un item de la liste des responsables projet
        /// </summary>
        [Required]
        [Display(Name = "Mail responsable projet (*)")]
        [Range(1, 100, ErrorMessage = "Sélectionnez un responsable projet")]
        public int SelectedRespProjId { get; set; }

        public IEnumerable<SelectListItem> RespProjItem { get; set; }

        #endregion

        #region Liste des options provenance projet

        /// <summary>
        /// Id d'un item de la liste provenance projet
        /// </summary>
        [Display(Name = "Provenance projet")]
        public int SelectedProvenanceId { get; set; }

        public IEnumerable<SelectListItem> ProvenanceItem { get; set; }

        #endregion

        #endregion

        #region Champs "Essai"

        #region Liste des personnes en charge de la manip

        /// <summary>
        /// Confidentialité du projet (restreint, ouvert ou confidentiel)
        /// </summary>
        [Required(ErrorMessage = "Champ 'Confidentialité' requis")]
        [Display(Name = "Confidentialité (*)")]
        public string ConfidentialiteEssai { get; set; }

        /// <summary>
        /// Id d'un item de la liste provenance manipulateur essai
        /// </summary>
        [Required]
        [Display(Name = "Personne en charge des manips (*)")]
        [Range(1, 100, ErrorMessage = "Sélectionnez un manipulateur pour l'essai")]
        public int SelectedManipulateurID { get; set; }

        public IEnumerable<SelectListItem> ManipProjItem { get; set; }

        #endregion

        #region Liste des produits entrée

        /// <summary>
        /// Id d'un item de la liste type produit d'entrée essai
        /// </summary>
        [Display(Name = "Type de produit d'entrée")]
        public int SelectedProductId { get; set; }

        public IEnumerable<SelectListItem> ProductItem { get; set; }

        #endregion
        /// <summary>
        /// String pour description sur le produit d'entrée
        /// </summary>
        [Display(Name = "Précision sur le produit: ")] 
        public string PrecisionProduitIn { get; set; }

        /// <summary>
        /// string Quantité de produit
        /// </summary>
        [Display(Name = "Quantité de produit (Kg):")]
        public int? QuantiteProduit { get; set; }

        #region Liste des provenances produits

        /// <summary>
        /// Id d'un item de la liste provenance du produit d'entrée essai
        /// </summary>
        [Display(Name = "Provenance produit")]
        public int SelectedProveProduitId { get; set; }

        public IEnumerable<SelectListItem> ProvenanceProduitItem { get; set; }


        #endregion

        #region Liste destination produits

        /// <summary>
        /// Id d'un item de la liste destinaison produit sortie essai
        /// </summary>
        [Display(Name = "Destination produits ")]
        public int SelectedDestProduit { get; set; }

        public IEnumerable<SelectListItem> DestProduitItem { get; set; }

        #endregion

        /// <summary>
        /// string à true ou false pour indiquer si le transport depend du STLO ou Autre
        /// </summary>
        [Required(ErrorMessage = "Champ 'Transport' requis")]
        [Display(Name = "Transport assuré par (*): ")]
        public string TransportSTLO { get; set; }

        /// <summary>
        /// String pour rajouter un commentaire dans le formulaire
        /// </summary>
        [Required(ErrorMessage = "Titre essai requis")]
        [Display(Name = "Titre ESSAI (Précisez des éléments de l'essai qui vous permettront de le retrouver plus facilement par la suite) (*)")]
        [RegularExpression(@"^[\w][^\/;\\.!:*?,]*$", ErrorMessage = "Le format est incorrect, évitez les caractères suivants:  /;\\.!:*?,")]
        public string TitreEssai { get; set; }

        #endregion

        #region Messages pour les tooltip des types de confidentialité

        public string ReservationOuverte = "Equipement réservé. Vous pourriez avoir d'autres utilisateurs autour de vous";

        public string ReservationRestreint = "Equipement réservé, et pré-blocage de tous les autres équipements de la zone." +
            " Utiliser ce mode de confidentialité uniquement si vous avez besoin de bloquer TOUTE la zone. Ex: essai qui nécessite beaucoup de place autour de vous";

        public string ReservationConfidentielle = "Si l'équipement réservé est sur la PFL, alors blocage de toute la PFL. " +
            "Si l'équipement réservé est dans une salle alimentaire alors blocage de toute la salle. Utiliser ce mode qu'en cas de stricte nécessité";
        #endregion
    }

    #region Enum pour établir des valeurs définis

    public enum EnumStatusEssai
    {
        WaitingValidation,
        Validate,
        Refuse,
        Canceled
    }

    public enum EnumZonesPfl
    {
        SalleAp7A = 18,
        SalleAp7B = 19,
        SalleAp7C = 20,
        SalleAp6 = 13,
        SalleAp5 = 12,
        SalleAp9 = 16,
        SalleAp8 = 15,
        Saumurage = 10,
        PatesMoulage = 5,
        PatesTranchage = 6,
        Labo = 11,
        PatesCuites = 7,
        PrepLait = 3,
        Membranes = 4,
        Innovation = 8,
        EquipMobiles = 17,
        Depot_Stockage = 2,
        Concent_Sech = 1,
        SalleStephan = 9
    }

    public enum EnumConfidentialite
    {
        Ouvert,
        Restreint,
        Confidentiel
    }
    #endregion
}