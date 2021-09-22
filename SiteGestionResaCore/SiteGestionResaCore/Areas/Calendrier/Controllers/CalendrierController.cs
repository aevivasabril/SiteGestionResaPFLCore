using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SiteGestionResaCore.Areas.Calendrier.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.PcVue;
using SiteGestionResaCore.Extensions;

namespace SiteGestionResaCore.Areas.Calendrier.Controllers
{
    [Area("Calendrier")]
    public class CalendrierController : Controller
    {
        private readonly ICalendResaDb CalendResaDb;
        private readonly PcVueContext pcVueDb;

        public CalendrierController(
           ICalendResaDb CalendResaDb,
           PcVueContext PcVueDb)
        {
            this.CalendResaDb = CalendResaDb;
            pcVueDb = PcVueDb;
        }

        /// <summary>
        /// Action pour obtenir les infos de réservation pour une semaine
        /// </summary>
        /// <returns></returns>
        public IActionResult CalendrierPFL(CalenViewModel vm)
        {
            //Liberer la session si la personne reclique sur le ménu calendrier?

            var x = DonneesCalendrierPFL(true, null, null);

            // Premier affichage du calendrier à complèter pour une semaine
            vm = new CalenViewModel()
            {
                ListResasZone = x.Item2, 
                JoursCalendrier = x.Item1,
                InfosPopUpEquipement = new InfosEquipementReserve()
            };

            // Créer une session pour éviter de reappliquer les opérations de lecture vers la BDD dans le cas où on clique pour avoir des infos 
            this.HttpContext.AddToSession("CalenViewModel", vm);

            return View(vm);
        }

        [HttpPost]
        public IActionResult AfficherPlanningDuAu(CalenViewModel model)
        {
            if (model.DateAu != null && model.DateDu != null) // Vérification uniquement des datePicker pour l'affichage du calendrier
            {
                if (model.DateDu.Value <= model.DateAu.Value)
                {
                    var x = DonneesCalendrierPFL(false, model.DateDu, model.DateAu);

                    model.JoursCalendrier = x.Item1;
                    model.ListResasZone = x.Item2;
                    model.InfosPopUpEquipement = new InfosEquipementReserve();

                    // Mettre à jour la session pour les dates souhaitées
                    this.HttpContext.AddToSession("CalenViewModel", model);
                }
                else
                {
                    // Récupérer la session "CalenViewModel" où se trouvent toutes les informations des réservations pour toute la PFL
                    model = HttpContext.GetFromSession<CalenViewModel>("CalenViewModel");
                    ModelState.AddModelError("", "La date fin pour l'affichage du planning équipement ne peut pas être inférieure à la date début");
                }
            }
            else
            {
                // Récupérer la session "CalenViewModel" où se trouvent toutes les informations des réservations pour toute la PFL
                model = HttpContext.GetFromSession<CalenViewModel>("CalenViewModel");
                ModelState.AddModelError("", "Oups! Vous avez oublié de saisir les dates! ");
            }


            return View("CalendrierPFL", model);
        }

        public IActionResult VoirInfosEssai(int id)
        {
            // Récupérer la session "CalenViewModel" où se trouvent toutes les informations des réservations pour toute la PFL

            // Obtenir les infos à afficher pour l'essai demandé
            InfosEquipementReserve InfosResa = CalendResaDb.ObtenirInfosResa(id);

            return PartialView ("_InfosResaCalendrier", InfosResa);
        }

        /// <summary>
        /// Ouverture du pop-up pour affichage des infos opération Maintenance
        /// </summary>
        /// <param name="id">Id maintenance</param>
        /// <returns></returns>
        public IActionResult VoirInfosInterv(int id)
        {
            // Obtenir les infos à afficher pour l'intervention maintenance demandé
            InfosAffichageMaint InfosInterv = CalendResaDb.ObtenirInfosInter(id);

            return PartialView("_InfosIntervCalendrier", InfosInterv);
        }


        #region Méthodes supplementaires

