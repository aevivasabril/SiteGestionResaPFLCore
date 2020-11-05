using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using SiteGestionResaCore.Areas.Equipe.Data;
using SiteGestionResaCore.Areas.Reservation.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Models
{
    public class ResaDB : IResaDB
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext context;
        private readonly UserManager<utilisateur> userManager;
        private readonly ILogger<ResaDB> logger;

        /// <summary>
        /// Constructeur initializant la connexion vers les 2 base de données
        /// </summary>
        public ResaDB(
            GestionResaContext resaDB,
            UserManager<utilisateur> userManager,
            ILogger<ResaDB> logger)
        {
            this.context = resaDB;
            this.userManager = userManager;
            this.logger = logger;
        }

        /// <summary>
        /// Dispose des connexions bases des données
        /// </summary>
        public void Dispose()
        {
            context?.Dispose();
        }

        #region Account controller zone (méthodes d'accès aux données pour la création des comptes utilisateur)
        /// <summary>
        /// Liste des organismes déclarés sur la BDD
        /// </summary>
        /// <returns>liste des organismes </returns>
        public List<organisme> ObtenirListOrg()
        {
            return context.organisme.ToList();
        }

        /// <summary>
        /// Créer un nouveau utilisateur dans la BDD suite au remplissage du formulaire inscription
        /// </summary>
        /// <param name="Nom">nom utilisateur </param>
        /// <param name="Prenom"> prenom utilisateur</param>
        /// <param name="organismeId"> id de l'item organisme d'appartenance </param>
        /// <param name="Email"> email utilisateur</param>
        public void CreerUtilisateur(string Nom, string Prenom, int organismeId, string Email)
        {
            context.utilisateur.Add(new utilisateur { nom = Nom, prenom = Prenom, Email = Email, organismeID = organismeId });
            context.SaveChanges();
        }
        #endregion

        #region Méthodes d'accès aux données pour la zone "Equipe" gestion des utilisateurs
        /// <summary>
        /// Méthode pour obtenir la liste des personnes avec rôle "Admin" et "MainAdmin"
        /// </summary>
        /// <returns>lis des utilisateurs Admin</returns>
        public List<utilisateur> ObtenirListAdmins()
        {
            return (from role in context.Roles
                          from roleusr in context.Users
                          where role.Name == "Admin" || role.Name == "MainAdmin"
                          select roleusr).Distinct().ToList();         
        }

        /// <summary>
        /// méthode pour obtenir la liste des administrateurs chargés de la logistique (validation de réservations) table "utilisateur"
        /// </summary>
        /// <returns> liste des utilisateurs</returns>
        //public async Task<IList<utilisateur>> ObtenirListAdminsLogistiqueAsync()
        //{

            //return await userManager.GetUsersInRoleAsync("Logistic");
            //obtention de tous les AspNetUsers dont le rôle est "Admin" et "logistic"
            //var adminLogicId = (from role in context.Roles
            //                    from roleUsr in context.Users
            //                    from usrrol in roleUsr.Roles
            //                    from x in roleUsr.Roles
            //                    where (role.Name == "Logistic" && x.RoleId == role.Id)
            //                    select roleUsr).Distinct().ToArray();
            //// adminId est convertie en Array! n'est plus une requete Linq c'est pour ça l'erreur persistant, je sais pas comme résoudre ce problème
            ///*var finale = (from resa in resaDB.utilisateur
            //              from adm in adminId
            //              where adm.Email == resa.Email
            //              select resa);*/
            //foreach (var y in adminLogicId)
            //{
            //    usr = context.utilisateur.First(r => r.Email == y.Email);
            //    tempAdminLogis.Add(usr);
            //}
            //return tempAdminLogis;
        //}

        public async Task<IList<utilisateur>> ObtenirUsersLogisticAsync()
        {
            return await userManager.GetUsersInRoleAsync("Logistic");

        }

        /// <summary>
        /// Méthode pour obtenir 2 listes, une pour les "utilisateurs" avec compte validé et une autre pour ceux qui sont en
        /// attente de validation de compte
        /// </summary>
        /// <returns>liste des utilisateurs avec compte valide, et liste de ceux qui sont en attente de valid</returns>
        public listAutresUtilisateurs ObtenirListAutres()
        {
            List<utilisateur> tempUsers = new List<utilisateur>();
            List<utilisateur> usrWaiting = new List<utilisateur>();
            utilisateur usr;

            //obtention de tous les AspNetUsers dont le rôle est "Admin"
            var userId = (from role in context.Roles
                          from roleUsr in context.Users
                          from usrrol in context.Roles
                          from x in context.Roles
                          where role.Name == "Utilisateur" && x.Id == role.Id && x.Id == roleUsr.Id
                          select roleUsr).ToArray();

            foreach (var y in userId)
            {
                usr = context.utilisateur.First(r => r.Email == y.Email);
                if (usr.EmailConfirmed == true)
                {
                    tempUsers.Add(usr);
                }
                else
                {
                    usrWaiting.Add(usr);
                }
            }

            return new listAutresUtilisateurs(tempUsers, usrWaiting);
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

        #region Méthodes pour la création des dropdownlist vue FormulaireProjet

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

        public IEnumerable<SelectListItem> ListEssaisToSelectItem(List<EssaiUtilisateur> essais)
        {
            // Premier item par défaut de la dropdownlist copie d'essai
            var DefaultEssaiItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = "- Sélectionnez un Essai -" }, count: 1);
            // Création d'une liste des item avec des détails d'un essai
            var allOrgs = essais.Select(f => new SelectListItem
            {
                Value = f.CopieEssai.id.ToString(),
                Text = "Essai crée le " + f.CopieEssai.date_creation.ToString() + " - Manipulateur Essai: " + f.user.nom + ", " + f.user.prenom + " - Commentaire essai: " +
                           f.CopieEssai.commentaire + " - Type produit entrant: " + f.CopieEssai.type_produit_entrant + " -" + f.CopieEssai.quantite_produit
            });
            return DefaultEssaiItem.Concat(allOrgs);
        }

        public IEnumerable<SelectListItem> ListTypeProjetItem(List<ld_type_projet> typeProj)
        {
            // Message par défaut entête de la liste type de projet
            var DefaultTypeProjItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = "- Type projet -" }, count: 1);

            var allOrgs = typeProj.Select(f => new SelectListItem
            {
                Value = f.id.ToString(),
                Text = f.nom_type_projet
            });
            return DefaultTypeProjItem.Concat(allOrgs);

        }

        public IEnumerable<SelectListItem> ListFinancementItem(List<ld_financement> financement)
        {
            var DefaultFinancementItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = "- Type financement -" }, count: 1);

            var allOrgs = financement.Select(f => new SelectListItem
            {
                Value = f.id.ToString(),
                Text = f.nom_financement
            });
            return DefaultFinancementItem.Concat(allOrgs);
        }

        public IEnumerable<SelectListItem> ListOrgItem(List<organisme> organismes)
        {
            // Entête de la liste pour selectionner un organisme
            var DefaultOrgItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = "- Options -" }, count: 1);

            // Création d'une liste Dropdownlist contenant les types d'organismes
            var allOrgs = organismes.Select(f => new SelectListItem
            {
                Value = f.id.ToString(),
                Text = f.nom_organisme
            });
            return DefaultOrgItem.Concat(allOrgs);
        }

        public IEnumerable<SelectListItem> ListRespItem(List<utilisateur> respProj)
        {
            var DefaultRespItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = "- Sélectionnez un responsable -" }, count: 1);

            var allOrgs = respProj.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )"
            }); ;
            return DefaultRespItem.Concat(allOrgs);
        }

        public IEnumerable<SelectListItem> ListProveItem(List<ld_provenance> provProj)
        {
            // Entete liste provenance projet 
            var DefaultProvItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = "- Options provenance projet -" }, count: 1);
            // Création d'une liste de provenance projet (dropdownlist)
            var allOrgs = provProj.Select(f => new SelectListItem
            {
                Value = f.id.ToString(),
                Text = f.nom_provenance
            });
            return DefaultProvItem.Concat(allOrgs);
        }

        public IEnumerable<SelectListItem> ListManipItem(List<utilisateur> UsersWithAccess)
        {
            // Entete de la liste des utilisateurs "manipulateurs" pour un essai
            var DefaultManipItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = "- Sélectionnez un Manipulateur -" }, count: 1);
            // Création d'une liste utilisateurs "manipulateur" de l'essai
            var allOrgs = UsersWithAccess.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.nom + ", " + f.prenom + " ( " + f.Email + " )"
            }); ;
            return DefaultManipItem.Concat(allOrgs);
        }

        public IEnumerable<SelectListItem> ListProdEntreeItem(List<ld_produit_in> produitEnt)
        {
            // Entete de la liste des produits en entrée pour un essai
            var DefaultProductItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = "- Options -" }, count: 1);

            // Création d'une liste dropdownlist pour le type produit entrée
            var allOrgs = produitEnt.Select(f => new SelectListItem
            {
                Value = f.id.ToString(),
                Text = f.nom_produit_in
            });
            return DefaultProductItem.Concat(allOrgs);
        }

        // Création d'une liste dropdownlit pour selectionner la provenance produit entrée
        public IEnumerable<SelectListItem> ListProvProdItem(List<ld_provenance_produit> provProduit)
        {
            // Entete dropdownlist provenance produit
            var DefaultProvenanceProItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = "- Options -" }, count: 1);

            // Création d'une liste dropdownlit pour selectionner la provenance produit entrée
            var allOrgs = provProduit.Select(f => new SelectListItem
            {
                Value = f.id.ToString(),
                Text = f.nom_provenance_produit
            });
            return DefaultProvenanceProItem.Concat(allOrgs);
        }

        public IEnumerable<SelectListItem> ListDestProdItem(List<ld_destination> destProduit)
        {
            // Entete dropdownlist destinaison produit
            var DefaultDestProItem =  Enumerable.Repeat(new SelectListItem { Value = "-1", Text = "- Options -" }, count: 1);
            // Création d'une liste dropdownlit pour selectionner la destinaison produit sortie
            var allOrgs = destProduit.Select(f => new SelectListItem
            {
                Value = f.id.ToString(),
                Text = f.nom_destination
            });
            return DefaultDestProItem.Concat(allOrgs);
        }

        #endregion

        /// <summary>
        /// Methode permettant de changer le rôle d'un "Admin" à "Utilisateur"
        /// Sauf pour le rôle "MainAdmin" AUCUN changement est permis
        /// </summary>
        /// <param name="id"> id utilisateur key</param>
        public async Task ChangeAccesToUser(int id)
        {
            //User manager pour accèder aux opérations sur la table AspNet
            var user = await context.Users.FindAsync(id); 
            if(user == null)
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
        /// Méthode permettant à un "Administrateur" du site de avoir un rôle "Logistic" en paralélle
        /// </summary>
        /// <param name="id"></param>
        public async Task AddAdminToLogisticRoleAsync(int id)
        {
            var user = await context.Users.FindAsync(id);
            // Submit the changes to the database.
            try
            {
                // Obtenir les rôles dont l'utilisateur
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
        /// Obtenir le mail d'un utilisateur à partir l'id table "utilisateur"
        /// </summary>
        /// <param name="id"></param>
        /// <returns>mail utilisateur</returns>
        public string mailUtilisateur(int id)
        {

            string mailUser = (from utili in context.utilisateur
                               where utili.Id == id
                               select utili.Email).First();
            return mailUser;
        }

        /// <summary>
        /// Récupérer l'ID de la table AspNetUsers à partir de l'id de notre table utilisateur 
        /// </summary>
        /// <param name="id">id table "utilisateur"</param>
        /// <returns>Id utilisateur sur la BDD aspnet users </returns>
        public int IdAspNetUser(int id)
        {
            //string UserID = null;
            string mail = mailUtilisateur(id);

            int UserID = (from roleUsr in context.Users
                      from usrnet in context.Roles
                      where mail == roleUsr.Email && roleUsr.Id == usrnet.Id
                      select roleUsr.Id).First();

            return UserID;
        }

        /// <summary>
        /// récupérer l'id AspNet users à partir d'une adresse mail enregistré sur la base de données
        /// </summary>
        /// <param name="mailUsr"></param>
        /// <returns>Id</returns>
        public int IdAspNetUserFromMail(string mailUsr)
        {
            return context.Users.FirstOrDefault(u => u.Email == mailUsr).Id;
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
            catch(Exception e)
            {
                logger.LogError(e, "Problème pour effacer la demande d'ouverture compte");
            }

            return context.SaveChangesAsync();
        }
        #endregion

        #region Méthodes d'accès aux données pour la partie ou zone "Réservation"

        /// <summary>
        /// Obtenir la liste des options type de projet 
        /// </summary>
        /// <returns>List<ld_type_projet></returns>
        public List<ld_type_projet> ObtenirList_TypeProjet()
        {
            return context.ld_type_projet.ToList();
        }

        /// <summary>
        /// Obtenir les options liste financement
        /// </summary>
        /// <returns>liste des options financement</returns>
        public List<ld_financement> ObtenirList_Financement()
        {
            return context.ld_financement.ToList();
        }

        /// <summary>
        /// Retrouver les utilisateurs dont le compte a déjà été validé et surtout le mail confirmé via le lien envoyé par mail
        /// </summary>
        /// <returns> liste des utilisateurs</returns>
        public List<utilisateur> ObtenirList_UtilisateurValide()
        {
            return context.Users.Where(e => e.EmailConfirmed == true).ToList();
            /*List<utilisateur> userWithAccess = new List<utilisateur>();
            utilisateur usr;

            var query = (from roleUsr in context.Users
                         where (roleUsr.EmailConfirmed == true)
                         select roleUsr).ToArray();

            foreach (var y in query)
            {
                usr = context.utilisateur.First(r => r.Email == y.Email);
                if (usr.compte_valide == true)
                {
                    userWithAccess.Add(usr);
                }
            }
            return userWithAccess;*/
        }

        /// <summary>
        /// Obtenir la liste des options provenance projet
        /// </summary>
        /// <returns> Liste options provenance</returns>
        public List<ld_provenance> ObtenirList_ProvenanceProjet()
        {
            return context.ld_provenance.ToList();
        }

        /// <summary>
        /// Obtenir liste des options type de produit d'entrée
        /// </summary>
        /// <returns> liste des options produit entrée</returns>
        public List<ld_produit_in> ObtenirList_TypeProduitEntree()
        {
            return context.ld_produit_in.ToList();
        }

        /// <summary>
        /// Obtenir liste des options provenance produit
        /// </summary>
        /// <returns>List<ld_provenance_produit></returns>
        public List<ld_provenance_produit> ObtenirList_ProvenanceProduit()
        {
            return context.ld_provenance_produit.ToList();
        }

        /// <summary>
        /// Obtenir liste des options destinaition produit sortie
        /// </summary>
        /// <returns></returns>
        public List<ld_destination> ObtenirList_DestinationPro()
        {
            return context.ld_destination.ToList();
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
            if(typeProjetId > 0)
                TypeProjetName = context.ld_type_projet.First(r => r.id == typeProjetId).nom_type_projet;
            // obtenir le nom du type de financement
            if (financId > 0)
                FinancName = context.ld_financement.First(r => r.id == financId).nom_financement;
            // obtenir le mail du responsable projet 
            if (respProjetId > 0)
                MailResponsable = context.utilisateur.First(r => r.Id == respProjetId).Email;
            // obtenir nom provenance projet
            if (provProj > 0)
                ProvenanProj = context.ld_provenance.First(r => r.id == provProj).nom_provenance;

            organisme org = context.organisme.FirstOrDefault(s => s.id == orgId);

            projet newProjet = new projet (){ titre_projet = TitreProjet, num_projet = numProj,
                type_projet = TypeProjetName, financement = FinancName, mailRespProjet = MailResponsable,
                provenance = ProvenanProj, description_projet = description, date_creation = dateCreation, compte_userID = Usr.Id.ToString(), organismeID = org.id };

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
        public essai CreationEssai(projet pr, utilisateur Usr, DateTime myDateTime, string confident, int manipId, int ProdId, string precisionProd, string QuantProd,
            int ProvId, int destProduit, string TransStlo, string commentaire)
        {
            // Déclaration des variables
            string TypeProduit = null;
            string ProvProd = null;
            string DestProd = null;

            // Obtenir le nom des variables du model en ID
            if (ProdId >0)
                TypeProduit = context.ld_produit_in.First(r => r.id == ProdId).nom_produit_in;
            if (ProvId >0)
                ProvProd = context.ld_provenance_produit.First(r => r.id == ProvId).nom_provenance_produit;
            if (destProduit > 0)
                DestProd = context.ld_destination.First(r => r.id == destProduit).nom_destination;

            // rajouter la clé étrangère (table "essai") vers l'utilisateur (manipulateurID)
            utilisateur usrManip = context.utilisateur.FirstOrDefault(r => r.Id == manipId);

            // Créer l'essai avec les infos provenant du model (rajouter les ID des clès étrangeres)
            essai Essai = new essai () { compte_userID = Usr.Id.ToString(), date_creation = myDateTime, type_produit_entrant = TypeProduit, precision_produit = precisionProd,
                quantite_produit = QuantProd, provenance_produit = ProvProd, destination_produit = DestProd, transport_stlo = Convert.ToBoolean(TransStlo),
                status_essai = EnumStatusEssai.WaitingValidation.ToString(), commentaire = commentaire, confidentialite = confident, manipulateurID = usrManip.Id, projetID = pr.id };

            //Ajouter dans la BDD "essai"  le nouveau essai
            context.essai.Add(Essai);

            // Enregistrer les changements
            context.SaveChanges();

            return Essai;
        }

        // TODO: à vérifier
        public reservation_projet CreationReservation(equipement Equip, essai Essai, DateTime dateDebut, DateTime dateFin)
        {
            //string sqlFormattedDateDebut = dateDebut.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //string sqlFormattedDateFin = dateFin.ToString("yyyy-MM-dd HH:mm:ss.fff");
            // Rajouter uniquement les ID's vers les autres tables (clé étrangere)
            reservation_projet resa = new reservation_projet () { equipementID = Equip.id, essaiID = Essai.id, date_debut = dateDebut, date_fin = dateFin };

            // Rajouter la clé étrangere (table equipement) vers 
            // Add this existing equipment to the new reservation_projet's "equipements" collection
            //resa.equipement = Equip; // ERREURRRRR: :( Cette ligne me créé un déuxième équipement dans la table équipement 
            //resa.equipementID = Equip.id;

            // Ajouter la réference vers l'essai
            //resa.essai = Essai;
            // Ajouter dans ma BDD "reservation_projet"
            context.reservation_projet.Add(resa);

            context.SaveChanges();

            // TODO: Temporaire
            /*var query = (from res in resaDB.reservation_projet
                         where (res.equipement.id == 163)
                         select res).ToArray();*/

            return resa;


        }

        public equipement GetEquipement(int IdEquip)
        {
            return (context.equipement.FirstOrDefault(u => u.id == IdEquip));
        }

        //TODO: Vérifier que je change uniquement l'essai existant
        public void UpdateEssai(essai Essai, DateTime dateInf, DateTime dateSup)
        {
            // Mettre à jour l'essai pour rajouter ces dates
            Essai.date_inf_confidentiel = dateInf;
            Essai.date_sup_confidentiel = dateSup;

            context.SaveChanges();
        }

        #endregion

        #region Méthodes pour la copie des données d'un projet + essais

        /// <summary>
        /// Vérifier que le projet existe
        /// </summary>
        /// <param name="NumProjet"></param>
        /// <returns>true ou false</returns>
        public bool ProjetExists(string NumProjet)
        {
            int countProj = (from proj in context.projet
                             where proj.num_projet == NumProjet
                             select proj).Count();

            // si compteur est égal à zéro alors le numéro de projet n'existe pas 
            if (countProj == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
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
            // Vérifier que le numéro de projet existe et que la personne qui fait la réservation est la propiètaire du projet
            propProjOk = context.projet.Where(p=>p.num_projet == numProjet).Where(p => p.id == usr.Id).Any();
            /*propProjOk = (from p in context.projet
                    where p.num_projet == numProjet && p.compte_userID == usr.Id.ToString()
                    select p).Any();*/
            // Si la personne n'est pas propiètaire du projet mais qu'elle est "Admin" ou "MainAdmin"
            foreach(string roles in allUserRoles)
            {
                if(roles == "Admin" || roles == "MainAdmin")
                {
                    adminSiteOk = true;
                    break;
                }
            }
            return (propProjOk || adminSiteOk);
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

            pr = (from proj in context.projet
                  where proj.num_projet == NumProjet
                  select proj).First();

            return pr;
        }

        /// <summary>
        /// Récupérer un essai par son ID
        /// </summary>
        /// <param name="idEssaie"> id essai</param>
        /// <returns>Essai</returns>
        public essai ObtenirEssai_pourCopie(int idEssaie)
        {
            essai ess = new essai();

            ess = (from essai in context.essai
                   where essai.id == idEssaie
                   select essai).First();

            return ess;
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
            if (proj.type_projet != null)
            {
                idType = (from typeprojet in context.ld_type_projet
                          from pro in context.projet
                          where (pro.id == IdProjet) && (pro.type_projet == typeprojet.nom_type_projet)
                          select typeprojet.id).First();
            }
            else
            {
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
                idResp = (from resp in context.utilisateur
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
            if(es.destination_produit != null)
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

        #endregion

        #region Méthodes pour la vue plan des zones de réservation

        /// <summary>
        /// Obtenir liste de toutes les zones PFL
        /// </summary>
        /// <returns>Liste "zone"</returns>
        public List<zone> ListeZones()
        {
            return context.zone.ToList();
        }

        /// <summary>
        /// Méthode pour obtenir une liste des équipement pour une zone déterminée
        /// </summary>
        /// <param name="idZone">id zone</param>
        /// <returns>Liste des équipements + boolean si équipement réservé ou pas</returns>
        public List<equipement> ListeEquipements(int idZone)
        {
            List<equipement> List = new List<equipement>();

            var query = (from equip in context.equipement
                         where equip.zoneID == idZone
                         select equip).ToArray();

            foreach(var y in query)
            {
                List.Add(y);
            }

            return List;
        }

        /// <summary>
        /// Méthode pour obtenir le nom d'une zone
        /// </summary>
        /// <param name="idZone">id zone</param>
        /// <returns>nom de la zone</returns>
        public string GetNomZone(int idZone)
        {
            return context.zone.First(x => x.id == idZone).nom_zone;
        }

        public string GetNomEquipement(int idEquip)
        {
            return context.equipement.First(x => x.id == idEquip).nom;
        }

        #endregion

        #region Méthodes pour les rêquetes destinées au calendrier 

        /// <summary>
        /// Méthode pour obtenir les réservations de un jour specifique, pour un équipement specifique et séparés selon matin et après-midi
        /// </summary>
        /// <param name="dateResa">date pour appliquer la rêquete BDD</param>
        /// <param name="IdEquipement">Id equipement concerné</param>
        /// <returns>Liste "ReservationsJour" contenant toutes les infos réservation</returns>
        public ReservationsJour ObtenirReservationsJourXEquipement(DateTime dateResa, int IdEquipement)
        {
            // Variables
            ReservationsJour Resas = new ReservationsJour();
            reservation_projet[] InfosResa = new reservation_projet[] { };
            DateTimeFormatInfo dateTimeFormats = null;

            // Requete vers la base de données pour obtenir toutes les réservations du type "projet" s'éxécutant dans la journée en question
            // il faudra vérifier les heures pour déterminer si c'est le matin ou l'aprèm
            InfosResa = (from resa in context.reservation_projet
                         where resa.equipementID == IdEquipement && 
                         ((dateResa.Day >= resa.date_debut.Day) && (dateResa.Month >= resa.date_debut.Month) && (dateResa.Year>= resa.date_debut.Year))&&
                         ((dateResa.Day <= resa.date_fin.Day) && (dateResa.Month <= resa.date_fin.Month) && (dateResa.Year <= resa.date_fin.Year))
                         select resa).Distinct().ToArray(); // Je pense il faut mettre distinct pour récupérer chaque réservation unique

            // Obtenir le nom du jour 
            Resas.JourResa = dateResa; // enregistrer la date en question
                                           // Traduire le nom du jour en cours de l'anglais au Français
            dateTimeFormats = new CultureInfo("fr-FR").DateTimeFormat;
            Resas.NomJour = dateResa.ToString("dddd", dateTimeFormats);
            // TODO: Requete vers la base de données pour obtenir toutes les réservations du type "maintenance" 
            // TODO: Requete vers la base de données pour obtenir toutes les réservations du type "métrologie" 

            // Couper en créneaux Matin et après-midi toutes les réservations de la journée en question
            for (int i = 0; i<InfosResa.Count(); i++) // faire un for pour chaque réservation trouvé
            {             
                if(dateResa == InfosResa[i].date_fin)
                {
                    // Regarder pour définir le créneau
                    if(InfosResa[i].date_fin.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                    {
                        Resas.InfosResaMatin[i] = InfosResa[i];
                    }
                    else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                    {
                        Resas.InfosResaMatin.Add(InfosResa[i]);
                        Resas.InfosResaAprem.Add(InfosResa[i]);
                    }
                }
                else
                {
                    // Ajouter cette résa sur le créneau matin et aprèm 
                    Resas.InfosResaMatin.Add(InfosResa[i]);
                    Resas.InfosResaAprem.Add(InfosResa[i]);
                }
            }


            if(Resas.NomJour!="samedi" && Resas.NomJour != "dimanche")
            {
                // CODE COULEUR DISPO SUR: https://encycolorpedia.fr/
                // Definir les couleurs de fond pour indiquer si le créneau est occupé ou pas
                if (Resas.InfosResaMatin.Count() > 0) // si au moins une réservation le matin alors matinée occupée
                    Resas.CouleurFondMatin = "#fdc0be"; // rouge
                else
                    Resas.CouleurFondMatin = "#a2d9d4"; // matin dispo (Vert)

                if (Resas.InfosResaAprem.Count() > 0) // si au moins une réservation l'aprèm alors aprèm occupée
                    Resas.CouleurFondAprem = "#fdc0be"; // rouge
                else
                    Resas.CouleurFondAprem = "#a2d9d4"; // Aprèm libre (Vert)
            }
            else // si jour samedi ou dimanche alors mettre en fond gris
            {
                Resas.CouleurFondMatin = "silver";
                Resas.CouleurFondAprem = "silver";
            }
            
            return Resas;
        }

        // VOIR si cette méthode marche
        public ReservationsJour ObtenirReservationsJourEssai(DateTime dateResa, int IdEquipement)
        {
            // Variables
            ReservationsJour Resas = new ReservationsJour();
            essai[] SubInfosEssai = new essai[] { };
            List<essai> InfosEssai = new List<essai>();
            DateTimeFormatInfo dateTimeFormats = null;
            DateTime dateSeuilInf = new DateTime();                                                 // RESTREINT: Date à comparer sur chaque réservation pour trouver le seuil inferieur
            DateTime dateSeuilSup = new DateTime();                                                 // RESTREINT: Date à comparer sur chaque réservation pour trouver le seuil supérieur
            reservation_projet ResaAGarder = new reservation_projet();                              // On garde une des réservations de côté (peu importe laquelle car on a juste besoin d'accèder aux infos "essai")
            bool IsEquipInZone = false;
            equipement EquipementPlanning = context.equipement.First(x => x.id == IdEquipement);     // Equipement à enqueter
            //string datResa = dateResa.ToShortDateString();
            // Requete vers la base de données pour obtenir tous les essais qui ont lieu ce jour 
            /*InfosEssai = (from resa in resaDB.reservation_projet
                         from essa in resaDB.essai
                         where resa.essaiID == essa.id &&
                         (Convert.ToDateTime(datResa).Date >= Convert.ToDateTime(resa.date_debut.ToShortDateString()).Date &&
                          Convert.ToDateTime(dateResa.ToShortTimeString()).Date <= Convert.ToDateTime(resa.date_fin.ToShortTimeString()).Date)
                          select essa).Distinct().ToArray(); // Je pense il faut mettre distinct pour récupérer chaque réservation unique*/

            SubInfosEssai = (from resa in context.reservation_projet
                          from essa in context.essai
                          where resa.essaiID == essa.id 
                          select essa).Distinct().ToArray();

            // Résupérer les essais où la date enquêté est bien dans la plage de déroulement
            foreach (var es in SubInfosEssai)
            {
                foreach (var resEs in es.reservation_projet)
                {
                    if ((dateResa.CompareTo(resEs.date_debut) >= 0) && // si dateResa est superieur à resEs.date_debut ou égal 
                        (dateResa.CompareTo(resEs.date_fin) <= 0))  // si dateResa est inferieur à resEs.date_fin ou égal
                    {
                        InfosEssai.Add(es);
                        break;
                    }
                }
            }


            foreach (var ess in InfosEssai)
            {
                switch(ess.confidentialite)
                {
                    case "Ouvert": // Dans ce cas: je regarde si mon équipement est concerné par cet essai et je bloque uniquement l'équipement

                        #region Confidentialité ouverte

                        Resas = ResaConfidentialiteOuverte(ess, IdEquipement, dateResa);

                        #endregion

                        break;
                    case "Restreint": // si "Restreint" il faut bloquer toute la zone, alors vérifier si les réservations sont dans la même zone que l'équipement et bloquer l'équipement 
                        
                        #region Confidentialité "Restreint"

                        bool IsFirstSearchOnEssai = true;

                            #region Recherche des dates superieur et inferieur pour chaque essai

                            foreach (var resa in ess.reservation_projet)
                            {
                                if (resa.equipement.zoneID == EquipementPlanning.zoneID) // si l'équipement objet du "planning" est dans la zone concerné alors il faut le bloquer
                                {
                                    if(EquipementPlanning.zoneID.Equals(EnumZonesPfl.HaloirAp7) || EquipementPlanning.zoneID.Equals(EnumZonesPfl.SalleAp5) ||
                                        EquipementPlanning.zoneID.Equals(EnumZonesPfl.SalleAp6) || EquipementPlanning.zoneID.Equals(EnumZonesPfl.SalleAp8) ||
                                        EquipementPlanning.zoneID.Equals(EnumZonesPfl.SalleAp9))
                                    { 
                                        // Pour ces zones alors faire comment on fait pour les essai du type "Ouvert" blocage uniquement des équipements
                                        Resas = ResaConfidentialiteOuverte(ess, IdEquipement, dateResa);
                                    }
                                    else // pour toutes les autres zones calculer la date seuil inferieur et superieur parmi toutes les réservations
                                    {
                                        if (IsFirstSearchOnEssai == true) // Executer que lors de la premiere réservation de la liste 
                                        {
                                            IsFirstSearchOnEssai = false;
                                            dateSeuilInf = resa.date_debut;
                                            dateSeuilSup = resa.date_fin;
                                        }
                                        else
                                        {
                                            // Recherche des dates superieur et inferieur sur toutes les réservations
                                            if ( resa.date_debut.CompareTo(dateSeuilInf) <= 0 ) // resa.date_debut <= dateSeuilInf)
                                            {
                                                dateSeuilInf = resa.date_debut;
                                            }
                                            if ( resa.date_debut.CompareTo(dateSeuilSup) >= 0 )  // resa.date_fin >= dateSeuilSup)
                                            {
                                                dateSeuilSup = resa.date_fin;
                                            }
                                        }
                                        ResaAGarder = resa;
                                        IsEquipInZone = true;
                                    }
                                }
                            }

                            #endregion

                            #region Bloquer l'équipement si la dateResa est dans les dates seuils rétrouvées

                            if ( (IsEquipInZone == true) && (dateResa.CompareTo(dateSeuilInf) >= 0) && (dateResa.CompareTo(dateSeuilSup) <= 0 ) ) // si l'équipement est dans la zone
                            {
                                if (dateResa.CompareTo(dateSeuilInf) == 0) // début
                                {
                                    // Regarder pour définir le créneau
                                    if (dateSeuilInf.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                    {
                                        Resas.InfosResaAprem.Add(ResaAGarder);
                                        //Resas.InfosResaMatin.Add(null); // Matin vide
                                    }
                                    else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                                    {
                                        Resas.InfosResaMatin.Add(ResaAGarder);
                                        Resas.InfosResaAprem.Add(ResaAGarder);
                                    }
                                }
                                else if (dateResa.CompareTo(dateSeuilSup) == 0) // fin
                                {
                                    // Regarder pour définir le créneau
                                    if (dateSeuilSup.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                                    {
                                        Resas.InfosResaMatin.Add(ResaAGarder);
                                        //Resas.InfosResaAprem.Add(null); // Aprèm vide TODO: voir si on peut ajouter un element null!!
                                    }
                                    else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                                    {
                                        Resas.InfosResaMatin.Add(ResaAGarder);
                                        Resas.InfosResaAprem.Add(ResaAGarder);
                                    }
                                }
                                else 
                                {
                                    // Ajouter cette résa sur le créneau matin et aprèm 
                                    Resas.InfosResaMatin.Add(ResaAGarder);
                                    Resas.InfosResaAprem.Add(ResaAGarder);
                                }
                                IsEquipInZone = false;
                            }

                            #endregion
                        #endregion

                        break;
                    case "Confidentiel": // Blocage de toute la plateforme sauf pour les salles alimentaires (5 zones)

                        #region Confidentialité "confidentiel" 

                        if (EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.HaloirAp7) || EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp5) ||
                            EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp6) || EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp8) ||
                            EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp9))
                        {
                            // Pour ces zones alors faire comment on fait pour les essai du type "Ouvert" blocage uniquement des équipements
                            Resas = ResaConfidentialiteOuverte(ess, IdEquipement, dateResa);
                        }
                        else
                        {
                            if ( (dateResa.CompareTo(ess.date_inf_confidentiel.Value) >= 0) && (dateResa.CompareTo(ess.date_sup_confidentiel.Value) <= 0) )
                            { 
                                // Créer une réservation uniquement pour avoir l'accès à l'essai (A modifier)
                                ResaAGarder = new reservation_projet { equipementID = IdEquipement, essaiID = ess.id, date_debut = ess.date_inf_confidentiel.Value, date_fin = ess.date_sup_confidentiel.Value, essai = ess };
                                if (dateResa.ToShortDateString().CompareTo(ess.date_inf_confidentiel.Value.ToShortDateString()) == 0) // début
                                {
                                    // Regarder pour définir le créneau
                                    if (ess.date_inf_confidentiel.Value.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                    {
                                        Resas.InfosResaAprem.Add(ResaAGarder);
                                        //Resas.InfosResaMatin.Add(null); // Matin vide
                                    }
                                    else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                                    {
                                        Resas.InfosResaMatin.Add(ResaAGarder);
                                        Resas.InfosResaAprem.Add(ResaAGarder);
                                    }
                                }
                                else if (dateResa.ToShortDateString().CompareTo(ess.date_sup_confidentiel.Value.ToShortDateString()) == 0) // fin
                                {
                                    // Regarder pour définir le créneau
                                    if (ess.date_sup_confidentiel.Value.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                                    {
                                        Resas.InfosResaMatin.Add(ResaAGarder);
                                        //Resas.InfosResaAprem.Add(null); // Aprèm vide TODO: voir si on peut ajouter un element null!!
                                    }
                                    else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                                    {
                                        Resas.InfosResaMatin.Add(ResaAGarder);
                                        Resas.InfosResaAprem.Add(ResaAGarder);
                                    }
                                }
                                else
                                {
                                    // Ajouter cette résa sur le créneau matin et aprèm 
                                    Resas.InfosResaMatin.Add(ResaAGarder);
                                    Resas.InfosResaAprem.Add(ResaAGarder);
                                }
                            }          
                        }
                        #endregion
                        break;
                }
            }

            #region Gestion nom du jour et couleurs pour l'affichage

            // Obtenir le nom du jour 
            Resas.JourResa = dateResa; // enregistrer la date en question
                                       // Traduire le nom du jour en cours de l'anglais au Français
            dateTimeFormats = new CultureInfo("fr-FR").DateTimeFormat;
            Resas.NomJour = dateResa.ToString("dddd", dateTimeFormats);
            // TODO: Requete vers la base de données pour obtenir toutes les réservations du type "maintenance" 
            // TODO: Requete vers la base de données pour obtenir toutes les réservations du type "métrologie" 

            if (Resas.NomJour != "samedi" && Resas.NomJour != "dimanche")
            {
                // CODE COULEUR DISPO SUR: https://encycolorpedia.fr/
                // Definir les couleurs de fond pour indiquer si le créneau est occupé ou pas
                if (Resas.InfosResaMatin.Count() > 0) // si au moins une réservation le matin alors matinée occupée
                    Resas.CouleurFondMatin = "#fdc0be"; // rouge
                else
                    Resas.CouleurFondMatin = "#a2d9d4"; // matin dispo (Vert)

                if (Resas.InfosResaAprem.Count() > 0) // si au moins une réservation l'aprèm alors aprèm occupée
                    Resas.CouleurFondAprem = "#fdc0be"; // rouge
                else
                    Resas.CouleurFondAprem = "#a2d9d4"; // Aprèm libre (Vert)
            }
            else // si jour samedi ou dimanche alors mettre en fond gris
            {
                Resas.CouleurFondMatin = "silver";
                Resas.CouleurFondAprem = "silver";
            }

            #endregion

            return Resas;
        }

        #endregion

        #region méthodes externes

        public ReservationsJour ResaConfidentialiteOuverte(essai ess, int IdEquipement, DateTime dateResa)
        {
            ReservationsJour Resas = new ReservationsJour();

            foreach (var resa in ess.reservation_projet)
            {
                if ((resa.equipementID == IdEquipement) && ((dateResa.CompareTo(resa.date_debut) >=0) && (dateResa.CompareTo(resa.date_fin) <= 0)))
                    //((Convert.ToDateTime(dateResa.ToShortDateString()).Date >= Convert.ToDateTime(resa.date_debut.ToShortDateString()).Date) &&
                    //(Convert.ToDateTime(dateResa.ToShortDateString()).Date <= (Convert.ToDateTime(resa.date_fin.ToShortDateString()).Date)))) // Si l'équipement à afficher est impliqué dans l'essai
                {
                    if ( dateResa.ToShortDateString().CompareTo(resa.date_debut.ToShortDateString()) == 0 ) // si dateResa égal à resa.date_debut
                    {
                        // Regarder pour définir le créneau
                        if (resa.date_debut.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                        {
                            Resas.InfosResaAprem.Add(resa);
                            //Resas.InfosResaMatin.Add(null); // Matin vide
                        }
                        else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                        {
                            Resas.InfosResaMatin.Add(resa);
                            Resas.InfosResaAprem.Add(resa);
                        }
                    }
                    else if ( dateResa.ToShortDateString().CompareTo(resa.date_fin.ToShortDateString()) == 0 ) // si dateResa égal à resa.date_fin
                    {
                        // Regarder pour définir le créneau
                        if (resa.date_fin.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                        {
                            Resas.InfosResaMatin.Add(resa);
                            //Resas.InfosResaAprem.Add(null); // Aprèm vide TODO: voir si on peut ajouter un element null!!
                        }
                        else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                        {
                            Resas.InfosResaMatin.Add(resa);
                            Resas.InfosResaAprem.Add(resa);
                        }
                    }
                    else // date à l'intérieur du seuil de réservation
                    {
                        // Ajouter cette résa sur le créneau matin et aprèm 
                        Resas.InfosResaMatin.Add(resa);
                        Resas.InfosResaAprem.Add(resa);
                    }

                }
            }

            return Resas;
        }

        #endregion
    }
}