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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data
{
    public class CreationEntrepotVM
    {
        public int idEssai { get; set; }
        public string TitreEssai { get; set; }
        public List<ReservationsXEssai> ListReservationsXEssai { get; set; }
        public List<type_document> ListeTypeDoc { get; set; }
        
        public string NomActivite { get; set; }
        public int IDActivite { get; set; }

        public List<DocAjoutePartieUn> ListDocsPartieUn { get; set; }
        public List<DocAjoutePartieDeux> ListDocsPartieDeux { get; set; }
        public List<DocAjoutePartieUn> ListDocsProduit { get; set; }
        public List<DocAjoutePartieUn> ListDocsProcede { get; set; }
        
        /// <summary>
        /// Id d'un item de la liste des responsables projet
        /// </summary>
        [Required]
        [Display(Name = "Type document*")]
        [Range(1, 20, ErrorMessage = "Sélectionnez un type de document")]
        public int TypeDocumentID { get; set; }

        public IEnumerable<SelectListItem> TypeDocumentItem { get; set; } // Liste pour sélectionner le type de document

        /// <summary>
        /// Id d'un item de la liste des responsables projet
        /// </summary>
        [Required]
        [Display(Name = "Type activité*")]
        [Range(1, 20, ErrorMessage = "Sélectionnez un type d'activité")]
        public int TypeActiviteID { get; set; }

        public IEnumerable<SelectListItem> TypeActiviteItem { get; set; } // Liste pour sélectionner le type de document

        public string NomEquipement { get; set; }
        public int IDEquipement { get; set; }
        public int idResa { get; set; }
        public string NomBoutonPage { get; set; }
        public bool AjoutDocs { get; set; }
        public bool AjoutDocsAdmin { get; set; }

    }
}
