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

using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    public interface IProjetEssaiResaDb
    {
        bool ProjetExists(string NumeroProjet);

        Task<bool> VerifPropieteProjetAsync(string numProjet, utilisateur usr);

        List<EssaiUtilisateur> ObtenirList_EssaisUser(string NumeroProjet);

        projet ObtenirProjet_pourCopie(string numProjet);

        essai ObtenirEssai_pourCopie(int essaiID);

        int IdTypeProjetPourCopie(int IdProjet);

        int IdFinancementPourCopie(int IdProjet);

        int IdRespoProjetPourCopie(int IdProjet);

        int IdProvenancePourCopie(int IdProjet);

        int IdProvProduitPourCopie(int IdEssai);

        int IdDestProduitPourCopie(int IdEssai);

        int IdProduitInPourCopie(int IdEssai);

        projet CreationProjet(string TitreProjet, int typeProjetId, int financId, int orgId,
          int respProjetId, string numProj, int provProj, string description, DateTime dateCreation, utilisateur User);
        
        essai CreationEssai(projet pr, utilisateur Usr, DateTime myDateTime, string confidentialite, int manipId, int ProdId, string precisionProd, int? QuantProd,
                int ProvId, int destProduit, string TransStlo, string commentaire);

        void UpdateEssai(essai Essai, DateTime dateInf, DateTime dateSup);

        projet ObtenirProjXEssai(int projetID);

        void UpdateStatusEssai(essai essai);

        List<reservation_projet> ObtenirResasEssai(int IdEssai);
    }
}
