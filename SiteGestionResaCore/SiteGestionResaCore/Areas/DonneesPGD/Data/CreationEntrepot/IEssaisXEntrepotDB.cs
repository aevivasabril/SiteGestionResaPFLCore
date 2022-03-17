using SiteGestionResaCore.Data;
using SiteGestionResaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data
{
    public interface IEssaisXEntrepotDB
    {
        public List<InfosResasSansEntrepot> ObtenirResasSansEntrepotUsr(utilisateur usr);
        public InfosProjet ObtenirInfosProjet(int id);
        public ConsultInfosEssaiChildVM ObtenirInfosEssai(int idEssai);
        public List<InfosReservation> InfosReservations(int idEssai);
        public List<ReservationsXEssai> ListeReservationsXEssai(int idEssai);
        public List<type_document> ListeTypeDocuments();
        public string ObtenirNomActivite(int id);
        public int ObtenirIDActivite(int id);
        public List<type_document> ListeTypeDocumentsXActivite(int IdActivite);
        public string ObtenirNomTypeDonnees(int IdTypeDonnees);
        public List<type_document> ListeTypeDocumentsXEquip(int IdEquipement);
        public int ObtenirIdActiviteXequip(int id);
        public bool EcrireDocTypeUn(CreationEntrepotVM model);
        public bool EcrireDocTypeDeux(CreationEntrepotVM model);
        public void UpdateEssaiXEntrepot(int idEssai);
        public reservation_projet ObtenirResa(int IdResa);
        public essai ObtenirEssai(int idEssai);
        public bool SaveDocEssaiPgd(doc_essai_pgd doc, string typedoc);
        public equipement ObtenirEquipement(int IdEquip);
        public projet ObtenirProjetXEssai(int IdProjet);
        public organisme ObtenirOrgXProj(int IdOrg);
        public string ObtenirTitreEssai(int idEssai);
    }
}
