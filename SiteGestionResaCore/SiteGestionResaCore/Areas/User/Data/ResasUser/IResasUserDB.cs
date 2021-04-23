using SiteGestionResaCore.Data;
using SiteGestionResaCore.Models;
using SiteGestionResaCore.Models.EquipementsReserves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.ResasUser
{
    public interface IResasUserDB
    {
        List<InfosResasUser> ObtenirResasUser(int IdUsr, string OpenPartialEssai, string OpenReservations, int IdEssai);

        essai ObtenirEssaiPourModif(int IdEssai);

        bool IsEssaiModifiable(int IdEssai);

        List<ld_produit_in> ListProduitEntree();

        List<ld_provenance_produit> ListProveProduit();

        List<ld_destination> ListDestinationPro();

        int IdProvProduitToCopie(int IdEssai);

        int IdDestProduitToCopie(int IdEssai);

        int IdProduitInToCopie(int IdEssai);

        ConsultInfosEssaiChildVM ObtenirInfosEssai(int idEssai);

        bool UpdateConfidentialiteEss(essai ess, string confidentialite);

        bool UpdateManipID(essai ess, int selecManipulateurID);

        bool compareTypeProdEntree(string TypeProdEntrant, int SelProductId);

        bool UpdateProdEntree(essai essa, int prodEntId);

        bool UpdatePrecisionProd(essai essa, string precision);

        bool UpdateQuantiteProd(essai essa, int? quantite);

        bool compareProvProd(string provProduit, int SelProvId);

        bool UpdateProvProd(essai essa, int SelectProvProduitId);

        bool compareDestProd(string destination_produit, int selectDestProduit);

        bool UpdateDestProd(essai essa, int SelectDestProduit);

        bool UpdateTransport(essai essa, string TranspSTLO);

        bool UpdateTitre(essai essa, string commentEssai);

        List<InfosResasEquipement> ResasEssai(int id);

        reservation_projet ObtenirResa(int IdReservation);

        bool SupprimerResa(int idResa);
    }
}
