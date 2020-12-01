using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data.Validation
{
    public interface IResaAValiderDb
    {
        InfosEssai ObtenirInfosEssai(int idEssai);
        Task<IList<InfosAffichage>> ObtenirInfosAffichageAsync();
        InfosProjet ObtenirInfosProjet(int idProj);
        List<InfosReservation> InfosReservations(int idEssai);
        List<InfosConflit> InfosConflits(int idEssai);
        InfosProjet ObtenirInfosProjetFromEssai(int idEssai);
        bool ValiderEssai(int idEssai);
        bool RefuserEssai(int idEssai, string raisonRefus);
        essai ObtenirEssai(int idEssai);
    }
}
