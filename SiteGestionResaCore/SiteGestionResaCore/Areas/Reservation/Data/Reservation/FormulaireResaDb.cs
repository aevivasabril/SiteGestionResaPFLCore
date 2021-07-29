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
        private readonly UserManager<utilisateur> userManager;

        //private readonly UserManager<utilisateur> userManager;
        //private readonly ILogger<FormulaireResaDb> logger;

        public FormulaireResaDb(
            GestionResaContext resaDB,
            UserManager<utilisateur> userManager)
        {
            this.context = resaDB;
            this.userManager = userManager;
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
            return context.Users.Where(e => e.EmailConfirmed == true && e.compteInactif != true).OrderBy(u => u.nom).ToList(); 
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

        public async Task<IList<utilisateur>> ObtenirLogisticUsersAsync()
        {
            return await userManager.GetUsersInRoleAsync("Logistic");
        }

        public async Task<IList<utilisateur>> ObtenirMainAdmUsersAsync()
        {
            return await userManager.GetUsersInRoleAsync("MainAdmin");
        }

        /// <summary>
        /// Liste des organismes déclarés sur la BDD
        /// </summary>
        /// <returns>liste des organismes </returns>
        public List<ld_equipes_stlo> ObtenirListEquips()
        {
            return context.ld_equipes_stlo.ToList();
        }
    }
}
