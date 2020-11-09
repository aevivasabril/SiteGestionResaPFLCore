using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    public interface IProjetEssaiResaDb
    {
        bool ProjetExists(string NumeroProjet);

        Task<bool> VerifPropieteProjetAsync(string numProjet, utilisateur usr);

        List<EssaiUtilisateur> ObtenirList_EssaisUser(string NumeroProjet);

        projet ObtenirProjet_pourCopie(string numProjet);

        essai ObtenirEssai_pourCopie(int essaiID);

        int IdTypeProjetPourCopie(int IdProjet);

        int IdFinancementPourCopie(int IdProjet);

        int IdRespoProjetPourCopie(int IdProjet);

        int IdProvenancePourCopie(int IdProjet);

        int IdProvProduitPourCopie(int IdEssai);

        int IdDestProduitPourCopie(int IdEssai);

        int IdProduitInPourCopie(int IdEssai);

        projet CreationProjet(string TitreProjet, int typeProjetId, int financId, int orgId,
          int respProjetId, string numProj, int provProj, string description, DateTime dateCreation, utilisateur User);
        
        essai CreationEssai(projet pr, utilisateur Usr, DateTime myDateTime, string confidentialite, int manipId, int ProdId, string precisionProd, string QuantProd,
                int ProvId, int destProduit, string TransStlo, string commentaire);

        void UpdateEssai(essai Essai, DateTime dateInf, DateTime dateSup);

        IEnumerable<SelectListItem> ListEssaisToSelectItem(List<EssaiUtilisateur> essais);
    }
}
