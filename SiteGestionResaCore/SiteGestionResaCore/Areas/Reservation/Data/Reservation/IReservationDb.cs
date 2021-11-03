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
       
        bool VerifDisponibilitéEquipementOuvert(DateTime dateDebut, DateTime dateFin, int idEquipement);
       
        bool VerifDisponibilitéEquipementRestreint(DateTime dateDebut, DateTime dateFin, int idEquipement);

        bool VerifDisponibilitéEquipementConfidentiel(DateTime dateDebut, DateTime dateFin, int idEquipement);

        string ObtenirNomEquipement(int id);
        
        bool DispoEssaiRestreintPourAjout(DateTime dateDebut, DateTime dateFin, int idEquipement, int IdEssai);

        bool DispoEssaiConfidentielPourAjout(DateTime dateDebut, DateTime dateFin, int idEquipement, int IdEssai);

        string ObtenirNomTypeMaintenance(int id);

        List<int> ObtenirListResasXAnnulationZone(DateTime debutToSave, DateTime finToSave, int idEquipement);

        List<int> ObtenirListResasXAnnulationEquipement(DateTime debutToSave, DateTime finToSave, int idEquipement);

        string ObtenirNomZoneImpacte(int IdEquipement);

        bool ZoneDisponibleXIntervention(DateTime dateDebut, DateTime dateFin, int idEquipement);

        bool EquipementDisponibleXIntervention(DateTime dateDebut, DateTime dateFin, int idEquipement);

        bool VerifDisponibilitZoneEquipSurInterventions(DateTime dateDebut, DateTime dateFin, int idEquipement);
        
        bool VerifDisponibilitEquipSurInterventions(DateTime dateDebut, DateTime dateFin, int idEquipement);

        string ObtenirNumGMAOEquip(int IdEquipement);

    }
}
