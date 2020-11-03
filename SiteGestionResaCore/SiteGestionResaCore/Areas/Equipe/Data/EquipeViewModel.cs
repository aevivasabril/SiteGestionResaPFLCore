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
            get { return _usersAdmin; }
            set { _usersAdmin = value; }
        }

        private List<utilisateur> _listUsers = new List<utilisateur>();
        /// <summary>
        /// Liste des utilisateurs avec un rôle 'user'
        /// </summary>
        public List<utilisateur> ListUsers
        {
            get { return _listUsers; }
            set { _listUsers = value; }
        }

        private List<utilisateur> _listUsersWaiting;
        /// <summary>
        /// Liste des utilisateurs en attente de validation de compte
        /// </summary>
        public List<utilisateur> ListUsersWaiting
        {
            get { return _listUsersWaiting; }
            set { _listUsersWaiting = value; }
        }

        private IList<utilisateur> _ListAdminLogistic;
        /// <summary>
        /// List des utilisateurs dont le rôle est 'Admin'
        /// </summary>
        public IList<utilisateur> ListAdminLogistic
        {
            get { return _ListAdminLogistic; }
            set { _ListAdminLogistic = value; }
        }

        /// <summary>
        /// Id de l'utilisateur selectionné pour le changement "utilisateur" à "admin"
        /// </summary>
        [Range(1, 100, ErrorMessage = "Selectionnez un utilisateur")]
        public int UserToUpdateId { get; set; }

        private IEnumerable<SelectListItem> _userItem = new SelectList(new[] { new SelectListItem { Text = " ", Value = " ", Selected = false }}); 

        public IEnumerable<SelectListItem> UserItem
        {
            get { return _userItem; }
            set { _userItem = value; }
        }

        /// <summary>
        /// Créer une liste déroulante contenant les données à afficher sur une DropDownList
        /// </summary>
        /*public IEnumerable<SelectListItem> UserItem
        {
            get
            {
                var allUsrs = ListUsers.Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = f.nom + ", "+f.prenom + "( "+ f.Email + " )"
                });;
                return DefaultUsrItem.Concat(allUsrs);
            }
        }*/

        /// <summary>
        /// Premier Item par défaut de la liste déroulante
        /// </summary>
        /*public IEnumerable<SelectListItem> DefaultUsrItem
        {
            get
            {
                return Enumerable.Repeat(new SelectListItem
                {
                    Value = "-1",
                    Text = "- Selectionner un utilisateur -"
                }, count: 1);
            }
        }*/


        /// <summary>
        /// Id de l'administrateur selectionné pour l'ajout dans le rôle "Logistic"
        /// </summary>
        [Range(1, 100, ErrorMessage = "Selectionnez un administrateur")]
        public int AdminToLogisticId { get; set; }

        private IEnumerable<SelectListItem> _AdminItem = new SelectList(new[] { new SelectListItem { Text = " ", Value = " ", Selected = false } });

        public IEnumerable<SelectListItem> AdminItem
        {
            get { return _AdminItem; }
            set { _AdminItem = value; }
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