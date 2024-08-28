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
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Data.PostEnquete
{
    public class HeadersNotesEnquetes
    {
        public string Projet = "Nom projet et N°";
        public string TitreEssai = "Titre essai";
        public string MailRespPro = "Mail responsable projet";
        public string DateFinManip = "Date fin des essais";
        public string MIQun = "I.1: Est-ce que le logiciel de réservation satisfait vos attentes (réservation des pilotes, récupération des données,...)?";
        public string MIQdeux = "I.2: Les procédures relatives aux modalités d'utilisation de la PFL, à l'utilisation des matériels, nettoyage étaient claires?";
        public string MIComm = "I. Commentaire";
        public string MIIQun = "II.1: Est-ce que les matériels sont disponibles facilement?";
        public string MIIQdeux = "II.2: Les matériels sont-ils en bon état de fonctionnement et de propreté?";
        public string MIIQtrois = "II.3: L'assistance technique a-t-elle été satisfaisante?";
        public string MIIQtroisConcerne = "II.3: NON Concerné par le champ II.3?";
        public string MIIComm = "II. Commentaire";
        public string MIIIQun = "III.1: L'état de proprète était satisfaisant?";
        public string MIIIQdeux = "III.2: La confidentialité de vos essais était assurée?";
        public string MIIIQtrois = "III.3: Est-ce que votre environnement de travail était secure?";
        public string MIIIQquatre = "III.4: Le materiel annexe aux appareils était disponible?";
        public string MIIIQcinq = "III.5: Comment jugez vous le fonctionnement des énergies et fluides au cours de votre essai?";
        public string MIIIComm = "III. Commentaire";
        public string MIVQun = "IV.1: L'approvisionnement de matière première était satisfaisant?";
        public string MIVQunConcerne = "IV: NON Concerné par le champ IV.1?";
        public string MIVQdeux = "IV.2: Disponibilité des produits de nettoyage?";
        public string MIVQtrois = "IV.3: L'approvisionnement des consommables (essuie-main, surchaussures,...) était satisfaisant?";
        public string MIVComm = "IV. Commentaire";
    }
}
