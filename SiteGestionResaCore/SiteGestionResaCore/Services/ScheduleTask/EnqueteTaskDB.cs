using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Services.ScheduleTask
{
    public class EnqueteTaskDB: IEnqueteTaskDB
    {
        private readonly UserManager<utilisateur> userManager;
        private readonly GestionResaContext resaDb;
        private readonly ILogger<EnqueteTaskDB> logger;

        public EnqueteTaskDB(
            UserManager<utilisateur> userManager,
            GestionResaContext resaDb,
            ILogger<EnqueteTaskDB> logger)
        {
            this.userManager = userManager;
            this.resaDb = resaDb;
            this.logger = logger;
        }

        public async Task<IList<utilisateur>> GetUtilisateurSuperAdminAsync()
        {
            return await userManager.GetUsersInRoleAsync("MainAdmin");
        }

        public List<enquete> GetEnquetesXFirstTime()
        {
            List<enquete> enquetesNonEnvoyées = new List<enquete>();
            List<enquete> enquetesToReturn = new List<enquete>();
            List<reservation_projet> reservations = new List<reservation_projet>();

            enquetesNonEnvoyées = resaDb.enquete.Where(e => e.date_envoi_enquete == null).ToList(); // enquetes dont l'essai n'était pas fini lors de la dernière exécution de la tâche
            
            try
            {
                foreach (var enq in enquetesNonEnvoyées)
                {
                    // trouver l'essai pour chaque enquete et voir si les réservations pour cet essai sont déjà finis 
                    var esssai = resaDb.essai.First(e => e.id == enq.essaiId);

                    if (esssai.status_essai == "Validate" || esssai.status_essai == "WaitingValidation")
                    {
                        // retrouver toutes les réservations pour cet essai (retrouver la date fin la plus proche d'aujourd'hui)
                        // ajouter un try catch car si l'essai n'a pas de réservation alors cela peut produire une erreur
                        try
                        {
                            reservations = resaDb.reservation_projet.Where(r => r.essaiID == esssai.id).OrderByDescending(r => r.date_fin).ToList();
                        }
                        catch (Exception e)
                        {
                            logger.LogError("Probleme de recuperation des reservations pour l'essai Id: " + enq.essaiId +
                                "./n exception: " + e.Message);
                            goto ENDT;
                        }

                        // récupérer la premiere date qu'est la plus récente par rapport à aujourd'hui
                        if (reservations.Count != 0)
                        {
                            if (reservations[0].date_fin <= DateTime.Today) // Si la réservation la plus loin est déjà passée alors envoyer l'enquete
                            {
                                // TODO: tester!! Mettre à jour la date du premier envoi pour cet enquete = date fin de l'essai
                                enq.date_premier_envoi = reservations[0].date_fin;
                                resaDb.SaveChanges();
                                enquetesToReturn.Add(enq);
                            }
                        }
                        else
                        {
                            logger.LogWarning("Il n'y a pas des réservations pour l'essai N° : " + enq.essaiId);
                        }
                    }                       
                    
                }
            }
            catch(Exception e)
            {
                logger.LogError(e, "Problème pour obtenir les enquetes à envoyer pour la 1ere fois");
            }          

            ENDT:
            return enquetesToReturn;
        }

        /// <summary>
        /// Méthode pour créer les enquetes sur les essais qui n'ont pas des enquetes (nouveau changement!)
        /// On pourrait supprimer la méthode après (TODO)
        /// </summary>
        /// <returns></returns>
        public bool AreEnquetesCreated()
        {
            foreach(var ess in resaDb.essai.ToList())
            {

                var enq = resaDb.enquete.FirstOrDefault(e => e.essaiId == ess.id); // Utiliser FirstOrDefault dans le cas où on souhaite obtenir des valeurs nulls
                if (enq == null)
                {
                    // créer l'enquete pour faciliter l'algorithme (Task pour l'envoie d'enquetes)
                    enquete enquete = new enquete { essaiId = ess.id };
                    // ajouter l'enquete
                    resaDb.enquete.Add(enquete);
                    // sauvegarder les changements sur la BDD
                    resaDb.SaveChanges();
                }
              
            }
            return true;
        }

        public essai GetEssaiParEnquete(int idEssai)
        {
            return resaDb.essai.First(e => e.id == idEssai);
        }

        public projet GetProjetParEnquete(int projetID)
        {
            return resaDb.projet.First(p => p.id == projetID);
        }

        public List<enquete> GetEnquetesPourRelance()
        {
            List<enquete> ListPourRelance = new List<enquete>();

            List<enquete> ListeNonRepondues = resaDb.enquete.Where(e => e.reponduEnquete == null).ToList();
            try
            {
                foreach (var enq in ListeNonRepondues)
                {
                    // trouver l'essai pour chaque enquete et voir si les réservations pour cet essai sont déjà finis 
                    var essai = resaDb.essai.First(e => e.id == enq.essaiId);

                    if(essai.status_essai == "Validate" || essai.status_essai == "WaitingValidation")
                    {
                        if (enq.date_envoi_enquete.HasValue)
                        {
                            // Si l'enquête a été envoyé il y a plus de 7 jours alors il faut relancer
                            TimeSpan diff = DateTime.Now - enq.date_envoi_enquete.Value;
                            if (diff.Days >= 7)
                            {
                                ListPourRelance.Add(enq);
                            }
                        }
                    }                        
                    
                }
            }
            catch(Exception e)
            {
                logger.LogError(e, "Problème pour obtenir les enquetes pour relance");
            }
          
            return ListPourRelance;
        }

        public void UpdateDateEnvoiEnquete(enquete enquete)
        {
            enquete.date_envoi_enquete = DateTime.Now;
            resaDb.SaveChanges();
        }

        public string GetEmailCreatorEssai(int userID)
        {
           return resaDb.Users.Find(userID).Email;
        }

        public void UpdateDateEnvoiEnqueteManuel(enquete enquete)
        {
            enquete.date_envoi_enquete = DateTime.Now;
            if(enquete.date_premier_envoi == null)
            {
                enquete.date_premier_envoi = DateTime.Now;
            }          
            resaDb.SaveChanges();
        }
    }
}
