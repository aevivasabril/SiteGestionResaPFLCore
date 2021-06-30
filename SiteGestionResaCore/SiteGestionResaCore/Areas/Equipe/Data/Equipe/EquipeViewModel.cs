using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiteGestionResaCore.Areas.Equipe.Data
{
    #region View model pour la page GestionUtilisateurs
    public class GestionUsersViewModel
    {
        /// <summary>
        /// "utilisateur" objet d'un changement (valider/refuser ouverture de compte, changement de rôle ou suppression de compte)
        /// </summary>
        public utilisateur UserToChange = new utilisateur();
       
        private List<utilisateur> _usersAdmin = new List<utilisateur>();
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

        private List<utilisateur> _listUsersWaiting = new List<utilisateur>();
        /// <summary>
        /// Liste des utilisateurs en attente de validation de compte
        /// </summary>
        public List<utilisateur> ListUsersWaiting
        {
            get { return _listUsersWaiting; }
            set { _listUsersWaiting = value; }
        }

        private IList<utilisateur> _ListAdminLogistic = new List<utilisateur>();
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
        [Range(1, 100, ErrorMessage = "Sélectionnez un utilisateur")]
        public int UserToUpdateId { get; set; }

        public IEnumerable<SelectListItem> UserItem { get; set; }
                     

        /// <summary>
        /// Id de l'administrateur selectionné pour l'ajout dans le rôle "Logistic"
        /// </summary>
        [Range(1, 100, ErrorMessage = "Sélectionnez un administrateur")]
        public int AdminToLogisticId { get; set; }

        public IEnumerable<SelectListItem> AdminItem { get; set; }

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
        public List<utilisateur> Users = new List<utilisateur>();

        /// <summary>
        /// Utilisateurs avec une compte non validée 
        /// </summary>
        public List<utilisateur> UsersWaitingValid = new List<utilisateur>();

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