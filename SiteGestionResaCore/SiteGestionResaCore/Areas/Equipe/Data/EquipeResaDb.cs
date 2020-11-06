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

namespace SiteGestionResaCore.Areas.Reservation.Data
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

            foreach (var user in users)
            {
                //usr = context.utilisateur.First(r => r.Email == y.Email);
                if (user.EmailConfirmed == true)
                    tempUsers.Add(user);
                else
                    usrWaiting.Add(user);
            }

            return new listAutresUtilisateurs(tempUsers, usrWaiting);
        }

        /// <summary>
        /// Méthode pour obtenir la liste des personnes avec rôle "Admin" et "MainAdmin"
        /// </summary>
        /// <returns>lis des utilisateurs Admin</returns>
        public List<utilisateur> ObtenirListAdmins()
        {
            // TODO: vérifier
            return (from role in context.Roles
                    from roleusr in context.Users
                    from usrrol in context.UserRoles
                    where (role.Name == "Admin" || role.Name == "MainAdmin") && role.Id == usrrol.RoleId
                    select roleusr).Distinct().ToList();
        }

        /// <summary>
        /// Obtenir la liste des admin "logistic" 
        /// </summary>
        /// <returns></returns>
        public async Task<IList<utilisateur>> ObtenirUsersLogisticAsync()
        {
            return await userManager.GetUsersInRoleAsync("Logistic");

        }

        public IEnumerable<SelectListItem> ListUsersToSelectItem(List<utilisateur> utilisateurs)
        {
            var DefaultUsrItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = "- Selectionner un utilisateur -" }, count: 1);

            var allUsrs = utilisateurs.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
            }); ;


            return DefaultUsrItem.Concat(allUsrs);
        }

        /// <summary>
        /// obtenir un utilisateur à partir de son ID
        /// </summary>
        /// <param name="id">id key base de données ADO.net</param>
        /// <returns>utilisateur</returns>
        public utilisateur ObtenirUtilisateur(int id)
        {
            return (context.utilisateur.FirstOrDefault(u => u.Id == id));
        }

        /// <summary>
        /// Methode permettant de changer le rôle d'un "Admin" à "Utilisateur"
        /// Sauf pour le rôle "MainAdmin" AUCUN changement est permis
        /// </summary>
        /// <param name="id"> id utilisateur key</param>
        public async Task ChangeAccesToUser(int id)
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

        //TODO: Tester cette méthode
        /// <summary>
        /// Méthode permettant de changer des droits "Utilisateur" à "Admin"
        /// </summary>
        public async Task ChangeAccesToAdminAsync(int id)
        {
            var user = await context.Users.FindAsync(id);

            /*var query = (from roleUsr in context.Users
                         from usrnet in context.Roles
                         where email == roleUsr.Email && roleUsr.Id == usrnet.Id
                         select usrnet).First();*/

            // Submit the changes to the database.
            try
            {
                // Obtenir les rôles dont l'utilisateur
                IList<string> allUserRoles = await userManager.GetRolesAsync(user);
                // Vérifier qu'il ne s'agit pas d'un "MainAdmin"
                if (allUserRoles.Contains("MainAdmin"))
                {
                    Console.WriteLine("Impossible de changer le rôle d'un utilisateur 'MainAdmin' ");
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
                userManager.Dispose();
                // Provide for exceptions.
            }
            userManager.Dispose();
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
                userManager.Dispose();
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
                await userManager.RemoveFromRoleAsync(await context.utilisateur.FindAsync(id), "Logistic");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Problème requete pour retirer les droits logistic");
                userManager.Dispose();
                // Provide for exceptions.
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
                context.utilisateur.First(u => u.Id == id).EmailConfirmed = true;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Problème requete pour validation de compte");
            }
        }


        /// <summary>
        /// Méthode pour refuser la demande d'ouverture de compte (effacer la personne de la BDD table "utilisateur")
        /// </summary>
        /// <param name="id">id "utilisateur"</param>
        public Task DeleteRequestAccount(int id)
        {
            try
            {
                // effacer les infos de la BDD PflStloResa
                context.utilisateur.Remove(context.utilisateur.First(u => u.Id == id));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Problème pour effacer la demande d'ouverture compte");
            }

            return context.SaveChangesAsync();
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context?.Dispose();
                    userManager?.Dispose();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~EquipeResaDb()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
