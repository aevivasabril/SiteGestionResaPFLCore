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
            context.utilisateur.Add(new utilisateur { nom = Nom, prenom = Prenom, Email = Email, organismeID = organismeId });
            context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    context?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AccountResaDB()
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
