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

using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.AboutPFL.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.AboutPFL.Controllers
{
    [Area("AboutPFL")]
    public class InfosPflController : Controller
    {
        private readonly IZonePflEquipDB pflEquipDB;

        public InfosPflController(
            IZonePflEquipDB pflEquipDB)
        {
            this.pflEquipDB = pflEquipDB;
        }

        /// <summary>
        /// Ouverture de la vue pour affichage du plan PFL
        /// </summary>
        /// <returns></returns>
        public IActionResult PlanZones()
        {
            ZonesPflViewModel vm = new ZonesPflViewModel()
            {
                Zones = pflEquipDB.ListeZones()
            };

            return View("PlanZonesPFL",vm);
        }

        public IActionResult ZoneVsEquipements(int ?id)
        {
            List<InfosEquipement> listEquipVM = new List<InfosEquipement>();
            //List<equipement> equipements = new List<equipement>();
            // 1. Obtenir la liste des equipements
            listEquipVM = pflEquipDB.ListeEquipementsXZone(id.Value);
            // 2. For pour crée la liste des InfosEquipement
            
            string nomZone = pflEquipDB.NomZoneXEquipement(id.Value);
            EquipParZoneViewModel vm = new EquipParZoneViewModel()
            {
               ListeEquipements = listEquipVM,
               NomZone = nomZone
            };

            return View("EquipsVsZone", vm);
        }

        /// <summary>
        /// Méthode pour télécharger la doc depuis la bases des données
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DownloadFichMat(int? id)
        {
            doc_fiche_materiel doc = pflEquipDB.ObtenirDocMateriel(id.Value);
            return File(doc.contenu_fiche, System.Net.Mime.MediaTypeNames.Application.Octet, doc.nom_document);
        }
    }
}
