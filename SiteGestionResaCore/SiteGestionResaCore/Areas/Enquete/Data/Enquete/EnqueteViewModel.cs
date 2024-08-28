/*
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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Data
{
    public class EnqueteViewModel
    {

        public int IdEnquete { get; set; }
        public string TitreEssai { get; set; }
        public int IdEssai { get; set; }
        public string NumProjet { get; set; }
        public string TitreProj { get; set; }

        #region 1 Méthodes

        [Range(1, 4, ErrorMessage = "Note requise")]
        //[Display(Name = "Logiciel de réservation: ")]
        [Display(Name = "Est-ce que le logiciel de réservation satisfait vos attentes (réservation des pilotes, récupération des données,...)?: ")]
        public int LogicielMeth { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        //[Display(Name = "Accesibilité/Clarté des procédures (équipements, nettoyage/désinfection, accès à la PFL,...: ")]
        [Display(Name = "Les procédures relatives aux modalités d'utilisation de la PFL, à l'utilisation des matériels, nettoyage étaient claires?")]
        public int AccessibiliteMeth { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentMéthodes { get; set; }
        #endregion

        #region 2 Matériels (outils de travail)

        [Range(1, 4, ErrorMessage = "Note requise")]
        //[Display(Name = "Disponibilité: ")]
        [Display(Name = "Est-ce que les matériels sont disponibles facilement?: ")]
        public int DisponibiliteMat { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        //[Display(Name = "Opérationnabilité: ")]
        [Display(Name = "Les matériels sont-ils en bon état de fonctionnement et de propreté?: ")]
        public int OperationnabiliteMat { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "L'assistance technique a-t-elle été satisfaisante?: ")]
        public int MaintenanceMat { get; set; }

        //[Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Cochez cette case si vous n'êtes pas concerné par la rubrique")]
        public bool IsNotConcerneMaintMat { get; set; }
       

        [Display(Name = "Commentaire: ")]
        public string CommentMateriels { get; set; }
        #endregion

        #region 3 Milieu

        [Range(1, 4, ErrorMessage = "Note requise")]
        //[Display(Name = "Hygiène/Securité/Ergonomie/Réglementation: ")]
        [Display(Name = "L'état de propreté était satisfaisant?: ")]
        public int HygieneMil { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        //[Display(Name = "Confidentialité: ")]
        [Display(Name = "La confidentialité de vos essais était assurée?: ")]
        public int ConfidentMil { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Est-ce que votre environnement de travail était sécure?: ")]
        public int SecureMil { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Le materiel annexe aux appareils était disponible?: ")]
        public int MaterielMil { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Comment jugez vous le fonctionnement des énergies et fluides au cours de votre essai?: ")]
        public int EnergieMil { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentaireMil { get; set; }

        #endregion

        #region 4 Matière
        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "L'approvisionnement de matière première était satisfaisant?: ")]
        public int ApprovisioMatiere { get; set; }

        //[Required(ErrorMessage = "Champ requis")]
        [Display(Name = "Cochez cette case si vous n'êtes pas concerné par la rubrique")]
        public bool IsNotConcerneApproviMat { get; set; }
        
        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "Disponibilité des produits de nettoyage?: ")]
        public int NettoMatiere { get; set; }

        [Range(1, 4, ErrorMessage = "Note requise")]
        [Display(Name = "L'approvisionnement des consommables (essuie-main, surchaussures,...) était satisfaisant?: ")]
        public int MaterielMatiere { get; set; }

        [Display(Name = "Commentaire: ")]
        public string CommentaireMatiere { get; set; }
        #endregion

    }
}
