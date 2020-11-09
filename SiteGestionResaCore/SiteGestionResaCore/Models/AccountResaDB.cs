using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Models
{
    public class AccountResaDB: IAccountResaDB
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext context;
        private readonly ILogger<AccountResaDB> logger;

        public AccountResaDB(
            GestionResaContext resaDB,
            ILogger<AccountResaDB> logger)
        {
            this.context = resaDB;
            this.logger = logger;
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
            context.Users.Add(new utilisateur { nom = Nom, prenom = Prenom, Email = Email, organismeID = organismeId });
            context.SaveChanges();
        }



        

    }
}
