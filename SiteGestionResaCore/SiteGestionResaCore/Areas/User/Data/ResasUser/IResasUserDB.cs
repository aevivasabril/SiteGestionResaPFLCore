using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.ResasUser
{
    public interface IResasUserDB
    {
        List<InfosResasUser> ObtenirResasUser(int IdUsr, string openPartial, int IdEssai);

        essai ObtenirEssaiPourModif(int IdEssai);

        bool IsEssaiModifiable(int IdEssai);

        List<ld_produit_in> ListProduitEntree();

        List<ld_provenance_produit> ListProveProduit();

        List<ld_destination> ListDestinationPro();

        int IdProvProduitToCopie(int IdEssai);

        int IdDestProduitToCopie(int IdEssai);

        int IdProduitInToCopie(int IdEssai);

        ConsultInfosEssaiChilVM ObtenirInfosEssai(int idEssai);
    }
}
