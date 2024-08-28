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

using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.Capteur
{
    public interface ICapteurDB
    {
        List<CapteurXAffichage> ObtenirListCapteurs();

        List<equipement> ObtListEquipements();

        bool AjouterCapteur(string NomCapteur,string CodeCapteur, int SelectedPiloteID, DateTime DateProchaineVerifInt, DateTime DateProchaineVerifExt,
                    DateTime DateDerniereVerifInt, DateTime DateDerniereVerifExt, double periodInt, double periodExt, bool CapteurConforme, 
                    double EmtCapteur, double FacteurCorrectif, string unite, string comment);
        capteur ObtenirCapteur(int id);

        bool SupprimerCapteur(int idCapteur);

        equipement ObtenirEquipement(int idEquipement);

        bool UpdatePeriodiciteInt(capteur Capteur, double periodicite);

        bool UpdatePeriodiciteExt(capteur Capteur, double periodicite);

        bool UpdateDateProVerifInt(capteur capt, DateTime dateverif);

        bool UpdateDateProVerifExt(capteur capt, DateTime dateverif);

        bool UpdateDateDerniereVerifInt(capteur capt, DateTime dateverif);
        bool UpdateDateDerniereVerifExt(capteur capt, DateTime dateverif);

        bool UpdateFacteur(capteur capt, double facteur);

        bool UpdateEMT(capteur capt, double emt);

        bool UpdateCodeCapteur(capteur capt, string code);

        bool UpdateNomCapteur(capteur capt, string nom);

        bool UpdateConformite(capteur capt, bool conformite);
        double FacteurCorrectif(capteur capt);

        bool UpdateUnite(capteur capt, string unite);

        bool UpdateCommentaire(capteur capt, string comment);
    }
}
