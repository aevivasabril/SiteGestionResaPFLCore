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

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    public class ProjetEssaiResaDb: IProjetEssaiResaDb
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext context;
        private readonly UserManager<utilisateur> userManager;
        private readonly ILogger<ProjetEssaiResaDb> logger;

        public ProjetEssaiResaDb(
            GestionResaContext projEssaiDb,
            UserManager<utilisateur> userManager,
            ILogger<ProjetEssaiResaDb> logger)
        {
            this.context = projEssaiDb;
            this.userManager = userManager;
            this.logger = logger;
        }

        /// <summary>
        /// Vérifier que le projet existe
        /// </summary>
        /// <param name="NumProjet"></param>
        /// <returns>true ou false</returns>
        public bool ProjetExists(string NumProjet)
        {
            int countProj = context.projet.Where(p => p.num_projet == NumProjet).Count();

            // si compteur est égal à zéro alors le numéro de projet n'existe pas 
            if (countProj == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Cette méthode permette de vérifier si l'utilisateur est propiètaire du projet (soit il appartient aux admins ou
        /// c'est une personne 'utilisateur' qui a crée ce projet)
        /// </summary>
        /// <param name="numProjet">numero de projet</param>
        /// <param name="IdAsp">id table aspnetusers</param>
        /// <returns>true ou false</returns>
        public async Task<bool> VerifPropieteProjetAsync(string numProjet, utilisateur usr)
        {
            IList<string> allUserRoles = await userManager.GetRolesAsync(usr);
            bool propProjOk = false;
            bool adminSiteOk = false;
            bool organismeOk = false;
            bool equipeOk = false;

            #region Vérifier que le numéro de projet existe et que la personne qui fait la réservation est la propiètaire du projet

            //propProjOk = context.projet.Where(p => p.num_projet == numProjet).Where(p => p.compte_userID == usr.Id).Any(); // PAS BON car j'ai besoin de comparer l'id user au parametre compte_userID (string)
            propProjOk = (from p in context.projet
                    where p.num_projet == numProjet && p.compte_userID == usr.Id
                    select p).Any();
            #endregion

            #region Si la personne n'est pas propiètaire du projet mais qu'elle est "Admin" ou "MainAdmin"
            foreach (string roles in allUserRoles)
            {
                if (roles == "Admin" || roles == "MainAdmin")
                {
                    adminSiteOk = true;
                    break;
                }
            }
            #endregion

            #region Autoriser les personnes d'une même organisation pour copier les infos du code projet

            var propProjet = (from pr in context.projet
                              where pr.num_projet == numProjet
                              select pr.compte_userID).First();

            var auteur = await userManager.FindByIdAsync(propProjet.ToString());

            if(usr.organismeID == auteur.organismeID) // vérifier que le propiètaire appartient au même organisme de la personne qui saisi la demande
            {
                if (usr.organismeID != 1) // sauf si c'est l'inrae ne pas autoriser pour le moment
                {
                    organismeOk = true;
                }
                else
                { // si l'organisme c'est l'inrae
                    if(usr.equipeID != null)
                    {
                        // Si l'organisme est "INRAE" alors vérifier l'appartenance à une même équipe
                        if (usr.equipeID.Value == auteur.equipeID.Value)
                            // Alors permettre la copie à la personne d'une même équipe
                            equipeOk = true;
                    }                    
                }
            }
            else
            {
                // si la personne n'appartient pas au même organisme mais qu'ils sont dans la même équipe (cas particulier de françoise)
                if(usr.equipeID.HasValue != false && auteur.equipeID.HasValue != false) // s'une des personnes ou les 2 n'ont pas d'équipe alors false
                {
                    if (usr.equipeID.Value == auteur.equipeID.Value)
                        equipeOk = true;
                }
                else
                {
                    equipeOk = false;
                }

            }
            #endregion
            return (propProjOk || adminSiteOk || organismeOk || equipeOk);
        }

        /// <summary>
        /// Trouver les essais pour un numéro de projet défini et leur manipulateur
        /// </summary>
        /// <param name="NumeroProjet"></param>
        /// <returns>Liste du type "EssaiUtilisateur"</returns>
        public List<EssaiUtilisateur> ObtenirList_EssaisUser(string NumeroProjet)
        {
            return context.essai.Include(e => e.manipulateur).Where(e => e.projet.num_projet == NumeroProjet).ToList()
                .Select(e => new EssaiUtilisateur { CopieEssai = e, user = e.manipulateur }).ToList();
        }

        /// <summary>
        /// Récupérer un projet
        /// </summary>
        /// <param name="NumProjet">Numéro de projet</param>
        /// <returns>projet</returns>
        public projet ObtenirProjet_pourCopie(string NumProjet)
        {
            projet pr = new projet();
            try
            {
                pr = context.projet.FirstOrDefault(p => p.num_projet == NumProjet);
            }
            catch(Exception e)
            {
                logger.LogError("Problème pour récupérer le projet associé: " + e.Message);               
            }
            
            /*pr = (from proj in context.projet
                  where proj.num_projet == NumProjet
                  select proj).First();*/

            return pr;
        }

        /// <summary>
        /// Récupérer un essai par son ID
        /// </summary>
        /// <param name="idEssaie"> id essai</param>
        /// <returns>Essai</returns>
        public essai ObtenirEssai_pourCopie(int idEssaie)
        {
            /*essai ess = new essai();

            ess = (from essai in context.essai
                   where essai.id == idEssaie
                   select essai).First();*/

            return context.essai.FirstOrDefault(e=>e.id == idEssaie);
        }

        /// <summary>
        /// Obtenir un type projet pour copie
        /// </summary>
        /// <param name="IdProjet">id projet</param>
        /// <returns>id type projet</returns>
        public int IdTypeProjetPourCopie(int IdProjet)
        {
            int idType;
            projet proj = context.projet.FirstOrDefault(u => u.id == IdProjet);
                        
            try
            {
                idType = (from typeprojet in context.ld_type_projet
                            from pro in context.projet
                            where (pro.id == IdProjet) && (pro.type_projet == typeprojet.nom_type_projet)
                            select typeprojet.id).First();
            }
            catch (Exception e)
            {
                logger.LogError("Problème pour récupérer le projet associé: " + e.Message);
                idType = -1;
            }
           
            return idType;
        }

        /// <summary>
        /// Obtenir Id option type de financement
        /// </summary>
        /// <param name="IdProjet">Id projet</param>
        /// <returns>id financement option</returns>
        public int IdFinancementPourCopie(int IdProjet)
        {
            int idTypeF;
            projet proj = context.projet.FirstOrDefault(u => u.id == IdProjet);
            if (proj.financement != null)
            {
                idTypeF = (from typefinanc in context.ld_financement
                           from pro in context.projet
                           where (pro.id == IdProjet) && (pro.financement == typefinanc.nom_financement)
                           select typefinanc.id).First();
            }
            else
            {
                idTypeF = -1;
            }
            return idTypeF;
        }

        /// <summary>
        /// Obtenir responsable projet
        /// </summary>
        /// <param name="IdProjet"> id projet</param>
        /// <returns>id responsable projet</returns>
        public int IdRespoProjetPourCopie(int IdProjet)
        {
            int idResp;
            projet pro = context.projet.FirstOrDefault(u => u.id == IdProjet);
            if (pro.mailRespProjet != null)
            {
                idResp = (from resp in context.Users
                          from proj in context.projet
                          where (proj.id == IdProjet) && (proj.mailRespProjet == resp.Email)
                          select resp.Id).First();
            }
            else
            {
                idResp = -1;
            }
            return idResp;
        }

        /// <summary>
        /// obtenir l'id provenance projet
        /// </summary>
        /// <param name="IdProjet"> id projet</param>
        /// <returns>id provenance</returns>
        public int IdProvenancePourCopie(int IdProjet)
        {
            int idProv;
            projet pro = context.projet.FirstOrDefault(u => u.id == IdProjet);
            if (pro.provenance != null)
            {
                idProv = (from prov in context.ld_provenance
                          from proj in context.projet
                          where (proj.id == IdProjet) && (proj.provenance == prov.nom_provenance)
                          select prov.id).First();
            }
            else
            {
                idProv = -1;
            }
            return idProv;
        }

        /// <summary>
        /// obtenir l'id provenance produit
        /// </summary>
        /// <param name="IdEssai"> id essai</param>
        /// <returns> id prov produit</returns>
        public int IdProvProduitPourCopie(int IdEssai)
        {
            int idProvPro;
            essai ess = context.essai.FirstOrDefault(u => u.id == IdEssai);
            if (ess.provenance_produit != null)
            {
                idProvPro = (from prov in context.ld_provenance_produit
                             from essai in context.essai
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
        public int IdDestProduitPourCopie(int IdEssai)
        {
            int idProvProd;
            essai es = context.essai.FirstOrDefault(u => u.id == IdEssai);
            if (es.destination_produit != null)
            {
                idProvProd = (from dest in context.ld_destination
                              from essai in context.essai
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
        public int IdProduitInPourCopie(int IdEssai)
        {
            int idProdIn;
            essai es = context.essai.FirstOrDefault(u => u.id == IdEssai);
            if (es.type_produit_entrant != null)
            {
                idProdIn = (from prod in context.ld_produit_in
                            from essai in context.essai
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
        /// Méthode pour créer un projet à partir des informations saisies dans un Formulaire
        /// </summary>
        /// <param name="TitreProjet"></param>
        /// <param name="Confidentialite"></param>
        /// <param name="typeProjetId"></param>
        /// <param name="financId"></param>
        /// <param name="orgId"></param>
        /// <param name="respProjetId"></param>
        /// <param name="numProj"></param>
        /// <param name="provProj"></param>
        /// <param name="description"></param>
        /// <param name="dateCreation"></param>
        /// <param name="IdUser"></param>
        /// <returns>Projet créé et écrit sur la base de données</returns>
        public projet CreationProjet(string TitreProjet, int typeProjetId, int financId, int orgId,
            int respProjetId, string numProj, int provProj, string description, DateTime dateCreation, utilisateur Usr)
        {
            // Déclaration des variables
            string TypeProjetName = null;
            string FinancName = null;
            string MailResponsable = null;
            string ProvenanProj = null;

            // obtenir le nom du type de projet
            if (typeProjetId > 0)
                TypeProjetName = context.ld_type_projet.First(r => r.id == typeProjetId).nom_type_projet;
            // obtenir le nom du type de financement
            if (financId > 0)
                FinancName = context.ld_financement.First(r => r.id == financId).nom_financement;
            // obtenir le mail du responsable projet 
            if (respProjetId > 0)
                MailResponsable = context.Users.First(r => r.Id == respProjetId).Email;
            // obtenir nom provenance projet
            if (provProj > 0)
                ProvenanProj = context.ld_provenance.First(r => r.id == provProj).nom_provenance;

            organisme org = context.organisme.FirstOrDefault(s => s.id == orgId);

            projet newProjet = new projet()
            {
                titre_projet = TitreProjet,
                num_projet = numProj,
                type_projet = TypeProjetName,
                financement = FinancName,
                mailRespProjet = MailResponsable,
                provenance = ProvenanProj,
                description_projet = description,
                date_creation = dateCreation,
                compte_userID = Usr.Id,
                organismeID = org.id
            };

            // ajouter dans ma BDD "projet" le nouveau projet 
            context.projet.Add(newProjet);

            context.SaveChanges();

            return newProjet;
        }

        /// <summary>
        /// Création d'un essai à partir des informations saisies dans le formulaire réservation
        /// </summary>
        /// <param name="pr"></param>
        /// <param name="IdUsr"></param>
        /// <param name="myDateTime"></param>
        /// <param name="manipId"></param>
        /// <param name="ProdId"></param>
        /// <param name="precisionProd"></param>
        /// <param name="QuantProd"></param>
        /// <param name="ProvId"></param>
        /// <param name="destProduit"></param>
        /// <param name="TransStlo"></param>
        /// <param name="commentaire"></param>
        public essai CreationEssai(projet pr, utilisateur Usr, DateTime myDateTime, string confident, int manipId, int ProdId, string precisionProd, int? QuantProd,
            int ProvId, int destProduit, string TransStlo, string commentaire)
        {
            // Déclaration des variables
            string TypeProduit = null;
            string ProvProd = null;
            string DestProd = null;

            // Obtenir le nom des variables du model en ID
            if (ProdId > 0)
                TypeProduit = context.ld_produit_in.First(r => r.id == ProdId).nom_produit_in;
            if (ProvId > 0)
                ProvProd = context.ld_provenance_produit.First(r => r.id == ProvId).nom_provenance_produit;
            if (destProduit > 0)
                DestProd = context.ld_destination.First(r => r.id == destProduit).nom_destination;

            // rajouter la clé étrangère (table "essai") vers l'utilisateur (manipulateurID)
            utilisateur usrManip = context.Users.FirstOrDefault(r => r.Id == manipId);

            // Créer l'essai avec les infos provenant du model (rajouter les ID des clès étrangeres)
            essai Essai = new essai()
            {
                compte_userID = Usr.Id,
                date_creation = myDateTime,
                type_produit_entrant = TypeProduit,
                precision_produit = precisionProd,
                quantite_produit = QuantProd,
                provenance_produit = ProvProd,
                destination_produit = DestProd,
                transport_stlo = Convert.ToBoolean(TransStlo),
                status_essai = EnumStatusEssai.WaitingValidation.ToString(),
                titreEssai = commentaire,
                confidentialite = confident,
                manipulateurID = usrManip.Id,
                projetID = pr.id
            };

            //Ajouter dans la BDD "essai"  le nouveau essai
            context.essai.Add(Essai);
            // Enregistrer les changements pour pouvoir accèder à l'id essai
            context.SaveChanges();
            // Dès que l'on crée un essai alors il faut créer l'enquete pour faciliter l'algorithme (Task pour l'envoie d'enquetes)
            enquete enq = new enquete { essaiId = Essai.id };
            context.enquete.Add(enq);
            // Enregistrer les changements
            context.SaveChanges();

            return Essai;
        }

        public void UpdateEssai(essai Essai, DateTime dateInf, DateTime dateSup)
        {
            // Mettre à jour l'essai pour rajouter ces dates
            Essai.date_inf_confidentiel = dateInf;
            Essai.date_sup_confidentiel = dateSup;

            context.SaveChanges();
        }

        public projet ObtenirProjXEssai(int projetID)
        {
            return context.projet.First(p => p.id == projetID);
        }

        public void UpdateStatusEssai(essai essai)
        {
            essai.status_essai = EnumStatusEssai.WaitingValidation.ToString();
            context.SaveChanges();
        }

        public List<reservation_projet> ObtenirResasEssai(int IdEssai)
        {
            return context.reservation_projet.Where(e => e.essaiID == IdEssai).ToList();
        }
    }
}