        /// <summary>
        /// Méthode inspiré de la méthode "DonneesCalendrierEquipement" du contrôlleur RESERVATION
        /// </summary>
        /// <param name="IsForOneWeek"></param>
        /// <param name="DateDu"></param>
        /// <param name="DateAu"></param>
        /// <returns>List<JourCalendrier>, List<ResasZone></returns>
        public (List<JourCalendrier>, List<ResasZone>) DonneesCalendrierPFL(bool IsForOneWeek, DateTime? DateDu, DateTime? DateAu)
        {
            List<JourCalendrier> ListCalendrierParZone = new List<JourCalendrier>();
            List<ResasEquipParJour> ListResEquipParjour = new List<ResasEquipParJour>();
            ResasZone resasZone = new ResasZone();
            EquipementVsResa equipementVsResa = new EquipementVsResa();
            DateTimeFormatInfo dateTimeFormats = null;
            List<ResasZone> ListResasZone = new List<ResasZone>();

            // pour ResasZone
            OccupationZonesParJour occupation = new OccupationZonesParJour();
            List<OccupationZonesParJour> ListeDispoZonesDuAu = new List<OccupationZonesParJour>();
            List<EquipementVsResa> ListEquiVsResa = new List<EquipementVsResa>();

            DateTime TodayDate = new DateTime(); // Datetime pour obtenir la date actuelle
            DateTime DateRecup = new DateTime();
            DateTime SaveRecupDate = new DateTime();

            int NbJours = 0;

            //Afficher toujours à partir du lundi de la semaine en cours 
            TodayDate = DateTime.Now;
            DayOfWeek dow = TodayDate.DayOfWeek;

            #region Déterminer s'il s'agit d'un calendrier pour une semaine ou pour une durée déterminée

            switch (IsForOneWeek)
            {
                case true:
                    // Revenir toujours au lundi de la semaine en cours
                    if (dow == DayOfWeek.Tuesday) TodayDate = TodayDate.AddDays(-1);
                    if (dow == DayOfWeek.Wednesday) TodayDate = TodayDate.AddDays(-2);
                    if (dow == DayOfWeek.Thursday) TodayDate = TodayDate.AddDays(-3);
                    if (dow == DayOfWeek.Friday) TodayDate = TodayDate.AddDays(-4);
                    if (dow == DayOfWeek.Saturday) TodayDate = TodayDate.AddDays(-5);
                    if (dow == DayOfWeek.Sunday) TodayDate = TodayDate.AddDays(-6);

                    NbJours = 7;
                    // Ajouter l'heure (à 7h00) car sinon on va avoir un problème pour faire la comparaison lors de la récupération des "essai"
                    DateRecup = new DateTime(TodayDate.Year, TodayDate.Month, TodayDate.Day, 7, 0, 0, DateTimeKind.Local);
                    break;
                case false:
                    // Calculer la difference des jours entre la date debut d'affichage et la date fin d'affichage
                    if (DateDu.Equals(DateAu))
                    {
                        // Si la personne souhaite afficher qu'un jour pour le calendrier, afficher la semaine pour éviter d'afficher qu'une colonne
                        NbJours = 7;
                    }
                    else
                    {
                        NbJours = (DateAu.Value - DateDu.Value).Days;
                        NbJours = NbJours + 1;  // Car il compte pas la bonne quantité 
                        if (NbJours < 7) // Afficher au moins une semaine 
                            NbJours = 7;

                    }
                    // Ajouter l'heure (à 7h00) car sinon on va avoir un problème pour faire la comparaison lors de la récupération des "essai"
                    DateRecup = new DateTime(DateDu.Value.Year, DateDu.Value.Month, DateDu.Value.Day, 7, 0, 0, DateTimeKind.Local);
                    break;
            }

            SaveRecupDate = DateRecup;
            //saveForZoneDisp = DateRecup;
            #endregion

            #region Recueil des réservations par jour et par équipement par zone

            // Obtenir la liste des zones
            List<zone> zones = CalendResaDb.ListeZones();

            // Pour chaque zone, obtenir la liste des équipements avec leurs réservations
            foreach (var z in zones)
            {
                ListEquiVsResa = new List<EquipementVsResa>(); // initialiser à zéro!
                // Obtenir la liste des équipements pour la zone Z
                List<equipement> equipements = CalendResaDb.ListeEquipements(z.id);

                foreach (var equip in equipements)
                {
                    equipementVsResa = new EquipementVsResa();
                    // Reinitialiser pour chaque equipement
                    ListResEquipParjour = new List<ResasEquipParJour>();

                    #region Initialiser les infos de l'équipement

                    equipementVsResa.IdEquipement = equip.id;
                    equipementVsResa.NomEquipement = equip.nom;

                    #endregion
                    // Remettre la date à sa valeur d'origine
                    DateRecup = SaveRecupDate;
                    // For pour recupérer les réservations des N jours à partir du lundi
                    for (int i = 0; i < NbJours; i++)
                    {
                        // Obtenir l'emploi du temps du jour de la semaine i pour un équipement
                        ResasEquipParJour EquipResaJour = CalendResaDb.ResasEquipementParJour(equip.id, DateRecup);
                        // Ajouter les données calendrier pour l'équipement dans la liste
                        ListResEquipParjour.Add(EquipResaJour);
                        
                        DateRecup = DateRecup.AddDays(1);
                    }
                    equipementVsResa.ListResasDuAu = ListResEquipParjour;
                    // Ajouter dans la liste destiné aux zones
                    ListEquiVsResa.Add(equipementVsResa);
                }

                // Supprimer les anciens valeurs sauvegardés
                ListeDispoZonesDuAu = new List<OccupationZonesParJour>();
                // Vérifier si la zone est libre ou occupé pour chaque jour
                // Remettre la date à sa valeur d'origine sinon on a des dates faussées
                DateRecup = SaveRecupDate;
                for (int d = 0; d < NbJours; d++)
                {
                    occupation = new OccupationZonesParJour();
                    bool ZonOccupeMatin = false;
                    bool ZonOccupeAprem = false;
                    foreach (var equip in equipements)
                    {
                        ResasEquipParJour EquipResaJour = CalendResaDb.ResasEquipementParJour(equip.id, DateRecup);
                        // Vérifier la disponibilité de la zone (juste pour affichage) 
                        if (EquipResaJour.ListResasMatin.Count() > 0 || EquipResaJour.InfosIntervMatin.Count() > 0) 
                            // Vérifier pour les opérations de maintenance et les réservations            
                            ZonOccupeMatin = true;

                        if ((EquipResaJour.ListResasAprem.Count() > 0 || EquipResaJour.InfosIntervAprem.Count() > 0) && occupation.IsZoneOccupeAprem != true)
                            ZonOccupeAprem = true;
                    }

                    if (ZonOccupeMatin == true)
                        occupation.IsZoneOccupeMatin = true;
                    if (ZonOccupeAprem == true)
                        occupation.IsZoneOccupeAprem = true;

                    ListeDispoZonesDuAu.Add(occupation); // occupation de la zone par jour
                    DateRecup = DateRecup.AddDays(1);
                }

                resasZone = new ResasZone
                {
                    IdZone = z.id,
                    ListeDispoZonesDuAu = ListeDispoZonesDuAu,
                    ListEquipementVsResa = ListEquiVsResa,
                    NomZone = z.nom_zone
                };

                // Completer les infos sur chaque zone en ajoutant les réservations par équipement
                ListResasZone.Add(resasZone);
            }

            #endregion

            #region Initialisation des jours pour affichage(uniquement les jours pour les header table)
            DateRecup = SaveRecupDate;
            for (int i = 0; i < NbJours; i++)
            {
                JourCalendrier Jc = new JourCalendrier { JourPourAffichage = DateRecup, NomJour = DateRecup.ToString("dddd", dateTimeFormats) };
                ListCalendrierParZone.Add(Jc);
                DateRecup = DateRecup.AddDays(1);
            }

            #endregion

            return (ListCalendrierParZone, ListResasZone);
        }

        #endregion
    }
}