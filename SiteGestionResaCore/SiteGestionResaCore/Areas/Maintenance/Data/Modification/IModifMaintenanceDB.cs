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

using SiteGestionResaCore.Areas.Maintenance.Data.Maintenance;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Modification
{
    public interface IModifMaintenanceDB
    {
        MaintenanceInfos ObtenirInfosMaint(string CodeIntervention);
        bool NumMaintExist(string CodeIntervention);
        List<EquipCommunXInterv> ListMaintAdj(int IdMaint);
        List<EquipPflXIntervention> ListIntervPFL(int IdMaint);
        DateTime ChangeDateFinEquipCommun(int IdResaCommun, DateTime NewDateFin);
        List<utilisateur> ObtenirListUtilisateursSite();
        maintenance ObtenirMaintenanceXInterv(int IdresaEquipComm);
        resa_maint_equip_adjacent ObtenirIntervEquiComm(int IdresaEquipComm);
        reservation_maintenance ObtenirIntervEquipPfl(int IdresaEquipPfl);
        maintenance ObtenirMaintenanceXIntervPFl(int IdresaEquipPfl);
        string ObtenirMailUser(int iduser);
        Task<IList<utilisateur>> List_utilisateurs_logistiqueAsync();
        void ChangeDateFinEquipPFL(int IdResaPfl, DateTime NewDateFin);
        bool ModifZoneDisponibleXIntervention(DateTime datefin, reservation_maintenance interv);
        bool ModifEquipementDisponibleXIntervention(DateTime datefin, reservation_maintenance interv);
        bool SupprimerMaintenance(int IdMaintenance, string raisonSupp);
        string ObtenirCodeIntervention(int IdMaintenance);
        maintenance ObtenirMaintenanceByID(int IdMaintenance);
        string NomEquipement(int IdEquipement);
        essai ObtenirEssai(int resaID);
        bool SupprimerReservation(int IDresa);
        bool VerifDisponibilitEquipSurInterventions(DateTime dateFin, reservation_maintenance interv);
        bool VerifDisponibilitZoneEquipSurInterventions(DateTime dateFin, reservation_maintenance interv);
        void UpdateStatusMaintenancePFLFinie(string action, int idMaint);
        void UpdateStatusMaintenanceCommFinie(string action, int idMaint);
    }
}
