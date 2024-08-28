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

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.User.Data.Profil;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Controllers
{
    [Area("User")]
    public class ProfilController : Controller
    {
        private readonly UserManager<utilisateur> userManager;
        private readonly IProfilDB profilDB;

        public ProfilController(
            UserManager<utilisateur> userManager,
            IProfilDB profilDB)
        {
            this.userManager = userManager;
            this.profilDB = profilDB;
        }

        public async Task<IActionResult> ProfilUserAsync()
        {
            // Obtenir les infos de l'utilisateur authentifié
            var user = await userManager.FindByIdAsync(User.GetUserId());

            ProfilUserVM vm = new ProfilUserVM();
            vm.NomUsr = user.nom;
            vm.PrenomUsr = user.prenom;
            if(user.equipeID != null)
            {
                vm.NomEquipe = profilDB.ObtNomEquipe(user.equipeID.Value);
            }
            else
            {
                vm.NomEquipe = null;
            }

            if (user.organismeID != null)
            {
                vm.NomOrganisme = profilDB.ObtNomOrganisme(user.organismeID.Value);
            }
            else
            {
                vm.NomOrganisme = null;
            }

            vm.MailUsr = user.Email;
            vm.ListEnquetesNonRepondues = profilDB.ObtListEnquetes(user);
            return View("ProfilUser", vm);
        }
    }
}
