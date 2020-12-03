using Microsoft.Extensions.Logging;
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

                // TODO: A SEPARER DANS UNE AUTRE METHODE! :

                /*bool isEssaiModifiable = false; // boolean pour indiquer si on ajoute ou pas de lien pour voir les infos
                bool isOnlyConsultation = false;

                // vérifier si l'essai est modifiable ou pas en regardant les réservations (dates et confidentialité) 
                var resas = resaDB.reservation_projet.Where(r => r.essaiID == i.id).ToList();

                bool IsFirstSearch = true;
                foreach (var x in resas)
                {
                    if(i.confidentialite == EnumConfidentialite.Confidentiel.ToString())
                    {
                        // la date début de l'essai est déjà calculé
                        dateBegin = i.date_inf_confidentiel.Value;
                    }
                    else
                    {
                        // Pour tous les autres cas, retrouver la date à laquel commence l'essai (date la plus récente)
                        if(IsFirstSearch == true)
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

                // vérifier si la date la plus récente a lieu plus tard qu'aujourd'hui
               if(dateBegin.Date > DateTime.Now.Date)
                    isEssaiModifiable = true;*/

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
    }
}
