using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using SiteGestionResaCore.Data.PcVue;
using SiteGestionResaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data
{
    public class EssaisXEntrepotDB: IEssaisXEntrepotDB
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private readonly GestionResaContext contextDB;
        private readonly ILogger<EssaisXEntrepotDB> logger;
        private readonly PcVueContext pcVueDb;

        public EssaisXEntrepotDB(
            GestionResaContext contextDB,
            ILogger<EssaisXEntrepotDB> logger,
            PcVueContext pcVueDb)
        {
            this.contextDB = contextDB;
            this.logger = logger;
            this.pcVueDb = pcVueDb;
        }
        public List<InfosResasSansEntrepot> ObtenirResasSansEntrepotUsr(utilisateur usr)
        {
            List<InfosResasSansEntrepot> infos = new List<InfosResasSansEntrepot>();
            //var essaiUsr = resaDB.essai.Where(e => e.compte_userID == IdUsr).ToList();
            var list = contextDB.essai.Where(e => e.entrepot_cree == null && e.compte_userID == usr.Id && e.resa_refuse!=true && e.resa_supprime != true).ToList();
            foreach(var ess in list)
            {
                // Obtenir le projet pour l'essai X
                var proj = contextDB.projet.First(p => p.id == ess.projetID);
                InfosResasSansEntrepot info = new InfosResasSansEntrepot { idEssai = ess.id, DateSaisieEssai = ess.date_creation,
                                                                            idProj = proj.id, MailRespProj = proj.mailRespProjet, 
                                                                            NomProjet = proj.titre_projet, NumProjet = proj.num_projet, TitreEssai = ess.titreEssai};
                infos.Add(info);
            }

            return infos;
        }

        /// <summary>
        /// Obtenir des infos sur un projet à partir son Id
        /// </summary>
        /// <param name="id">id projet</param>
        /// <returns></returns>
        public InfosProjet ObtenirInfosProjet(int id)
        {
            var proj = contextDB.projet.First(p => p.id == id);

            InfosProjet infos = new InfosProjet()
            {
                DateCreation = proj.date_creation,
                Description = proj.description_projet,
                Financement = proj.financement,
                MailRespProj = proj.mailRespProjet,
                MailUsrSaisie = contextDB.Users.First(p => p.Id == proj.compte_userID).Email,
                NumProjet = proj.num_projet,
                Organisme = contextDB.organisme.First(o => o.id == proj.organismeID).nom_organisme,
                Provenance = proj.provenance,
                TitreProjet = proj.titre_projet,
                TypeProjet = proj.type_projet
            };
            return infos;
        }

        /// <summary>
        /// Obtenir les infos affichage essai à partir d'un id essai
        /// </summary>
        /// <param name="idEssai"></param>
        /// <returns></returns>
        public ConsultInfosEssaiChildVM ObtenirInfosEssai(int idEssai)
        {
            var essai = contextDB.essai.First(e => e.id == idEssai);

            ConsultInfosEssaiChildVM Infos = new ConsultInfosEssaiChildVM
            {
                id = essai.id,
                TitreEssai = essai.titreEssai,
                Confidentialite = essai.confidentialite,
                DateCreation = essai.date_creation,
                DestProd = essai.destination_produit,
                MailManipulateur = contextDB.Users.First(u => u.Id == essai.manipulateurID).Email,
                MailUser = contextDB.Users.First(u => u.Id == essai.compte_userID).Email,
                PrecisionProd = essai.precision_produit,
                ProveProd = essai.provenance_produit,
                QuantiteProd = essai.quantite_produit,
                TransportStlo = essai.transport_stlo,
                TypeProduitEntrant = essai.type_produit_entrant
            };
            return Infos;
        }

        /// <summary>
        /// Obtenir toutes les réservations pour un essai
        /// </summary>
        /// <param name="idEssai">id essai</param>
        /// <returns>liste des réservations</returns>
        public List<InfosReservation> InfosReservations(int idEssai)
        {
            List<InfosReservation> ListResas = new List<InfosReservation>();
            // récupérer toutes les réservations du projet 
            var reservations = contextDB.reservation_projet.Where(r => r.essaiID == idEssai).Distinct().ToList();

            foreach (var x in reservations)
            {
                InfosReservation resa = new InfosReservation()
                {
                    DateDebut = x.date_debut,
                    DateFin = x.date_fin,
                    NomEquipement = contextDB.equipement.First(e => e.id == x.equipementID).nom,
                    ZoneEquipement = (from zon in contextDB.zone
                                      from equi in contextDB.equipement
                                      where zon.id == equi.zoneID && equi.id == x.equipementID
                                      select zon.nom_zone).First()
                };
                ListResas.Add(resa);
            }
            return ListResas;
        }

        public List<ReservationsXEssai> ListeReservationsXEssai(int idEssai)
        {
            bool IsEquipPcVue = false;
            bool IsDataReady = false;
            string donneesDispo = "";

            List<ReservationsXEssai> list = new List<ReservationsXEssai>();
            var listeRe = contextDB.reservation_projet.Where(r => r.essaiID == idEssai).ToList();
            foreach(var resa in listeRe)
            {
                // Obtenir la liste des types des documents pour cet équipement
                List<string> typesDoc = ListeTypeDoc(contextDB.equipement.First(e => e.id == resa.equipementID).activiteID.Value);
                // Voir si le fichier PcVue est dispo
                (IsEquipPcVue, IsDataReady) = EquipementVsDonneesPcVue(resa);
                if(IsEquipPcVue == true && IsDataReady == true)
                {
                    donneesDispo = "Données disponibles";
                } 
                else if (IsEquipPcVue == true && IsDataReady == false)
                {
                    donneesDispo = "Données indisponibles";
                }
                else if (IsEquipPcVue != true )
                {
                    donneesDispo = "";
                }
                ReservationsXEssai r = new ReservationsXEssai
                {
                    IdEquipement = resa.equipementID,
                    idResa = resa.id,
                    NomEquipement = contextDB.equipement.First(e => e.id == resa.equipementID).nom, 
                    ListeTypeDocs = typesDoc,
                    FichierPcVue = donneesDispo
                };

                list.Add(r);
            }
            return list;
        }

        public List<type_document> ListeTypeDocuments()
        {
            return contextDB.type_document.ToList();
        }

        #region Méthodes complémentaires

        List<string> ListeTypeDoc(int ActiviteID)
        {
            List<string> ListType = new List<string>();
            string ExprRegular = @"[aA-zZ][^,]*"; // identifier les initiales des types de documents

            Regex Rg = new Regex(ExprRegular);
            var typeDocXActivite = contextDB.activite_pfl.First(a => a.id == ActiviteID);
            MatchCollection collection = Rg.Matches(typeDocXActivite.type_documents); 

            foreach(var col in collection)
            {
                string type = col.ToString();
                ListType.Add(type);
            }

            return ListType;
        }

        (bool IsEquipPcVue, bool IsDataReady) EquipementVsDonneesPcVue(reservation_projet resa)
        {
            bool IsEquipPcVue = false;
            DateTime dateDebut = new DateTime();
            bool IsDataReady = false; 

            string tableName = contextDB.equipement.First(e => e.id == resa.equipementID).nomTabPcVue;
            if (tableName != null)
            {
                IsEquipPcVue = true;
                // 2. Vérifier s'il y a des données à récupérer entre les dates de réservation de l'équipement
                // http://kosted.free.fr/pdf/rapport.pdf: Chrono qui représente un temps (Mois-Jour-AnnéeMinutes-Secondes) bien précis. La norme utilisée pour enregistrer ces informations est celle du
                // FILE TIME. Le FILE TIME est le temps écoulé en nanosecondes écoulés depuis le 1er Janvier 1601.
                dateDebut = resa.date_debut.AddHours(-3);
                dateDebut = dateDebut.AddYears(-1600);

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
                    case "tab_UA_UFMF":
                        query = (from donnees in pcVueDb.tab_UA_UFMF
                                 where donnees.Chrono >= dateDebut.Ticks
                                 select donnees).Any();
                        break;
                    case "tab_UA_ECREM":
                        query = (from donnees in pcVueDb.tab_UA_ECREM
                                 where donnees.Chrono >= dateDebut.Ticks
                                 select donnees).Any();
                        break;
                    default: // vérifier le cas des 2 tables pour l'évaporateur
                        string pattern = @"[\w]+";
                        Regex rg = new Regex(pattern);
                        MatchCollection collect = rg.Matches(tableName);
                        if (collect.Count <= 1) // ça veut dire que c'est un équipement dont on a pas l'acquisition de données (même si la table existe) 
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
            return (IsEquipPcVue, IsDataReady);
        }

        public string ObtenirNomActivite(int id)
        {
            return contextDB.activite_pfl.First(a => a.id == id).nom_activite;
        }

        #endregion
    }
}
