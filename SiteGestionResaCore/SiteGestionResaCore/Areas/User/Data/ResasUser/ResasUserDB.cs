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
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Areas.Reservation.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using SiteGestionResaCore.Models;
using SiteGestionResaCore.Models.EquipementsReserves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.ResasUser
{
    public class ResasUserDB : IResasUserDB
    {
        private readonly GestionResaContext resaDB;
        private readonly ILogger<ResasUserDB> logger;
        private readonly UserManager<utilisateur> userManager;
        private readonly IReservationDb reservationDb;

        public ResasUserDB(
            GestionResaContext resaDB,
            ILogger<ResasUserDB> logger,
            UserManager<utilisateur> userManager,
            IReservationDb reservationDb)
        {
            this.resaDB = resaDB;
            this.logger = logger;
            this.userManager = userManager;
            this.reservationDb = reservationDb;
        }
        /// <summary>
        /// Obtenir les essais crées par l'utilisateur authentifié
        /// </summary>
        /// <param name="IdUsr">id utilisateur</param>
        /// <param name="OpenPartialEssai">valeur pour le paramètre activant la vue partielle _ModifEssai : style=display:OpenPartial</param>
        /// <param name="IdEssai"> id essai dont l'ouverture de vue partielle est demandée</param>
        /// <returns></returns>
        public async Task<List<InfosResasUser>> ObtenirResasUserAsync(int IdUsr, string OpenPartialEssai, string OpenReservations, int IdEssai)
        {
            List<InfosResasUser> List = new List<InfosResasUser>();
            InfosResasUser infos = new InfosResasUser();
            string StatusEssai = "";
            bool ReadyToSupp = false;

            var essaiUsr = resaDB.essai.Where(e => e.compte_userID == IdUsr).ToList();

            foreach(var i in essaiUsr)
            {
                // récupérer le projet auquel appartient l'essai
                var proj = resaDB.projet.First(p => p.id == i.projetID);                
                switch (i.status_essai)
                {
                    case "Canceled":
                        StatusEssai = "Essai Annulé par vous"; // ajouter du lien info mais pas de bouton pour modification
                        //isEssaiModifiable = false;
                        break;
                    case "Refuse":
                        StatusEssai = "Essai Refusé";
                        //isEssaiModifiable = false;
                        break;
                    case "Validate":
                        StatusEssai = "Essai Validé";
                        break;
                    case "WaitingValidation":
                        StatusEssai = "Essai en attente de validation";
                        break;
                }

                // Obtenir les infos de l'utilisateur authentifié
                var user = await userManager.FindByIdAsync(IdUsr.ToString());
                // Obtenir les roles et vérifier s'il est "Logistic"
                var allUserRoles = await userManager.GetRolesAsync(user);
                bool IsLogistic = false;
                // Vérifier si la personne est dans le groupe des "Logistic"
                if (allUserRoles.Contains("Logistic"))
                {
                    IsLogistic = true;
                }
                if (IsLogistic)
                    ReadyToSupp = true;
                else // vérifier si cet essai est modifiable           
                    ReadyToSupp = IsEssaiModifiableOuSupp(i.id);

                // Vérifier si il faut afficher le partial view infos essai pour un des essais
                if (IdEssai != 0) // signifie que une des vues infos essai ou infos réservation devra être affichée
                {
                    if(i.id == IdEssai && OpenPartialEssai!=null) // montrer la vue partielle de cet essai
                    {
                        infos = new InfosResasUser { TitreEssai = i.titreEssai, DateCreation = i.date_creation,
                                        IdEssai = i.id, NumProjet = proj.num_projet,
                                        TitreProj = proj.titre_projet, StatusEssai = StatusEssai, OpenPartialEssai = OpenPartialEssai, OpenReservations = "none", IsCanceledAutorised = ReadyToSupp};

                    }else if (i.id == IdEssai && OpenReservations != null)
                    {
                        infos = new InfosResasUser { TitreEssai = i.titreEssai, DateCreation = i.date_creation,
                                        IdEssai = i.id, NumProjet = proj.num_projet,
                                        TitreProj = proj.titre_projet, StatusEssai = StatusEssai, OpenPartialEssai = "none", OpenReservations = OpenReservations, IsCanceledAutorised = ReadyToSupp};
                    }
                    else
                    {
                        infos = new InfosResasUser { TitreEssai = i.titreEssai, DateCreation = i.date_creation,
                                        IdEssai = i.id, NumProjet = proj.num_projet,
                                        TitreProj = proj.titre_projet, StatusEssai = StatusEssai, OpenPartialEssai = "none", OpenReservations="none", IsCanceledAutorised = ReadyToSupp};
                    }
                }
                else
                {
                    infos = new InfosResasUser { TitreEssai = i.titreEssai, DateCreation = i.date_creation,
                                        IdEssai = i.id, NumProjet = proj.num_projet,
                                        TitreProj = proj.titre_projet, StatusEssai = StatusEssai, OpenPartialEssai = "none", OpenReservations="none", IsCanceledAutorised = ReadyToSupp};
                }

                List.Add(infos);
                //List = List.OrderByDescending(x => x.DateCreation).ToList();
            }
            return List.OrderByDescending(x => x.DateCreation).ToList();
        }

        public essai ObtenirEssaiPourModif(int IdEssai)
        {
            return resaDB.essai.FirstOrDefault(e => e.id == IdEssai);
        }

        public bool IsEssaiModifiableOuSupp(int IdEssai)
        {
            var essai = resaDB.essai.First(e=>e.id == IdEssai);
            var user = resaDB.Users.First(u => u.Id == essai.compte_userID);

            // si l'essai est refusé ou annulé alors essai non modifiable
            if (essai.status_essai == EnumStatusEssai.Refuse.ToString() || essai.status_essai == EnumStatusEssai.Canceled.ToString())
                return false;
            else // Essais modifiables mais uniquement si l'essai n'est pas passé
            {
                // vérifier si l'essai est modifiable ou pas en regardant les réservations (dates et confidentialité) 
                var resas = resaDB.reservation_projet.Where(r => r.essaiID == essai.id).ToList();

                // Retrouver la date la plus recente des réservations
                //var dateInf = resas.OrderBy(r => r.date_debut).ToList();
                // Retrouver la date fin la plus ancienne des réservations
                var dateInf = resas.OrderByDescending(r => r.date_fin).ToList(); 

                TimeSpan diff = DateTime.Today - dateInf[0].date_fin;
                // Il faut que l'utilisateur puisse modifier la réservation 15 jours (pour avoir la vrai utilisation)
                // après début de l'essai s'il a un pb d'approvissionement ou en anticipant un problème
                if (diff.Days <= 15)  // 
                    return true;
                else
                    return false;
            }          
        }

        /// <summary>
        /// Obtenir liste des options type de produit d'entrée
        /// </summary>
        /// <returns> liste des options produit entrée</returns>
        public List<ld_produit_in> ListProduitEntree()
        {
            return resaDB.ld_produit_in.ToList();
        }

        /// <summary>
        /// Obtenir liste des options provenance produit
        /// </summary>
        /// <returns>List<ld_provenance_produit></returns>
        public List<ld_provenance_produit> ListProveProduit()
        {
            return resaDB.ld_provenance_produit.ToList();
        }

        /// <summary>
        /// Obtenir liste des options destinaition produit sortie
        /// </summary>
        /// <returns></returns>
        public List<ld_destination> ListDestinationPro()
        {
            return resaDB.ld_destination.ToList();
        }

        /// <summary>
        /// obtenir l'id provenance produit
        /// </summary>
        /// <param name="IdEssai"> id essai</param>
        /// <returns> id prov produit</returns>
        public int IdProvProduitToCopie(int IdEssai)
        {
            int idProvPro;
            essai ess = resaDB.essai.FirstOrDefault(u => u.id == IdEssai);
            if (ess.provenance_produit != null)
            {
                idProvPro = (from prov in resaDB.ld_provenance_produit
                             from essai in resaDB.essai
                             where (essai.id == IdEssai) && (essai.provenance_produit == prov.nom_provenance_produit)
                             select prov.id).First();
            }
            else
            {
                idProvPro = -1;
            }
            return idProvPro;
        }

        /// <summary>
        /// obtenir id option destinaison produit sortie
        /// </summary>
        /// <param name="IdEssai"> id essai</param>
        /// <returns>id destinaison produit</returns>
        public int IdDestProduitToCopie(int IdEssai)
        {
            int idProvProd;
            essai es = resaDB.essai.FirstOrDefault(u => u.id == IdEssai);
            if (es.destination_produit != null)
            {
                idProvProd = (from dest in resaDB.ld_destination
                              from essai in resaDB.essai
                              where (essai.id == IdEssai) && (essai.destination_produit == dest.nom_destination)
                              select dest.id).First();
            }
            else
            {
                idProvProd = -1;
            }
            return idProvProd;
        }

        /// <summary>
        /// Obtenir id produit entrée
        /// </summary>
        /// <param name="IdEssai"></param>
        /// <returns>id produit entrée</returns>
        public int IdProduitInToCopie(int IdEssai)
        {
            int idProdIn;
            essai es = resaDB.essai.FirstOrDefault(u => u.id == IdEssai);
            if (es.type_produit_entrant != null)
            {
                idProdIn = (from prod in resaDB.ld_produit_in
                            from essai in resaDB.essai
                            where (essai.id == IdEssai) && (essai.type_produit_entrant == prod.nom_produit_in)
                            select prod.id).First();
            }
            else
            {
                idProdIn = -1;
            }
            return idProdIn;
        }

        /// <summary>
        /// Obtenir les infos affichage essai à partir d'un id essai
        /// </summary>
        /// <param name="idEssai"></param>
        /// <returns></returns>
        public ConsultInfosEssaiChildVM ObtenirInfosEssai(int idEssai)
        {
            var essai = resaDB.essai.First(e => e.id == idEssai);

            ConsultInfosEssaiChildVM Infos = new ConsultInfosEssaiChildVM
            {
                id = essai.id,
                TitreEssai = essai.titreEssai,
                Confidentialite = essai.confidentialite,
                DateCreation = essai.date_creation,
                DestProd = essai.destination_produit,
                MailManipulateur = resaDB.Users.First(u => u.Id == essai.manipulateurID).Email,
                MailUser = resaDB.Users.First(u => u.Id == essai.compte_userID).Email,
                PrecisionProd = essai.precision_produit,
                ProveProd = essai.provenance_produit,
                QuantiteProd = essai.quantite_produit,
                TransportStlo = essai.transport_stlo,
                TypeProduitEntrant = essai.type_produit_entrant
            };
            return Infos;
        }

        public bool UpdateConfidentialiteEss(essai ess, string confidentialite)
        {   
            try
            {
                ess.confidentialite = confidentialite;
                resaDB.SaveChanges();
            }
            catch(Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ de confidentialité pour un essai");
                return false;
            }
            return true;
        }

        public bool UpdateManipID(essai ess, int selecManipulateurID)
        {
            try
            {
                ess.manipulateurID = selecManipulateurID;
                resaDB.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ manipulateur ID pour un essai");
                return false;
            }
            return true;
        }

        public bool compareTypeProdEntree(string TypeProdEntrant, int SelProductId)
        {
            return resaDB.ld_produit_in.First(t => t.nom_produit_in == TypeProdEntrant).id == SelProductId;
        }

        public bool UpdateProdEntree(essai essa, int prodEntId)
        {
            try
            {
                essa.type_produit_entrant = resaDB.ld_produit_in.First(p=>p.id == prodEntId).nom_produit_in;
                resaDB.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ 'Produit entrée' pour un essai");
                return false;
            }
            return true;
        }

        public bool UpdatePrecisionProd(essai essa, string precision)
        {
            try
            {
                essa.precision_produit = precision;
                resaDB.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ 'précision produit' pour un essai");
                return false;
            }
            return true;
        }

        public bool UpdateQuantiteProd(essai essa, int? quantite)
        {
            try
            {
                essa.quantite_produit = quantite;
                resaDB.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ 'quantite Produit' pour un essai");
                return false;
            }
            return true;
        }

        public bool compareProvProd(string provProduit, int SelProvId)
        {
            return resaDB.ld_provenance_produit.First(p => p.nom_provenance_produit == provProduit).id == SelProvId;
        }

        public bool UpdateProvProd(essai essa, int SelectProvProduitId)
        {
            try
            {
                essa.provenance_produit = resaDB.ld_provenance_produit.First(p=>p.id == SelectProvProduitId).nom_provenance_produit;
                resaDB.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ 'Provenance Produit' pour un essai");
                return false;
            }
            return true;
        }

        public bool compareDestProd(string destination_produit, int selectDestProduit)
        {
            return resaDB.ld_destination.First(d => d.nom_destination == destination_produit).id == selectDestProduit;
        }

        public bool UpdateDestProd(essai essa, int SelectDestProduit)
        {
            try
            {
                essa.destination_produit = resaDB.ld_destination.First(d => d.id == SelectDestProduit).nom_destination;
                resaDB.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ 'Destination produit' pour un essai");
                return false;
            }
            return true;
        }

        public bool UpdateTransport(essai essa, string TranspSTLO)
        {
            try
            {
                essa.transport_stlo = Convert.ToBoolean(TranspSTLO);
                resaDB.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ 'Transport' pour un essai");
                return false;
            }
            return true;
        }

        public bool UpdateTitre(essai essa, string commentEssai)
        {
            try
            {
                essa.titreEssai = commentEssai;
                resaDB.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ '' pour un essai");
                return false;
            }
            return true;
        }

        public List<InfosResasEquipement> ResasEssai(int id)
        {
            List<InfosResasEquipement> list = new List<InfosResasEquipement>();

            var resas = resaDB.reservation_projet.Where(r => r.essaiID == id).Distinct().ToList();
            foreach (var r in resas)
            {
                InfosResasEquipement infoRes = new InfosResasEquipement() {
                    IdResa = r.id, DateDebut = r.date_debut, DateFin = r.date_fin,
                    NomEquipement = resaDB.equipement.First(e => e.id == r.equipementID).nom, 
                    ZoneEquipement = (from zon in resaDB.zone
                                      from equi in resaDB.equipement
                                      where zon.id == equi.zoneID && equi.id == r.equipementID
                                      select zon.nom_zone).First()
                };
                list.Add(infoRes);
            }

            return list;
        }

        public reservation_projet ObtenirResa(int IdReservation)
        {
            return resaDB.reservation_projet.First(r => r.id == IdReservation);
        }

        public bool SupprimerResa(int idResa)
        {
            try
            {
                reservation_projet resa = resaDB.reservation_projet.First(r => r.id == idResa);
                resaDB.reservation_projet.Remove(resa);
                resaDB.SaveChanges();
            }
            catch(Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la suppression réservation");
                return false;
            }
            return true;
        }

        public bool AnnulerEssai(int IdEssai, string RaisonAnnulation)
        {
            bool IsChangeOk = false;

            int retry = 0;

            var essai = resaDB.essai.First(e => e.id == IdEssai);
            essai.status_essai = EnumStatusEssai.Canceled.ToString();
            essai.date_validation = DateTime.Now;
            essai.resa_supprime = true;
            essai.raison_suppression = RaisonAnnulation;
            while (retry < 3 && IsChangeOk != true)
            {
                try
                {
                    resaDB.SaveChanges();
                    IsChangeOk = true;
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Erreur lors de la validation de l'essai N° :" + IdEssai);
                    retry++;
                }
            }

            return IsChangeOk;
        }

        public equipement ObtenirEquipement(int IdEquipement)
        {
            return resaDB.equipement.First(r => r.id == IdEquipement);
        }

        /// <summary>
        /// Méthode permettant de retourner les réservations à afficher sur le calendrier
        /// </summary>
        /// <param name="IsForOneWeek"> boolean true ou false pour indiquer si l'affichage est de 7 jours ou pas</param>
        /// <param name="idEquipement"> id de l'équipement</param>
        /// <param name="DateDu"> date à partir de laquelle on récupére les réservations</param>
        /// <param name="DateAu"> date jusqu'à laquelle on récupére les réservations</param>
        /// <returns> liste des réservations </returns>
        public List<ReservationsJour> DonneesCalendrierEquipement(bool IsForOneWeek, int idEquipement, DateTime? DateDu, DateTime? DateAu)
        {
            // Variables 
            List<ReservationsJour> ListResas = new List<ReservationsJour>();  // Liste des réservations pour une semaine ou pour une durée déterminée
            ReservationsJour ResaJour = new ReservationsJour(); // Reservation pour une journée defini 
            DateTime TodayDate = new DateTime(); // Datetime pour obtenir la date actuelle
            DateTime DateRecup = new DateTime();
            int NbJours = 0;

            //Afficher toujours à partir du lundi de la semaine en cours 
            TodayDate = DateTime.Now;
            DayOfWeek dow = TodayDate.DayOfWeek;

            #region Déterminer s'il s'agit d'un calendrier pour une semaine ou pour une durée déterminée

            switch (IsForOneWeek)
            {
                case true:
                    // Revenir toujours au lundi de la semaine en cours
                    if (dow == DayOfWeek.Tuesday) TodayDate = TodayDate.AddDays(-1);
                    if (dow == DayOfWeek.Wednesday) TodayDate = TodayDate.AddDays(-2);
                    if (dow == DayOfWeek.Thursday) TodayDate = TodayDate.AddDays(-3);
                    if (dow == DayOfWeek.Friday) TodayDate = TodayDate.AddDays(-4);
                    if (dow == DayOfWeek.Saturday) TodayDate = TodayDate.AddDays(-5);
                    if (dow == DayOfWeek.Sunday) TodayDate = TodayDate.AddDays(-6);

                    NbJours = 7;
                    // Ajouter l'heure (à 7h00) car sinon on va avoir un problème pour faire la comparaison lors de la récupération des "essai"
                    DateRecup = new DateTime(TodayDate.Year, TodayDate.Month, TodayDate.Day, 7, 0, 0, DateTimeKind.Local);
                    break;
                case false:
                    // Calculer la difference des jours entre la date debut d'affichage et la date fin d'affichage
                    if (DateDu.Equals(DateAu))
                    {
                        // Si la personne souhaite afficher qu'un jour pour le calendrier, afficher la semaine pour éviter d'afficher qu'une colonne
                        NbJours = 7;
                    }
                    else
                    {
                        NbJours = (DateAu.Value - DateDu.Value).Days;
                        NbJours = NbJours + 1;  // Car il compte pas la bonne quantité 
                        if (NbJours < 7) // Afficher au moins une semaine 
                            NbJours = 7;

                    }
                    // Ajouter l'heure (à 7h00) car sinon on va avoir un problème pour faire la comparaison lors de la récupération des "essai"
                    DateRecup = new DateTime(DateDu.Value.Year, DateDu.Value.Month, DateDu.Value.Day, 7, 0, 0, DateTimeKind.Local);
                    break;
            }

            #endregion

            #region Recueil des réservations pour la durée demandée

            // for pour recupérer les réservations des N jours à partir du lundi
            for (int i = 0; i < NbJours; i++)
            {
                // Obtenir l'emploi du temps du jour de la semaine i pour un équipement
                ResaJour = reservationDb.ObtenirReservationsJourEssai(DateRecup, idEquipement);
                // ajouter à la liste de la semaine
                ListResas.Add(ResaJour);
                // incrementer le nombre des jours à partir du lundi
                DateRecup = DateRecup.AddDays(1);
            }

            #endregion 

            return ListResas;
        }

        public bool ChangerDatesResa(DateTime dateDebut, DateTime dateFin, int IdResa)
        {
            try
            {
                var resa = resaDB.reservation_projet.First(r => r.id == IdResa);
                resa.date_debut = dateDebut;
                resa.date_fin = dateFin;
                resaDB.SaveChanges();
            }
            catch(Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la suppression réservation");
                return false;
            }

            return true;
        }

        public projet ObtenirProjet(int IdProjet)
        {
            return resaDB.projet.First(p => p.id == IdProjet);
        }

        public void UpdateStatusEssai(essai essai)
        {
            essai.status_essai = EnumStatusEssai.WaitingValidation.ToString();
            resaDB.SaveChanges();
        }

        /// <summary>
        /// Obtenir la liste des admin "logistic" 
        /// </summary>
        /// <returns></returns>
        public async Task<IList<utilisateur>> ObtenirUsersLogisticAsync()
        {
            return await userManager.GetUsersInRoleAsync("Logistic");
        }
    }
}
