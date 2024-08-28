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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    public class FormulaireResaDb: IFormulaireResaDb
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext context;
        private readonly UserManager<utilisateur> userManager;

        //private readonly UserManager<utilisateur> userManager;
        //private readonly ILogger<FormulaireResaDb> logger;

        public FormulaireResaDb(
            GestionResaContext resaDB,
            UserManager<utilisateur> userManager)
        {
            this.context = resaDB;
            this.userManager = userManager;
        }

        /// <summary>
        /// Obtenir la liste des options type de projet 
        /// </summary>
        /// <returns>List<ld_type_projet></returns>
        public List<ld_type_projet> ObtenirList_TypeProjet()
        {
            return context.ld_type_projet.ToList();
        }

        /// <summary>
        /// Obtenir les options liste financement
        /// </summary>
        /// <returns>liste des options financement</returns>
        public List<ld_financement> ObtenirList_Financement()
        {
            return context.ld_financement.ToList();
        }

        /// <summary>
        /// Liste des organismes déclarés sur la BDD
        /// </summary>
        /// <returns>liste des organismes </returns>
        public List<organisme> ObtenirListOrg()
        {
            return context.organisme.ToList();
        }

        /// <summary>
        /// Retrouver les utilisateurs dont le compte a déjà été validé et surtout le mail confirmé via le lien envoyé par mail
        /// </summary>
        /// <returns> liste des utilisateurs</returns>
        public List<utilisateur> ObtenirList_UtilisateurValide()
        {
            return context.Users.Where(e => e.EmailConfirmed == true && e.compteInactif != true).OrderBy(u => u.nom).ToList(); 
        }

        /// <summary>
        /// Obtenir liste des options provenance produit
        /// </summary>
        /// <returns>List<ld_provenance_produit></returns>
        public List<ld_provenance_produit> ObtenirList_ProvenanceProduit()
        {
            return context.ld_provenance_produit.ToList();
        }

        /// <summary>
        /// Obtenir la liste des options provenance projet
        /// </summary>
        /// <returns> Liste options provenance</returns>
        public List<ld_provenance> ObtenirList_ProvenanceProjet()
        {
            return context.ld_provenance.ToList();
        }

        /// <summary>
        /// Obtenir liste des options type de produit d'entrée
        /// </summary>
        /// <returns> liste des options produit entrée</returns>
        public List<ld_produit_in> ObtenirList_TypeProduitEntree()
        {
            return context.ld_produit_in.ToList();
        }

        /// <summary>
        /// Obtenir liste des options destinaition produit sortie
        /// </summary>
        /// <returns></returns>
        public List<ld_destination> ObtenirList_DestinationPro()
        {
            return context.ld_destination.ToList();
        }

        public async Task<IList<utilisateur>> ObtenirLogisticUsersAsync()
        {
            return await userManager.GetUsersInRoleAsync("Logistic");
        }

        public async Task<IList<utilisateur>> ObtenirMainAdmUsersAsync()
        {
            return await userManager.GetUsersInRoleAsync("MainAdmin");
        }

        /// <summary>
        /// Liste des organismes déclarés sur la BDD
        /// </summary>
        /// <returns>liste des organismes </returns>
        public List<ld_equipes_stlo> ObtenirListEquips()
        {
            return context.ld_equipes_stlo.ToList();
        }
    }
}
