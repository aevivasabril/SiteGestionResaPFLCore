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

            var essaiUsr = resaDB.essai.Where(e => e.compte_userID == IdUsr).ToList();

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
                    EquipementSousPcVue = IsEquipUnderPcVue

                };
                //IsEquipUnderPcVue = false;   
                List.Add(infos);
            }
            return List;
        }

        public ConsultInfosEssaiChilVM ObtenirInfosEssai(int IdEssai)
        {
            var essai = resaDB.essai.First(e => e.id == IdEssai);
            ConsultInfosEssaiChilVM vm = new ConsultInfosEssaiChilVM
            {
                id = essai.id,
                Commentaire = essai.commentaire,
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
            DateTime dateDebut;
            DateTime dateFin;
            DateTime DateToday = DateTime.Now;
            string tableName;
            

            // Récupérer toutes les réservations pour cet essai
            var lisEquip = resaDB.reservation_projet.Where(r => r.essaiID == IdEssai).ToList();

            // Determiner si au moins un des équipements est sous supervision de données et si les données sont disponibles
            foreach (var ResaEquip in lisEquip)
            {
                // 1. Vérifier si l'équipement est sous pcvue et ensuite si les données sont disponibles
                tableName = resaDB.equipement.First(e => e.id == ResaEquip.equipementID).nomTabPcVue;
                if (tableName != null)
                {
                    IsEquipPcVue = true;
                    // 2. Vérifier s'il y a des données à récupérer entre les dates de réservation de l'équipement
                    // http://kosted.free.fr/pdf/rapport.pdf: Chrono qui représente un temps (Mois-Jour-AnnéeMinutes-Secondes) bien précis. La norme utilisée pour enregistrer ces informations est celle du
                    // FILE TIME. Le FILE TIME est le temps écoulé en nanosecondes écoulés depuis le 1er Janvier 1601.
                    dateDebut = ResaEquip.date_debut;
                    dateDebut = dateDebut.AddYears(-1600);
                    //dateDebut = dateDebut.AddHours(-2);
                    // convertir à ticks pour obtenir un format bigint? dateDebut.Ticks;

                    dateFin = ResaEquip.date_fin;
                    dateFin = dateFin.AddYears(-1600);
                    // convertir au format la date de consultation
                    DateToday = DateToday.AddYears(-1600);
                    //dateFin = dateFin.AddHours(-2);

                    //TypeConverter typeConverter = TypeDescriptor.GetConverter(pcVueDb.tab_UA_ACT);
                    //object propValue = typeConverter.ConvertFromString(tableName);

                    // Méthode qui permet de définir la table sur laquelle on execute la requete
                    bool query = false;
                    switch (tableName)
                    {
                        case "tab_UA_ACT":
                            query = (from donnees in pcVueDb.tab_UA_ACT
                                         where donnees.Chrono >= dateDebut.Ticks
                                         select donnees).Any();
                            break;
                        case "tab_UA_CUV":
                            query = (from donnees in pcVueDb.tab_UA_CUV
                                         where donnees.Chrono >= dateDebut.Ticks
                                         select donnees).Any();
                            break;
                        /*case "tab_UA_EVAA":
                            query = (from donnees in pcVueDb.tab_UA_EVAA
                                     where donnees.Chrono >= dateDebut.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_EVAB":
                            query = (from donnees in pcVueDb.tab_UA_EVAB
                                     where donnees.Chrono >= dateDebut.Ticks
                                     select donnees).Any();
                            break;*/
                        case "tab_UA_GP7":
                            query = (from donnees in pcVueDb.tab_UA_GP7
                                     where donnees.Chrono >= dateDebut.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_MAT":
                            query = (from donnees in pcVueDb.tab_UA_MAT
                                     where donnees.Chrono >= dateDebut.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_MFMG":
                            query = (from donnees in pcVueDb.tab_UA_MFMG
                                     where donnees.Chrono >= dateDebut.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_MTH":
                            query = (from donnees in pcVueDb.tab_UA_MTH
                                     where donnees.Chrono >= dateDebut.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_NEP":
                            query = (from donnees in pcVueDb.tab_UA_NEP
                                     where donnees.Chrono >= dateDebut.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_OPTIMAL":
                            query = (from donnees in pcVueDb.tab_UA_OPTIMAL
                                     where donnees.Chrono >= dateDebut.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_SEC":
                            query = (from donnees in pcVueDb.tab_UA_SEC
                                     where donnees.Chrono >= dateDebut.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_SPI":
                            query = (from donnees in pcVueDb.tab_UA_SPI
                                     where donnees.Chrono >= dateDebut.Ticks
                                     select donnees).Any();
                            break;
                        case "tab_UA_VALO":
                            query = (from donnees in pcVueDb.tab_UA_VALO
                                     where donnees.Chrono >= dateDebut.Ticks
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
                                                 where donnees.Chrono >= dateDebut.Ticks
                                                 select donnees).Any();
                                        if (query)
                                            break;
                                    }
                                    else
                                    {
                                        query = (from donnees in pcVueDb.tab_UA_EVAB
                                                 where donnees.Chrono >= dateDebut.Ticks
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

        /*public DbSet<SiteGestionResaCore.Data.PcVue> PcVueTable(string tableName)
        {
            switch (tableName)
            {
                case "tab_UA_ACT":
                    return pcVueDb.Set<tab_UA_ACT>();
                case "tab_UA_CUV":
                    return pcVueDb.Set<tab_UA_CUV>();
                case "tab_UA_EVAA":
                    return pcVueDb.Set<tab_UA_EVAA>();
                case "tab_UA_EVAB":
                    return pcVueDb.Set<tab_UA_EVAB>();
                case "tab_UA_GP7":
                    return pcVueDb.Set<tab_UA_GP7>();
                case "tab_UA_MAT":
                    return pcVueDb.Set<tab_UA_MAT>();
                case "tab_UA_MFMG":
                    return pcVueDb.Set<tab_UA_MFMG>();
                case "tab_UA_MTH":
                    return pcVueDb.Set<tab_UA_MTH>();
                case "tab_UA_NEP":
                    return pcVueDb.Set<tab_UA_NEP>();
                case "tab_UA_OPTIMAL":
                    return pcVueDb.Set<tab_UA_OPTIMAL>();
                case "tab_UA_SEC":
                    return pcVueDb.Set<tab_UA_SEC>();
                case "tab_UA_SPI":
                    return pcVueDb.Set<tab_UA_SPI>();
                case "tab_UA_VALO":
                    return pcVueDb.Set<tab_UA_VALO>();
                default:

            } 
        }*/
    }
}
