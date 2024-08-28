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
using SiteGestionResaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data
{
    public interface IEssaisXEntrepotDB
    {
        public List<InfosResasSansEntrepot> ObtenirResasSansEntrepotUsr(utilisateur usr);
        public InfosProjet ObtenirInfosProjet(int id);
        public ConsultInfosEssaiChildVM ObtenirInfosEssai(int idEssai);
        public List<InfosReservation> InfosReservations(int idEssai);
        public List<ReservationsXEssai> ListeReservationsXEssai(int idEssai);
        public List<type_document> ListeTypeDocuments();
        public string ObtenirNomActivite(int id);
        public int ObtenirIDActivite(int id);
        public List<type_document> ListeTypeDocumentsXActivite(int IdActivite);
        public string ObtenirNomTypeDonnees(int IdTypeDonnees);
        public List<activite_pfl> ObtenirListActiviteXResa(int IdResa);
        public bool EcrireDocTypeUn(CreationEntrepotVM model);
        public bool EcrireDocTypeDeux(CreationEntrepotVM model);
        public void UpdateEssaiXEntrepot(int idEssai);
        public reservation_projet ObtenirResa(int IdResa);
        public essai ObtenirEssai(int idEssai);
        public bool SaveDocEssaiPgd(doc_essai_pgd doc, string typedoc);
        public equipement ObtenirEquipement(int IdEquip);
        public projet ObtenirProjetXEssai(int IdProjet);
        public organisme ObtenirOrgXProj(int IdOrg);
        public string ObtenirTitreEssai(int idEssai);
        public bool SaveDateCreationEntrepot(int idProjet);
        public int ObtIdActiviteXEquip(int IdEquipement);
    }
}
