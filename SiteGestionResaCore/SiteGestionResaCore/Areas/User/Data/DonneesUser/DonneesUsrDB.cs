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

using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Areas.User.Data.ResasUser;
using SiteGestionResaCore.Data.Data;
using SiteGestionResaCore.Models.EquipementsReserves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiteGestionResaCore.Data.PcVue;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using SiteGestionResaCore.Areas.User.Data.DataPcVue;
using SiteGestionResaCore.Models;
using SiteGestionResaCore.Data;

namespace SiteGestionResaCore.Areas.User.Data.DonneesUser
{
    public class DonneesUsrDB: IDonneesUsrDB
    {
        private readonly GestionResaContext resaDB;
        private readonly ILogger<DonneesUsrDB> logger;
        private readonly PcVueContext pcVueDb;

        public DonneesUsrDB(
            GestionResaContext resaDB,
            ILogger<DonneesUsrDB> logger,
            PcVueContext pcVueDb)
        {
            this.resaDB = resaDB;
            this.logger = logger;
            this.pcVueDb = pcVueDb;
        }

        public List<InfosResa> ObtenirResasUser(int IdUsr)
        {
            List<InfosResa> List = new List<InfosResa>();
            InfosResa infos = new InfosResa();
            bool IsEquipUnderPcVue = false;

            var essaiUsr = resaDB.essai.Where(e => e.compte_userID == IdUsr && e.resa_refuse == null && e.resa_supprime == null).ToList().Distinct();

            foreach (var i in essaiUsr)
            {
                // Récupérer toutes les réservations pour cet essai
                var resasEssai = resaDB.reservation_projet.Where(r => r.essaiID == i.id).ToList();

                // Determiner si au moins un des équipements est sous supervision de données
                foreach (var resa in resasEssai)
                {
                    IsEquipUnderPcVue = (resaDB.equipement.First(e => e.id == resa.equipementID).nomTabPcVue != null);
                    if (IsEquipUnderPcVue == true)
                        break;
                }

                infos = new InfosResa
                {
                    NomProjet = resaDB.projet.First(p => p.id == i.projetID).titre_projet,
                    NumProjet = resaDB.projet.First(p => p.id == i.projetID).num_projet,
                    IdEssai = i.id,
                    TitreEssai = i.titreEssai,
                    EquipementSousPcVue = IsEquipUnderPcVue,
                    DateCreationEssai = i.date_creation

                };  
                List.Add(infos);
            }
            return List.OrderByDescending(x=>x.DateCreationEssai).ToList();
        }

        public ConsultInfosEssaiChildVM ObtenirInfosEssai(int IdEssai)
        {
            var essai = resaDB.essai.First(e => e.id == IdEssai);
            ConsultInfosEssaiChildVM vm = new ConsultInfosEssaiChildVM
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
            return vm;
        }

