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
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Models
{
    public class ConsultInfosEssaiChildVM
    {
        [DisplayName("N° Essai")]
        public int id { get; set; }
        [DisplayName("Date Création")]
        public DateTime DateCreation { get; set; }
        [DisplayName("Email manipulateur essai")]
        public string MailManipulateur { get; set; }
        [DisplayName("Type de produit entrant")]
        public string TypeProduitEntrant { get; set; }
        [DisplayName("Demande saisie par")]
        public string MailUser { get; set; }
        [DisplayName("Descriptif essai")]
        public string TitreEssai { get; set; }
        [DisplayName("Confidentialité essai")]
        public string Confidentialite { get; set; }
        [DisplayName("Transport assuré par")]
        public bool TransportStlo { get; set; }
        [DisplayName("Précision produit")]
        public string PrecisionProd { get; set; }
        [DisplayName("Quantité produit")]
        public int? QuantiteProd { get; set; }
        [DisplayName("Provenance produit")]
        public string ProveProd { get; set; }
        [DisplayName("Destination produit")]
        public string DestProd { get; set; }
    }
}
