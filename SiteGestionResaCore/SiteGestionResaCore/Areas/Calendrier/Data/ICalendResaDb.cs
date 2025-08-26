using SiteGestionResaCore.Data;
using SiteGestionResaCore.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    public interface ICalendResaDb
    {
        //List<InfosCalenZone> ObtenirZonesVsEquipements();

        Task<IList<zone>> ListeZonesAsync();

        Task<IList<equipement>> ListeEquipementsAsync(int ZoneID);

        //InfosCalenZone ResasEquipementsParZone(DateTime DateRecup, int NbJours, int ZoneId);

        Task<ResasEquipParJour> ResasEquipementParJourAsync(int IdEquipement, DateTime DateRecup);

        InfosEquipementReserve ObtenirInfosResa(int IdEssai);

        essai ObtenirEssai(int IdEssai);

        projet ObtenirProjetEssai(essai Essai);

        InfosAffichageMaint ObtenirInfosInter(int IdMaint);
    }
}
