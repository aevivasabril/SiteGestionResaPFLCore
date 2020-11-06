using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    public interface IFormulaireResaDb: IDisposable
    {
        List<ld_type_projet> ObtenirList_TypeProjet();

        List<ld_financement> ObtenirList_Financement();

        List<organisme> ObtenirListOrg();

        List<utilisateur> ObtenirList_UtilisateurValide();

        List<ld_provenance_produit> ObtenirList_ProvenanceProduit();

        List<ld_provenance> ObtenirList_ProvenanceProjet();

        List<ld_produit_in> ObtenirList_TypeProduitEntree();

        List<ld_destination> ObtenirList_DestinationPro();

        IEnumerable<SelectListItem> ListTypeProjetItem(List<ld_type_projet> typeProj);

        IEnumerable<SelectListItem> ListFinancementItem(List<ld_financement> financement);

        IEnumerable<SelectListItem> ListOrgItem(List<organisme> organismes);

        IEnumerable<SelectListItem> ListRespItem(List<utilisateur> respProj);

        IEnumerable<SelectListItem> ListProveItem(List<ld_provenance> provProj);

        IEnumerable<SelectListItem> ListManipItem(List<utilisateur> manipulateur);

        IEnumerable<SelectListItem> ListProdEntreeItem(List<ld_produit_in> produitEnt);

        IEnumerable<SelectListItem> ListProvProdItem(List<ld_provenance_produit> provProduit);

        IEnumerable<SelectListItem> ListDestProdItem(List<ld_destination> destProduit);
    }
}
