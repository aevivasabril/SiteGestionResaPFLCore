using SiteGestionResaCore.Data;
using SiteGestionResaCore.Models.Maintenance;
using System;
using System.Collections.Generic;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    public interface ICalendResaDb
    {
        //List<InfosCalenZone> ObtenirZonesVsEquipements();

        List<zone> ListeZones();

        List<equipement> ListeEquipements(int ZoneID);

        //InfosCalenZone ResasEquipementsParZone(DateTime DateRecup, int NbJours, int ZoneId);

        ResasEquipParJour ResasEquipementParJour(int IdEquipement, DateTime DateRecup);

        InfosEquipementReserve ObtenirInfosResa(int IdEssai);

        essai ObtenirEssai(int IdEssai);

        projet ObtenirProjetEssai(essai Essai);

        InfosAffichageMaint ObtenirInfosInter(int IdMaint);
    }
}
