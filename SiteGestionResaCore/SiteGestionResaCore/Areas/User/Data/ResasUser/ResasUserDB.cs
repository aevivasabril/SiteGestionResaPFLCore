﻿using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Areas.Reservation.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
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

        public ResasUserDB(
            GestionResaContext resaDB,
            ILogger<ResasUserDB> logger)
        {
            this.resaDB = resaDB;
            this.logger = logger;
        }
        /// <summary>
        /// Obtenir les essais crées par l'utilisateur authentifié
        /// </summary>
        /// <param name="IdUsr">id utilisateur</param>
        /// <param name="openPartial">valeur pour le paramètre activant la vue partielle _ModifEssai : style=display:OpenPartial</param>
        /// <param name="IdEssai"> id essai dont l'ouverture de vue partielle est demandée</param>
        /// <returns></returns>
        public List<InfosResasUser> ObtenirResasUser(int IdUsr, string openPartial, int IdEssai)
        {
            List<InfosResasUser> List = new List<InfosResasUser>();
            InfosResasUser infos = new InfosResasUser();
            string StatusEssai = "";

            var essaiUsr = resaDB.essai.Where(e => e.compte_userID == IdUsr.ToString()).ToList();

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
                        StatusEssai = "Essai Validée";
                        break;
                    case "WaitingValidation":
                        StatusEssai = "Essai en attente de validation";
                        break;
                }

                // Vérifier si il faut afficher le partial view pour un des essais
                if(openPartial == null && IdEssai == 0) // pas d'affichage nécessaire
                {
                    infos = new InfosResasUser { CommentEssai = i.commentaire, DateCreation = i.date_creation,
                                        IdEssai = i.id, NumProjet = proj.num_projet,
                                        TitreProj = proj.titre_projet, StatusEssai = StatusEssai, OpenPartial = "none"};                 
                }
                else 
                { 
                    if(i.id == IdEssai) // montrer la vue partielle de cet essai
                    {
                        infos = new InfosResasUser { CommentEssai = i.commentaire, DateCreation = i.date_creation,
                                        IdEssai = i.id, NumProjet = proj.num_projet,
                                        TitreProj = proj.titre_projet, StatusEssai = StatusEssai, OpenPartial = openPartial};
                    }
                    else // montrer uniquement la vue partielle de l'essai demandé 
                    {
                        infos = new InfosResasUser { CommentEssai = i.commentaire, DateCreation = i.date_creation,
                                        IdEssai = i.id, NumProjet = proj.num_projet,
                                        TitreProj = proj.titre_projet, StatusEssai = StatusEssai, OpenPartial = "none"};
                    }
                }
                List.Add(infos);
            }
            return List;
        }

        public essai ObtenirEssaiPourModif(int IdEssai)
        {
            return resaDB.essai.FirstOrDefault(e => e.id == IdEssai);
        }

        public bool IsEssaiModifiable(int IdEssai)
        {
            DateTime dateBegin = new DateTime();

            var essai = resaDB.essai.First(e=>e.id == IdEssai);
            // si l'essai est refusé ou annulé alors essai non modifiable
            if (essai.status_essai == EnumStatusEssai.Refuse.ToString() || essai.status_essai == EnumStatusEssai.Canceled.ToString())
                return false;
            else // Essais modifiables mais uniquement si l'essai n'est pas passé
            {
                // vérifier si l'essai est modifiable ou pas en regardant les réservations (dates et confidentialité) 
                var resas = resaDB.reservation_projet.Where(r => r.essaiID == essai.id).ToList();

                bool IsFirstSearch = true;
                if (essai.confidentialite == EnumConfidentialite.Confidentiel.ToString())
                {
                    // la date début de l'essai est déjà calculé
                    dateBegin = essai.date_inf_confidentiel.Value;
                }
                else
                {
                    foreach (var x in resas)
                    {

                        // Pour tous les autres cas, retrouver la date à laquel commence l'essai (date la plus récente)
                        if (IsFirstSearch == true)
                        {
                            dateBegin = x.date_debut;
                        }
                        else
                        {
                            if (dateBegin > x.date_debut)
                                dateBegin = x.date_debut;
                        }
                    }
                }
                TimeSpan diff = dateBegin - DateTime.Now.Date;
                // vérifier si la date la plus récente a lieu plus tard une semaine avant
                if (diff.TotalDays >= 7) // pour comparer uniquement la date dd/mm/yy
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
        public ConsultInfosEssaiChilVM ObtenirInfosEssai(int idEssai)
        {
            var essai = resaDB.essai.First(e => e.id == idEssai);

            ConsultInfosEssaiChilVM Infos = new ConsultInfosEssaiChilVM
            {
                id = essai.id,
                Commentaire = essai.commentaire,
                Confidentialite = essai.confidentialite,
                DateCreation = essai.date_creation,
                DestProd = essai.destination_produit,
                MailManipulateur = resaDB.Users.First(u => u.Id == essai.manipulateurID).Email,
                MailUser = resaDB.Users.First(u => u.Id == Convert.ToInt32(essai.compte_userID)).Email,
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

        public bool UpdateQuantiteProd(essai essa, string quantite)
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

        public bool UpdateComment(essai essa, string commentEssai)
        {
            try
            {
                essa.commentaire = commentEssai;
                resaDB.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString(), "Problème lors de la MAJ '' pour un essai");
                return false;
            }
            return true;
        }
    }
}
