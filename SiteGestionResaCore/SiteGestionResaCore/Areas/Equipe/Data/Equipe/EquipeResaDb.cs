using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Areas.Equipe.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Equipe.Data
{
    public class EquipeResaDb: IEquipeResaDb
    {

        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext context;
        private readonly UserManager<utilisateur> userManager;
        private readonly ILogger<EquipeResaDb> logger;

        public EquipeResaDb(
            GestionResaContext resaDB,
            UserManager<utilisateur> userManager,
            ILogger<EquipeResaDb> logger)
        {
            this.context = resaDB;
            this.userManager = userManager;
            this.logger = logger;
        }

        /// <summary>
        /// Méthode pour obtenir 2 listes, une pour les "utilisateurs" avec compte validé et une autre pour ceux qui sont en
        /// attente de validation de compte
        /// </summary>
        /// <returns>liste des utilisateurs avec compte valide, et liste de ceux qui sont en attente de valid</returns>
        public async Task<listAutresUtilisateurs> ObtenirListAutresAsync()
        {
            List<utilisateur> tempUsers = new List<utilisateur>();
            List<utilisateur> usrWaiting = new List<utilisateur>();

            var users = await userManager.GetUsersInRoleAsync("Utilisateur");
            var usrs = users.Where(u => u.compteInactif != true); // TODO: Vérifier!!!
            foreach (var user in usrs)
            {
                if (user.EmailConfirmed == true)
                    tempUsers.Add(user);
                else
                    usrWaiting.Add(user);
            }

            return new listAutresUtilisateurs(tempUsers.OrderBy(u=>u.nom).ToList(), usrWaiting.OrderBy(u=>u.nom).ToList());
        }

        /// <summary>
        /// Méthode pour obtenir la liste des personnes avec rôle "Admin" et "MainAdmin"
        /// </summary>
        /// <returns>lis des utilisateurs Admin</returns>
        public List<utilisateur> ObtenirListAdmins()
        {
            return (from role in context.Roles
                    from roleusr in context.Users
                    from usrrol in context.UserRoles
                    where (role.Name == "Admin" || role.Name == "MainAdmin") && role.Id == usrrol.RoleId && roleusr.Id == usrrol.UserId
                    select roleusr).Distinct().OrderBy(u=>u.nom).ToList();
        }

        /// <summary>
        /// Obtenir la liste des admin "logistic" 
        /// </summary>
        /// <returns></returns>
        public async Task<IList<utilisateur>> ObtenirUsersLogisticAsync()
        {
            return await userManager.GetUsersInRoleAsync("Logistic");

        }
        
        /// <summary>
        /// obtenir un utilisateur à partir de son ID
        /// </summary>
        /// <param name="id">id key base de données ADO.net</param>
        /// <returns>utilisateur</returns>
        public utilisateur ObtenirUtilisateur(int id)
        {
            return (context.Users.FirstOrDefault(u => u.Id == id));
        }

        /// <summary>
        /// Methode permettant de changer le rôle d'un "Admin" à "Utilisateur"
        /// Sauf pour le rôle "MainAdmin" AUCUN changement est permis
        /// </summary>
        /// <param name="id"> id utilisateur key</param>
        public async Task ChangeAccesToUserAsync(int id)
        {
            //User manager pour accèder aux opérations sur la table AspNet
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                logger.LogError($"Utilisateur {id} non trouvé");
                return;
            }
            // Submit the changes to the database.
            try
            {
                // Obtenir les rôles attribués à l'utilisateur
                var allUserRoles = await userManager.GetRolesAsync(user);
                // Vérifier qu'il ne s'agit pas d'un "MainAdmin"
                if (allUserRoles.Contains("MainAdmin"))
                {
                    logger.LogWarning("Impossible de changer le rôle d'un utilisateur 'MainAdmin' ");
                }
                else
                {
                    await userManager.RemoveFromRolesAsync(user, allUserRoles);
                    await userManager.AddToRoleAsync(user, "Utilisateur");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Erreur générale au changement d'user vers admin");
            }
        }

        /// <summary>
        /// Méthode permettant de changer des droits "Utilisateur" à "Admin"
        /// </summary>
        public async Task ChangeAccesToAdminAsync(int id)
        {
            var user = await context.Users.FindAsync(id);
            // Submit the changes to the database.
            try
            {
                // Obtenir les rôles utilisateur
                var allUserRoles = await userManager.GetRolesAsync(user);
                // Vérifier qu'il ne s'agit pas d'un "MainAdmin"
                if (allUserRoles.Contains("MainAdmin"))
                {
                    logger.LogWarning(" Impossible de changer le rôle d'un utilisateur 'MainAdmin' ");
                }
                else
                {
                    await userManager.RemoveFromRolesAsync(user, allUserRoles);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Problème requete pour ajouter l'utilisateur dans le groupe Admin");
            }
            
        }

        /// <summary>
        /// Méthode permettant à un "Administrateur" du site de avoir un rôle "Logistic" en paralélle
        /// </summary>
        /// <param name="id"></param>
        public async Task AddAdminToLogisticRoleAsync(int id)
        {
            var user = await context.Users.FindAsync(id);
            // Submit the changes to the database.
            try
            {
                // Obtenir les rôles utilisateur
                var allUserRoles = await userManager.GetRolesAsync(user);
                if (!allUserRoles.Contains("Logistic"))
                {
                    // Pas la peine de vérifier s'il est dans le groupe Admin (vérification déjà faite sur l'équipeViewModel)
                    await userManager.AddToRoleAsync(user, "Logistic");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Problème pour ajouter un admin dans le groupe logistique");
                
                // Provide for exceptions.
            }
        }

        /// <summary>
        /// Méthode pour retirer à un utilisateur le rôle "Logistic"
        /// </summary>
        /// <param name="id"></param>
        public async Task RemoveLogisticRoleAsync(int id)
        {
            try
            {
                await userManager.RemoveFromRoleAsync(await context.Users.FindAsync(id), "Logistic");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Problème requete pour retirer les droits logistic");
            }
        }

        /// <summary>
        /// Méthode pour changer la colonne "compte_valide" d'un utilisateur (BDD site)
        /// </summary>
        /// <param name="id"> id table "utilisateur"</param>
        public void ValidateAccount(int id)
        {
            try
            {
                // Mettre à jour d'abord la table des "utilisateurs"
                context.Users.First(u => u.Id == id).EmailConfirmed = true;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Problème requete pour validation de compte");
            }
        }

        public int NbEssaiXUser(utilisateur user)
        {
            return context.essai.Where(e=> e.compte_userID == user.Id).Count();
        }

        /// <summary>
        /// Obtenir la liste des admin "logistic maintenance"
        /// </summary>
        /// <returns></returns>
        public async Task<IList<utilisateur>> ObtenirUsersIntervAsync()
        {
            return await userManager.GetUsersInRoleAsync("LogisticMaint");
        }

        public async Task AddingAdminToInterv(int id)
        {
            var user = await context.Users.FindAsync(id);
            // Submit the changes to the database.
            try
            {
                // Obtenir les rôles utilisateur
                var allUserRoles = await userManager.GetRolesAsync(user);
                if (!allUserRoles.Contains("LogisticMaint"))
                {
                    // Pas la peine de vérifier s'il est dans le groupe Admin (vérification déjà faite sur l'équipeViewModel)
                    await userManager.AddToRoleAsync(user, "LogisticMaint");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Problème pour ajouter un admin dans le groupe maintenance");
            }
        }

        /// <summary>
        /// Méthode pour retirer à un utilisateur le rôle "LogisticMaint"
        /// </summary>
        /// <param name="id"></param>
        public async Task RemoveLogisticMaintRoleAsync(int id)
        {
            try
            {
                await userManager.RemoveFromRoleAsync(await context.Users.FindAsync(id), "LogisticMaint");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Problème requete pour retirer les droits LogisticMaint");
            }
        }
        public async Task<IList<utilisateur>> ObtenirUsersDonneesAsync()
        {
            return await userManager.GetUsersInRoleAsync("DonneesAdmin");
        }

        public async Task AddingAdminToAdmDonnees(int id)
        {
            var user = await context.Users.FindAsync(id);
            // Submit the changes to the database.
            try
            {
                // Obtenir les rôles utilisateur
                var allUserRoles = await userManager.GetRolesAsync(user);
                if (!allUserRoles.Contains("DonneesAdmin"))
                {
                    // Pas la peine de vérifier s'il est dans le groupe Admin (vérification déjà faite sur l'équipeViewModel)
                    await userManager.AddToRoleAsync(user, "DonneesAdmin");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Problème pour ajouter un admin dans le groupe Admin entrepôts");

                // Provide for exceptions.
            }
        }

        public async Task RemoveAdmDonneesUsr(int id)
        {
            try
            {
                await userManager.RemoveFromRoleAsync(await context.Users.FindAsync(id), "DonneesAdmin");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Problème requete pour retirer les droits LogisticMaint");
            }
        }

        public void ChangerEquipeUser()
        {
            var listUsr = context.Users.ToList().Distinct();

            foreach (var usr in listUsr)
            {
                if (usr.equipeID == 3 || usr.equipeID == 4 || usr.equipeID == 5) // les utilisateurs dont l'équipe était PSM, ISF ou SMCF
                {
                    usr.equipeID = 8; //changement d'équipe pour les rajouter dans l'équipe PSF
                    context.SaveChanges();
                }
            }
        }
    }
}
