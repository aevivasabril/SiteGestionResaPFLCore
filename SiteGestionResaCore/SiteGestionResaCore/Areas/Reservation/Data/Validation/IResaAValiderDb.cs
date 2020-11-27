using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data.Validation
{
    public interface IResaAValiderDb
    {
        List<InfosAffichage> ResasAValider();
        InfosEssai ObtenirInfosEssai(int idEssai);
        Task<IList<InfosAffichage>> ObtenirInfosAffichageAsync();
        InfosProjet ObtenirInfosProjet(int id);
        List<InfosReservation> InfosReservations(int idEssai);
        List<InfosConflit> InfosConflits(int idEssai);
    }
}