        public List<InfosResasEquipement> ListEquipVsDonnees(int IdEssai)
        {
            List<InfosResasEquipement> List = new List<InfosResasEquipement>();
            bool IsEquipPcVue = false;
            bool IsDataReady = false;
            string tableName;
            DateTime DateToday = DateTime.Now; // pour vérifier quelle date utiliser pour la requete!
            DateTime dateDebutPcVue = new DateTime();
            DateTime dateFinPcVue = new DateTime();

            /* Test pour déterminer que la deduction des dates est OK, juste une heure de décalage en moins sur la table pcVue donc 
             * soustraire une HEURE aux datetime pour obtenir la vrai valeur de chrono sur la table pcvue
             * Copie de db_archive faite à 19h56 le 08/02/2021
             * DateTime debutToSave = new DateTime(2021-1600, 02 , 08, 18, 55, 00, DateTimeKind.Local);*/

            // Récupérer toutes les réservations pour cet essai
            var lisEquip = resaDB.reservation_projet.Where(r => r.essaiID == IdEssai).ToList();

            // Determiner si au moins un des équipements est sous supervision des données et si les données sont disponibles
            foreach (var ResaEquip in lisEquip)
            {
                // Déterminer la date debut et fin pour vérifier s'il y a des données
                if (ResaEquip.date_debut <= DateToday && ResaEquip.date_fin <= DateToday) // Manip finie! 
                {
                    // convertir les dates fin et date debut réservation 
                    dateDebutPcVue = ResaEquip.date_debut.AddHours(-3);
                    dateDebutPcVue = dateDebutPcVue.AddYears(-1600);

                    // Vérifier le créneau pour ajouter ou enlever des heures
                    if (ResaEquip.date_fin.Hour == 12) // Finie la matinée vers midi alors rajouter une heure
                    {
                        dateFinPcVue = ResaEquip.date_fin.AddHours(-1); // on enleve une heure (conversion) et on rajoute une heure donc rien à rajouter
                        dateFinPcVue = dateFinPcVue.AddYears(-1600);
                    }
                    else // heure fin 18h, rajouter 6h c'est à dire 5h à cause de la conversion (-1h)
                    {
                        dateFinPcVue = ResaEquip.date_fin.AddHours(3);
                        dateFinPcVue = dateFinPcVue.AddYears(-1600);
                    }
                }
                else if (ResaEquip.date_debut <= DateToday && ResaEquip.date_fin >= DateToday) // Manip encore en cours!
                { // si la date est supérieur ou égal à la date d'aujourd'hui
                  // convertir les dates fin et date debut réservation 
                    dateDebutPcVue = ResaEquip.date_debut.AddHours(-3);
                    dateDebutPcVue = dateDebutPcVue.AddYears(-1600);

                    dateFinPcVue = DateToday;
                    dateFinPcVue = dateFinPcVue.AddYears(-1600);
                }

                // 1. Vérifier si l'équipement est sous pcvue et ensuite si les données sont disponibles
                tableName = resaDB.equipement.First(e => e.id == ResaEquip.equipementID).nomTabPcVue;
                if (tableName != null)
                {
                    IsEquipPcVue = true;
                    // 2. Vérifier s'il y a des données à récupérer entre les dates de réservation de l'équipement
                    // http://kosted.free.fr/pdf/rapport.pdf: Chrono qui représente un temps (Mois-Jour-AnnéeMinutes-Secondes) bien précis. La norme utilisée pour enregistrer ces informations est celle du
                    // FILE TIME. Le FILE TIME est le temps écoulé en nanosecondes écoulés depuis le 1er Janvier 1601.

                    // Méthode qui permet de définir la table sur laquelle on execute la requete
                    bool query = false;
                    switch (tableName)
                    {
                        case "tab_UA_ACT":
                            query = (from donnees in pcVueDb.tab_UA_ACT
                                         where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                         select donnees).Any();
                            break;
                        case "tab_UA_CUV":
                            query = (from donnees in pcVueDb.tab_UA_CUV
                                         where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                         select donnees).Any();
                            break;
                        case "tab_UA_GP7":
                            query = (from donnees in pcVueDb.tab_UA_GP7
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_MAT":
                            query = (from donnees in pcVueDb.tab_UA_MAT
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_MFMG":
                            query = (from donnees in pcVueDb.tab_UA_MFMG
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_MTH":
                            query = (from donnees in pcVueDb.tab_UA_MTH
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_NEP":
                            query = (from donnees in pcVueDb.tab_UA_NEP
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_OPTIMAL":
                            query = (from donnees in pcVueDb.tab_UA_OPTIMAL
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_SEC":
                            query = (from donnees in pcVueDb.tab_UA_SEC
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_SPI":
                            query = (from donnees in pcVueDb.tab_UA_SPI
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_VALO":
                            query = (from donnees in pcVueDb.tab_UA_VALO
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_UFMF":
                            query = (from donnees in pcVueDb.tab_UA_UFMF
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_ECREM":
                            query = (from donnees in pcVueDb.tab_UA_ECREM
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_UFFC":
                            query = (from donnees in pcVueDb.tab_UA_UFFC
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).Any();
                            break;
                        default: // vérifier le cas des 2 tables pour l'évaporateur
                            string pattern = @"[\w]+";
                            Regex rg = new Regex(pattern);
                            MatchCollection collect = rg.Matches(tableName);
                            if(collect.Count <=1) // ça veut dire que c'est un équipement dont on a pas l'acquisition de données (même si la table existe) 
                            {
                                IsEquipPcVue = false; // tab_UA_ECR a été ajouté mais aucune récup de données est implémenté
                                query = false;
                            }
                            else
                            {
                                for (int i = 0; i < collect.Count; i++)
                                {
                                    string table = collect[i].Value;
                                    // pour le cas de l'évapo A et B j'imagine ils sont synchros en terme des données (TODO: A verifier)
                                    if (table == "tab_UA_EVAA")
                                    {
                                        query = (from donnees in pcVueDb.tab_UA_EVAA
                                                 where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                                 select donnees).Any();
                                        if (query)
                                            break;
                                    }
                                    else
                                    {
                                        query = (from donnees in pcVueDb.tab_UA_EVAB
                                                 where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                                 select donnees).Any();
                                        if (query)
                                            break;
                                    }
                                }
                            }                                      
                            break;
                    }
                    // si la variable query est égal a true alors ça veut dire que des données sont disponibles pour récupération
                    if (query)
                        IsDataReady = true;
                    else
                        IsDataReady = false;
                }
                else
                {
                    IsEquipPcVue = false;
                    IsDataReady = false;
                }

                InfosResasEquipement Infos = new InfosResasEquipement
                {
                    DateDebut = ResaEquip.date_debut,
                    DateFin = ResaEquip.date_fin,
                    IdResa = ResaEquip.id,
                    NomEquipement = resaDB.equipement.First(e => e.id == ResaEquip.equipementID).nom,
                    ZoneEquipement = (from zon in resaDB.zone
                                      from equi in resaDB.equipement
                                      where zon.id == equi.zoneID && equi.id == ResaEquip.equipementID
                                      select zon.nom_zone).First(),
                    // 3. Vérifier si l'équipement est sous supervision
                    IsEquipUnderPcVue = IsEquipPcVue,
                    IsDataReady = IsDataReady
                };
                List.Add(Infos);
            }

            //IsEquipUnderPcVue = false;   
            return List;
        }

        public AllDataPcVue ObtenirDonneesPcVue(int idResa)
        {
            AllDataPcVue DataPcVue = new AllDataPcVue();
            List<DataPcVueEquip> OnlyData = new List<DataPcVueEquip>();
            DateTime dateDebutPcVue = new DateTime();
            DateTime dateFinPcVue = new DateTime();
            DateTime DateToday = DateTime.Now; // pour vérifier quelle date utiliser pour la requete!

            var resa = resaDB.reservation_projet.First(r => r.id == idResa);
            // Vérifier le nom de la table pour l'équipement
            equipement equipement = resaDB.equipement.First(e => e.id == resa.equipementID);

            if(resa.date_debut <= DateToday && resa.date_fin <= DateToday) // Manip finie! 
            {
                // convertir les dates fin et date debut réservation 
                dateDebutPcVue = resa.date_debut.AddHours(-3);
                dateDebutPcVue = dateDebutPcVue.AddYears(-1600);

                // Vérifier le créneau pour ajouter ou enlever des heures
                if (resa.date_fin.Hour == 12) // Finie la matinée vers midi alors rajouter une heure
                {
                    dateFinPcVue = resa.date_fin.AddHours(-1); // on enleve une heure (conversion) et on rajoute une heure donc rien à rajouter
                    dateFinPcVue = dateFinPcVue.AddYears(-1600);
                }
                else // heure fin 18h, rajouter 6h c'est à dire 5h à cause de la conversion (-1h)
                {
                    dateFinPcVue = resa.date_fin.AddHours(3);
                    dateFinPcVue = dateFinPcVue.AddYears(-1600);
                }
            }
            else if(resa.date_debut <= DateToday && resa.date_fin >= DateToday) // Manip encore en cours!
            { // si la date est supérieur ou égal à la date d'aujourd'hui
                // convertir les dates fin et date debut réservation 
                dateDebutPcVue = resa.date_debut.AddHours(-3);
                dateDebutPcVue = dateDebutPcVue.AddYears(-1600);

                dateFinPcVue = DateToday;
                dateFinPcVue = dateFinPcVue.AddYears(-1600);
            }

            //Application de la requete pour obtenir les infos selon la table PcVue
            // selon le nom de la table alors obtenir les données et les convertir au format globale
            //dateDebutPcVue = new DateTime(2021, 04, 21, 7, 0, 0);
            //dateFinPcVue = new DateTime(2021, 04, 22, 7, 0, 0);
            switch (equipement.nomTabPcVue)
            {
                case "tab_UA_ACT":
                    var queryAct = (from donnees in pcVueDb.tab_UA_ACT
                                  where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                  select donnees).ToList();
                    foreach (var donne in queryAct)
                    {
                        DataPcVueEquip DataPcV;
                        // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans
                        DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                        //Rajouter dans la liste des données PcVue
                        OnlyData.Add(DataPcV);
                    }
                    break;
                case "tab_UA_CUV":
                    var queryyCuv = (from donnees in pcVueDb.tab_UA_CUV
                                  where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                  select donnees).ToList();
                    foreach (var donne in queryyCuv)
                    {
                        DataPcVueEquip DataPcV;
                        // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans
                        DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                        //Rajouter dans la liste des données PcVue
                        OnlyData.Add(DataPcV);
                    }
                    break;
                case "tab_UA_GP7":
                    var queryyGp = (from donnees in pcVueDb.tab_UA_GP7
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).ToList();
                    foreach (var donne in queryyGp)
                    {
                        DataPcVueEquip DataPcV;
                        // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans
                        DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                        //Rajouter dans la liste des données PcVue
                        OnlyData.Add(DataPcV);
                    }
                    break;
                case "tab_UA_MAT":
                    var queryyMat = (from donnees in pcVueDb.tab_UA_MAT
                                    where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                    select donnees).ToList();
                    foreach (var donne in queryyMat)
                    {
                        DataPcVueEquip DataPcV;
                        // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans
                        DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                        //Rajouter dans la liste des données PcVue
                        OnlyData.Add(DataPcV);
                    }
                    break;
                case "tab_UA_MFMG":
                    var queryyMFMG = (from donnees in pcVueDb.tab_UA_GP7
                                    where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                    select donnees).ToList();
                    foreach (var donne in queryyMFMG)
                    {
                        DataPcVueEquip DataPcV;
                        // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans
                        DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                        //Rajouter dans la liste des données PcVue
                        OnlyData.Add(DataPcV);
                    }
                    break;
                case "tab_UA_MTH":
                    var queryyMth = (from donnees in pcVueDb.tab_UA_MTH
                                      where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                      select donnees).ToList();
                    foreach (var donne in queryyMth)
                    {
                        DataPcVueEquip DataPcV;
                        // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans
                        DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                        //Rajouter dans la liste des données PcVue
                        OnlyData.Add(DataPcV);
                    }
                    break;
                case "tab_UA_NEP":
                    var queryyNep = (from donnees in pcVueDb.tab_UA_NEP
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).ToList();
                    foreach (var donne in queryyNep)
                    {
                        DataPcVueEquip DataPcV;
                        // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans)
                        DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                        //Rajouter dans la liste des données PcVue
                        OnlyData.Add(DataPcV);
                    }
                    break;
                case "tab_UA_OPTIMAL":
                    var queryyOptimal = (from donnees in pcVueDb.tab_UA_OPTIMAL
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).ToList();
                    foreach (var donne in queryyOptimal)
                    {
                        DataPcVueEquip DataPcV;
                        // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans
                        DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                        //Rajouter dans la liste des données PcVue
                        OnlyData.Add(DataPcV);
                    }
                    break;
                case "tab_UA_SEC":
                    var queryySec = (from donnees in pcVueDb.tab_UA_SEC
                                         where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                         select donnees).ToList();
                    foreach (var donne in queryySec)
                    {
                        DataPcVueEquip DataPcV;
                        // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans
                        DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                        //Rajouter dans la liste des données PcVue
                        OnlyData.Add(DataPcV);
                    }
                    break;
                case "tab_UA_SPI":
                    var queryySpi = (from donnees in pcVueDb.tab_UA_SPI
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).ToList();
                    foreach (var donne in queryySpi)
                    {
                        DataPcVueEquip DataPcV;
                        // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans
                        DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                        //Rajouter dans la liste des données PcVue
                        OnlyData.Add(DataPcV);
                    }
                    break;
                case "tab_UA_VALO":
                    var queryyValo = (from donnees in pcVueDb.tab_UA_VALO
                                     where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                     select donnees).ToList();
                    foreach (var donne in queryyValo)
                    {
                        DataPcVueEquip DataPcV;
                        // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans
                        DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                        //Rajouter dans la liste des données PcVue
                        OnlyData.Add(DataPcV);
                    }
                    break;
                case "tab_UA_UFMF":
                    var queryyUfMf = (from donnees in pcVueDb.tab_UA_UFMF
                                      where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                      select donnees).ToList();
                    foreach (var donne in queryyUfMf)
                    {
                        DataPcVueEquip DataPcV;
                        // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans
                        DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                        //Rajouter dans la liste des données PcVue
                        OnlyData.Add(DataPcV);
                    }
                    break;
                case "tab_UA_ECREM":
                    var queryyEcrem = (from donnees in pcVueDb.tab_UA_ECREM
                                      where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                      select donnees).ToList();
                    foreach (var donne in queryyEcrem)
                    {
                        DataPcVueEquip DataPcV;
                        // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans
                        DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                        //Rajouter dans la liste des données PcVue
                        OnlyData.Add(DataPcV);
                    }
                    break;
                case "tab_UA_UFFC":
                    var queryyUffc = (from donnees in pcVueDb.tab_UA_UFFC
                                      where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                       select donnees).ToList();
                    foreach (var donne in queryyUffc)
                    {
                        DataPcVueEquip DataPcV;
                        // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans
                        DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                        //Rajouter dans la liste des données PcVue
                        OnlyData.Add(DataPcV);
                    }
                    break;
                default: // vérifier le cas des 2 tables pour l'évaporateur
                    //string pattern = @"[\w]+";
                    //Regex rg = new Regex(pattern);
                    var list = equipement.nomTabPcVue.Split(", ");
                    //MatchCollection collect = rg.Matches(NamePcVueTable);

                    for (int i = 0; i < list.Length; i++)
                    {
                        //string table = list[i];
                        // pour le cas de l'évapo A et B j'imagine ils sont synchros en terme des données (TODO: A verifier)
                        if (list[i] == "tab_UA_EVAA")
                        {
                            var queryA  = (from donnees in pcVueDb.tab_UA_EVAA
                                            where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                            select donnees).ToList();
                            foreach (var donne in queryA)
                            {
                                DataPcVueEquip DataPcV;
                                // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans
                                DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                                //Rajouter dans la liste des données PcVue
                                OnlyData.Add(DataPcV);
                            }
                            //if (query)
                            //break;
                        }
                        else
                        {
                            var queryB = (from donnees in pcVueDb.tab_UA_EVAB
                                          where donnees.Chrono >= dateDebutPcVue.Ticks && donnees.Chrono <= dateFinPcVue.Ticks
                                          select donnees).ToList();
                            foreach (var donne in queryB)
                            {
                                DataPcVueEquip DataPcV;
                                // Reconvertir la date à partir des secondes lus vers datetime (ajouter les 1600 ans)
                                DataPcV = new DataPcVueEquip { Chrono = new DateTime(donne.Chrono).AddYears(1600).ToLocalTime(), NomCapteur = donne.Name, Value = donne.Value };
                                //Rajouter dans la liste des données PcVue
                                OnlyData.Add(DataPcV);
                            }
                            //if (query)
                            //break;
                        }
                    }

                    break;
            }
            DataPcVue = new AllDataPcVue { DataEquipement = OnlyData, NomEquipement = resaDB.equipement.First(e => e.id == resa.equipementID).nom, NumGmao = equipement.numGmao };
            return DataPcVue;
        }

    }
}
