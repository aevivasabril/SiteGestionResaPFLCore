using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Models
{
    /// <summary>
    /// Interface contenant les méthodes d'accès aux données pour le contrôleur AccountController
    /// pour création de compte
    /// </summary>
    public interface IAccountResaDB: IDisposable
    {
        void CreerUtilisateur(string nom, string prenom, int organismeId, string email);
    }
}
