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

using SiteGestionResaCore.Areas.StatistiquePFL.Data.Stats;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Data
{
    public interface IStatistiquesDB
    {
        List<InfosReservations> ObtenirResasDuAu(DateTime datedu, DateTime dateau);
        List<ZoneStats> ObtenirListZones();
        List<InfosEquipVsJours> ObtListEquipsVsJours(int idZone, int Annee);
        zone ObtNomZone(int id);
        List<projet> ObtenirListProjet();
        List<InfosReservations> ObtRecapitulatifXProjet(int IdProjet);
        projet ObtenirProjet(int IdProjet);
        List<ld_equipes_stlo> ObtenirListEquip();
        List<InfosReservations> ObtRecapitulatifXEquipe(int IdEquipeStlo, DateTime datedebut, DateTime datefin);
        ld_equipes_stlo ObtInfosEquipe(int IdEquipe);
        List<organisme> ListeOrganismes();
        List<InfosReservations> ObtRecapitulatifXOrg(int IdOrg, DateTime datedebut, DateTime datefin);
        organisme ObtenirOrganisme(int IdOrg);
        List<CategorieXProj> ListProjXProvenance(int IdProv);
        ld_provenance NomProvenance(int IdProvenance);
        List<ld_provenance> ListeProvenances();
        List<CategorieXProj> ListProjXNonProv();
        int LaitAnneeEnCours();
        int LaitXDates(DateTime dateDebut, DateTime dateFin);
        List<MaintenanceInfos> ListMaintenances(DateTime dateDu, DateTime dateAu);
        List<ld_type_projet> ListTypeProjet();
        List<CategorieXProj> ListProjetXType(int idType);
        ld_type_projet NomTypeProj(int Id);
        List<CategorieXProj> ListProjsSansType();
        List<ld_produit_in> ListProdsEntree();
        List<EssaiXprod> ListEssaisXprod(int idprod);
        string NomTypeProd(int idprod);
        List<EssaiXprod> ListEssaisSansprod();
    }
}
