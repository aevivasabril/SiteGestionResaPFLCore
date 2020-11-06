using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    public class FormulaireResaDb: IFormulaireResaDb
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext context;
        //private readonly UserManager<utilisateur> userManager;
        //private readonly ILogger<FormulaireResaDb> logger;

        public FormulaireResaDb(
            GestionResaContext resaDB/*,
            UserManager<utilisateur> userManager,
            ILogger<FormulaireResaDb> logger*/)
        {
            this.context = resaDB;
            //this.userManager = userManager;
            //this.logger = logger;
        }

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
        /// Liste des organismes déclarés sur la BDD
        /// </summary>
        /// <returns>liste des organismes </returns>
        public List<organisme> ObtenirListOrg()
        {
            return context.organisme.ToList();
        }

        /// <summary>
        /// Retrouver les utilisateurs dont le compte a déjà été validé et surtout le mail confirmé via le lien envoyé par mail
        /// </summary>
        /// <returns> liste des utilisateurs</returns>
        public List<utilisateur> ObtenirList_UtilisateurValide()
        {
            return context.utilisateur.Where(e => e.EmailConfirmed == true).ToList();
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
        /// Obtenir liste des options provenance produit
        /// </summary>
        /// <returns>List<ld_provenance_produit></returns>
        public List<ld_provenance_produit> ObtenirList_ProvenanceProduit()
        {
            return context.ld_provenance_produit.ToList();
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
        /// Obtenir liste des options destinaition produit sortie
        /// </summary>
        /// <returns></returns>
        public List<ld_destination> ObtenirList_DestinationPro()
        {
            return context.ld_destination.ToList();
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
            var DefaultDestProItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = "- Options -" }, count: 1);
            // Création d'une liste dropdownlit pour selectionner la destinaison produit sortie
            var allOrgs = destProduit.Select(f => new SelectListItem
            {
                Value = f.id.ToString(),
                Text = f.nom_destination
            });
            return DefaultDestProItem.Concat(allOrgs);
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
        // ~FormulaireResaDb()
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
