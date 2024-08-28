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

using SiteGestionResaCore.Areas.Reservation.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Models;
using SiteGestionResaCore.Models.EquipementsReserves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.ResasUser
{
    public interface IResasUserDB
    {
        Task<List<InfosResasUser>> ObtenirResasUserAsync(int IdUsr, string OpenPartialEssai, string OpenReservations, int IdEssai);

        essai ObtenirEssaiPourModif(int IdEssai);

        bool IsEssaiModifiableOuSupp(int IdEssai);

        List<ld_produit_in> ListProduitEntree();

        List<ld_provenance_produit> ListProveProduit();

        List<ld_destination> ListDestinationPro();

        int IdProvProduitToCopie(int IdEssai);

        int IdDestProduitToCopie(int IdEssai);

        int IdProduitInToCopie(int IdEssai);

        ConsultInfosEssaiChildVM ObtenirInfosEssai(int idEssai);

        bool UpdateConfidentialiteEss(essai ess, string confidentialite);

        bool UpdateManipID(essai ess, int selecManipulateurID);

        bool compareTypeProdEntree(string TypeProdEntrant, int SelProductId);

        bool UpdateProdEntree(essai essa, int prodEntId);

        bool UpdatePrecisionProd(essai essa, string precision);

        bool UpdateQuantiteProd(essai essa, int? quantite);

        bool compareProvProd(string provProduit, int SelProvId);

        bool UpdateProvProd(essai essa, int SelectProvProduitId);

        bool compareDestProd(string destination_produit, int selectDestProduit);

        bool UpdateDestProd(essai essa, int SelectDestProduit);

        bool UpdateTransport(essai essa, string TranspSTLO);

        bool UpdateTitre(essai essa, string commentEssai);

        List<InfosResasEquipement> ResasEssai(int id);

        reservation_projet ObtenirResa(int IdReservation);

        bool SupprimerResa(int idResa);

        bool AnnulerEssai(int IdEssai, string RaisonAnnulation);

        equipement ObtenirEquipement(int IdEquipement);

        List<ReservationsJour> DonneesCalendrierEquipement(bool IsForOneWeek, int idEquipement, DateTime? DateDu, DateTime? DateAu);

        bool ChangerDatesResa(DateTime dateDebut, DateTime dateFin, int IdResa);

        projet ObtenirProjet(int IdProjet);

        void UpdateStatusEssai(essai ess);

        Task<IList<utilisateur>> ObtenirUsersLogisticAsync();
    }
}
