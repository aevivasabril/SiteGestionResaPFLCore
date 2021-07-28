using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    public interface IReservationDb
    {
        reservation_projet CreationReservation(int EquipId, essai Essai, DateTime dateDebut, DateTime dateFin);

        ReservationsJour ObtenirReservationsJourEssai(DateTime dateResa, int IdEquipement);

        bool VerifDisponibilitéEquipement(DateTime dateDebut, DateTime dateFin, int idEquipement);

        string ObtenirNomEquipement(int id);

        bool DispoEssaiRestreintPourAjout(DateTime dateDebut, DateTime dateFin, int idEquipement, int IdEssai);

        bool DispoEssaiConfidentielPourAjout(DateTime dateDebut, DateTime dateFin, int idEquipement, int IdEssai);

    }
}
