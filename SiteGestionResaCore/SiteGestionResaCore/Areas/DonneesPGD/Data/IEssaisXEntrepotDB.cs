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
    }
}
