using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Data;
using SiteReservationGestionPFL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiteReservationGestionPFL.Areas.Equipe.Data
{
    #region View model pour la page GestionUtilisateurs
    public class GestionUsersViewModel
    {
        /// <summary>
        /// "utilisateur" objet d'un changement (valider/refuser ouverture de compte, changement de rôle ou suppression de compte)
        /// </summary>
        public utilisateur UserToChange = new utilisateur();
       
        private List<utilisateur> _usersAdmin;
        /// <summary>
        /// List des utilisateurs dont le rôle est 'Admin'
        /// </summary>
        public List<utilisateur> UsersAdmin
        {
            get
            {
                using (IResaDB resaBdd = new ResaDB())
                {
                    _usersAdmin = new List<utilisateur>(resaBdd.ObtenirListAdmins());
                }
                return _usersAdmin;
            }
            set { _usersAdmin = value; }
        }

        private List<utilisateur> _listUsers;
        /// <summary>
        /// Liste des utilisateurs avec un rôle 'user'
        /// </summary>
        public List<utilisateur> ListUsers
        {
            get 
            {
                using (IResaDB resaBdd = new ResaDB())
                {
                    _listUsers = new List<utilisateur>(resaBdd.ObtenirListAutres().Users);
                }
                return _listUsers;
            }
            set { _listUsers = value; }
        }

        private List<utilisateur> _listUsersWaiting;
        /// <summary>
        /// Liste des utilisateurs en attente de validation de compte
        /// </summary>
        public List<utilisateur> ListUsersWaiting
        {
            get
            {
                using (IResaDB resaBdd = new ResaDB())
                {
                    _listUsersWaiting = new List<utilisateur>(resaBdd.ObtenirListAutres().UsersWaitingValid);
                }
                return _listUsersWaiting;
            }
            set { _listUsersWaiting = value; }
        }

        private List<utilisateur> _ListAdminLogistic;
        /// <summary>
        /// List des utilisateurs dont le rôle est 'Admin'
        /// </summary>
        public List<utilisateur> ListAdminLogistic
        {
            get
            {
                using (IResaDB resaBdd = new ResaDB())
                {
                    _ListAdminLogistic = new List<utilisateur>(resaBdd.ObtenirListAdminsLogistiqueAsync());
                }
                return _ListAdminLogistic;
            }
            set { _ListAdminLogistic = value; }
        }

        /// <summary>
        /// Id de l'utilisateur selectionné pour le changement "utilisateur" à "admin"
        /// </summary>
        [Range(1, 100, ErrorMessage = "Selectionnez un utilisateur")]
        public int UserToUpdateId { get; set; }

        /// <summary>
        /// Créer une liste déroulante contenant les données à afficher sur une DropDownList
        /// </summary>
        public IEnumerable<SelectListItem> UserItem
        {
            get
            {
                var allUsrs = ListUsers.Select(f => new SelectListItem
                {
                    Value = f.id.ToString(),
                    Text = f.nom + ", "+f.prenom + "( "+ f.Email + " )"
                });;
                return DefaultUsrItem.Concat(allUsrs);
            }
        }

        /// <summary>
        /// Premier Item par défaut de la liste déroulante
        /// </summary>
        public IEnumerable<SelectListItem> DefaultUsrItem
        {
            get
            {
                return Enumerable.Repeat(new SelectListItem
                {
                    Value = "-1",
                    Text = "- Selectionner un utilisateur -"
                }, count: 1);
            }
        }


        /// <summary>
        /// Id de l'administrateur selectionné pour l'ajout dans le rôle "Logistic"
        /// </summary>
        [Range(1, 100, ErrorMessage = "Selectionnez un administrateur")]
        public int AdminToLogisticId { get; set; }

        /// <summary>
        /// Créer une liste déroulante contenant les données à afficher sur une DropDownList
        /// </summary>
        public IEnumerable<SelectListItem> AdminItem
        {
            get
            {
                var allUsrs = UsersAdmin.Select(f => new SelectListItem
                {
                    Value = f.id.ToString(),
                    Text = f.nom + ", " + f.prenom + "( " + f.Email + " )"
                }); ;
                return DefaultAdminItem.Concat(allUsrs);
            }
        }

        /// <summary>
        /// Premier Item par défaut de la liste déroulante
        /// </summary>
        public IEnumerable<SelectListItem> DefaultAdminItem
        {
            get
            {
                return Enumerable.Repeat(new SelectListItem
                {
                    Value = "-1",
                    Text = "- Selectionner un administrateur -"
                }, count: 1);
            }
        }

        /// <summary>
        /// Nom de l'ActionResult à utiliser sur un utilisateur (refuser ou valider)
        /// </summary>
        public string ActionName { get; set; }
    }

    /// <summary>
    /// Classe pour séparer les utilisateurs en 2 listes (compte validée ou non validée)
    /// </summary>
    public class listAutresUtilisateurs
    {
        /// <summary>
        /// Utilisateurs avec une compte validée
        /// </summary>
        public List<utilisateur> Users;

        /// <summary>
        /// Utilisateurs avec une compte non validée 
        /// </summary>
        public List<utilisateur> UsersWaitingValid;

        /// <summary>
        /// Méthode pour séparer les 2 listes 
        /// </summary>
        /// <param name="utilisateurs">Utilisateurs avec une compte validée</param>
        /// <param name="usersWaiting">Utilisateurs avec une compte non validée</param>
        public listAutresUtilisateurs(List<utilisateur> utilisateurs, List<utilisateur> usersWaiting)
        {
            Users = utilisateurs;
            UsersWaitingValid = usersWaiting;
        }
    }
    #endregion
}