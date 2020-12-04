﻿using Microsoft.AspNetCore.Identity;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    public class ReservationDb: IReservationDb
    {

        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private GestionResaContext context;
        private readonly UserManager<utilisateur> userManager;
        //private readonly ILogger<FormulaireResaDb> logger;

        public ReservationDb(
            GestionResaContext projEssaiDb,
            UserManager<utilisateur> userManager/*,
            ILogger<FormulaireResaDb> logger*/)
        {
            this.context = projEssaiDb;
            //this.userManager = userManager;
            //this.logger = logger;
        }

        // TODO: à vérifier
        public reservation_projet CreationReservation(int EquipId, essai Essai, DateTime dateDebut, DateTime dateFin)
        {
            // Rajouter uniquement les ID's vers les autres tables (clé étrangere)
            reservation_projet resa = new reservation_projet() { equipementID = EquipId, essaiID = Essai.id, date_debut = dateDebut, date_fin = dateFin };

            // Ajouter dans ma BDD "reservation_projet"
            context.reservation_projet.Add(resa);

            context.SaveChanges();

            return resa;
        }

        // VOIR si cette méthode marche
        public ReservationsJour ObtenirReservationsJourEssai(DateTime dateResa, int IdEquipement)
        {
            // Variables
            ReservationsJour Resas = new ReservationsJour();
            essai[] SubInfosEssai = new essai[] { };
            List<essai> InfosEssai = new List<essai>();
            DateTimeFormatInfo dateTimeFormats = null;
            DateTime dateSeuilInf = new DateTime();                                                 // RESTREINT: Date à comparer sur chaque réservation pour trouver le seuil inferieur
            DateTime dateSeuilSup = new DateTime();                                                 // RESTREINT: Date à comparer sur chaque réservation pour trouver le seuil supérieur
            reservation_projet ResaAGarder = new reservation_projet();                              // On garde une des réservations de côté (peu importe laquelle car on a juste besoin d'accèder aux infos "essai")
            bool IsEquipInZone = false;
            equipement EquipementPlanning = context.equipement.First(x => x.id == IdEquipement);     // Equipement à enqueter

            // CORRECTION: initialisation des dateTime pour trouver les réservations se chevauchant (4 dates differentes)
            DateTime DatEnqDebMatin = dateResa; // date debut matin
            DateTime DatEnqDebAprem = new DateTime(dateResa.Year, dateResa.Month, dateResa.Day, 13, 0, 0, DateTimeKind.Local); // date début aprèm
            DateTime DatEnqFinMatin = new DateTime(dateResa.Year, dateResa.Month, dateResa.Day, 12, 0, 0, DateTimeKind.Local); // date fin matin
            DateTime DatEnqFinAprem = new DateTime(dateResa.Year, dateResa.Month, dateResa.Day, 18, 0, 0, DateTimeKind.Local); // date fin aprèm          

            // Si jour égal à samedi ou dimanche pas besoin de appliquer toute la méthode de recherche!
            // Traduire le nom du jour en cours de l'anglais au Français
            dateTimeFormats = new CultureInfo("fr-FR").DateTimeFormat;
            string jourName = dateResa.ToString("dddd", dateTimeFormats);

            if (jourName == "samedi" || jourName == "dimanche")
                goto ENDT;
                
                //TODO: question pour christophe: Comment faire une recherche en regardant la date aussi??? 
                SubInfosEssai = (from resa in context.reservation_projet
                             from essa in context.essai
                             where resa.essaiID == essa.id && 
                             (essa.status_essai == EnumStatusEssai.Validate.ToString() || 
                             essa.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                             select essa).Distinct().ToArray();

            // Résupérer les essais où la date enquêté est bien dans la plage de déroulement
            foreach (var es in SubInfosEssai)
            {
                var res = context.reservation_projet.Where(r => r.essaiID == es.id).ToList();
                foreach (var resEs in res)
                {
                    if( (DatEnqDebMatin >= resEs.date_debut || DatEnqDebAprem >= resEs.date_debut) &&
                        (DatEnqFinMatin <= resEs.date_fin || DatEnqFinAprem <= resEs.date_fin) )
                    {
                        InfosEssai.Add(es);
                        break;
                    }
                }
            }


            foreach (var ess in InfosEssai)
            {
                // Obtenir le projet pour cet essai
                var proj = context.projet.First(p => p.id == ess.projetID);
                // informations sur l'essai + projet qui seront affichés sur le calendrier
                ReservationInfos resaInfo = new ReservationInfos { confidentialite = ess.confidentialite, mailRespProjet = proj.mailRespProjet,
                                    num_projet = proj.num_projet , titre_projet = proj.titre_projet, StatusEssai = ess.status_essai, numEssai = ess.id};
                
                switch (ess.confidentialite)
                {
                    case "Ouvert": // Dans ce cas: je regarde si mon équipement est concerné par cet essai et je bloque uniquement l'équipement

                        #region Confidentialité ouverte

                        Resas = ResaConfidentialiteOuverte(ess, resaInfo, IdEquipement, dateResa);

                        #endregion

                        break;
                    case "Restreint": // si "Restreint" il faut bloquer toute la zone, alors vérifier si les réservations sont dans la même zone que l'équipement et bloquer l'équipement 

                        #region Confidentialité "Restreint"

                        bool IsFirstSearchOnEssai = true;

                        #region Recherche des dates superieur et inferieur pour chaque essai

                        foreach (var resa in context.reservation_projet.Where(r => r.essaiID == ess.id))
                        {
                            if (context.equipement.Where(e=>e.id == resa.equipementID).First().zoneID.Value == EquipementPlanning.zoneID) // si l'équipement objet du "planning" est dans la zone concerné
                            {
                                // Cas 1: l'équipement est dans une des zones dont le conflit ne fait pas partie
                                if (EquipementPlanning.zoneID.Equals(EnumZonesPfl.HaloirAp7) || EquipementPlanning.zoneID.Equals(EnumZonesPfl.SalleAp5) ||
                                    EquipementPlanning.zoneID.Equals(EnumZonesPfl.SalleAp6) || EquipementPlanning.zoneID.Equals(EnumZonesPfl.SalleAp8) ||
                                    EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp9) || EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.EquipMobiles))
                                {
                                    // Pour ces zones alors faire comment on fait pour les essai du type "Ouvert" blocage uniquement des équipements
                                    Resas = ResaConfidentialiteOuverte(ess, resaInfo, IdEquipement, dateResa);
                                }
                                else // Cas 2: pour toutes les autres zones, calculer la date seuil inferieur et superieur parmi toutes les réservations(blocage de la zone)
                                {
                                    if (IsFirstSearchOnEssai == true) // Executer que lors de la premiere réservation de la liste 
                                    {
                                        IsFirstSearchOnEssai = false;
                                        dateSeuilInf = resa.date_debut;
                                        dateSeuilSup = resa.date_fin;
                                    }
                                    else
                                    {
                                        // Recherche des dates superieur et inferieur sur toutes les réservations
                                        if (resa.date_debut.CompareTo(dateSeuilInf) <= 0) // resa.date_debut <= dateSeuilInf)
                                        {
                                            dateSeuilInf = resa.date_debut;
                                        }
                                        if (resa.date_fin.CompareTo(dateSeuilSup) >= 0)  // resa.date_fin >= dateSeuilSup)
                                        {
                                            dateSeuilSup = resa.date_fin;
                                        }
                                    }
                                    ResaAGarder = resa;
                                    IsEquipInZone = true;
                                }
                            }
                        }

                        #endregion

                        #region Bloquer l'équipement si la dateResa est dans les dates seuils rétrouvées

                        if ( (IsEquipInZone == true) && ( DateTime.Parse(dateResa.ToShortDateString()) >= DateTime.Parse(dateSeuilInf.ToShortDateString()) ) 
                            && ( DateTime.Parse(dateResa.ToShortDateString()) <= DateTime.Parse(dateSeuilSup.ToShortDateString()) ) ) // si l'équipement est dans la zone et que la date enqueté est dans le seuil
                        {
                            if ( DateTime.Parse(dateResa.ToShortDateString()) == DateTime.Parse(dateSeuilInf.ToShortDateString()) ) // début
                            {
                                // Regarder pour définir le créneau
                                if (dateSeuilInf.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                {
                                    Resas.InfosResaAprem.Add(resaInfo);
                                    //Resas.InfosResaMatin.Add(null); // Matin vide
                                }
                                else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                                {
                                    Resas.InfosResaMatin.Add(resaInfo);
                                    Resas.InfosResaAprem.Add(resaInfo);
                                }
                            }
                            else if ( DateTime.Parse(dateResa.ToShortDateString()) == DateTime.Parse(dateSeuilSup.ToShortDateString()) ) // fin
                            {
                                // Regarder pour définir le créneau
                                if (dateSeuilSup.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                                {
                                    Resas.InfosResaMatin.Add(resaInfo);
                                    //Resas.InfosResaAprem.Add(null); // Aprèm vide TODO: voir si on peut ajouter un element null!!
                                }
                                else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                                {
                                    Resas.InfosResaMatin.Add(resaInfo);
                                    Resas.InfosResaAprem.Add(resaInfo);
                                }
                            }
                            else
                            {
                                // Ajouter cette résa sur le créneau matin et aprèm 
                                Resas.InfosResaMatin.Add(resaInfo);
                                Resas.InfosResaAprem.Add(resaInfo);
                            }
                            IsEquipInZone = false;
                        }

                        #endregion
                        #endregion

                        break;
                    case "Confidentiel": // Blocage de toute la plateforme sauf pour les salles alimentaires (5 zones) et la zone equipements mobiles

                        #region Confidentialité "confidentiel" 

                        if (EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.HaloirAp7) || EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp5) ||
                            EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp6) || EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp8) ||
                            EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp9)) //|| EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.EquipMobiles)) (TODO:  la zone equipements mobiles devrait être bloqué?)
                        {
                            // Pour ces zones alors faire comment on fait pour les essai du type "Ouvert" blocage uniquement des équipements
                            Resas = ResaConfidentialiteOuverte(ess, resaInfo, IdEquipement, dateResa);
                        }
                        else
                        {
                            //bool t = (DateTime.Parse(dateResa.ToShortDateString()) >= DateTime.Parse(ess.date_sup_confidentiel.Value.ToShortDateString()));
                            if ((DateTime.Parse(dateResa.ToShortDateString()) >= DateTime.Parse(ess.date_inf_confidentiel.Value.ToShortDateString()))   //Bonne manière de comparer les dates converties en shortstring!
                                && (DateTime.Parse(dateResa.ToShortDateString()) <= DateTime.Parse(ess.date_sup_confidentiel.Value.ToShortDateString())))
                            {
                                // Créer une réservation uniquement pour avoir l'accès à l'essai (A modifier)
                                ResaAGarder = new reservation_projet { equipementID = IdEquipement, essaiID = ess.id, date_debut = ess.date_inf_confidentiel.Value, date_fin = ess.date_sup_confidentiel.Value, essai = ess };
                                
                                if ( DateTime.Parse(dateResa.ToShortDateString()) == DateTime.Parse(ess.date_inf_confidentiel.Value.ToShortDateString()) ) // début
                                {
                                    // Regarder pour définir le créneau
                                    if (ess.date_inf_confidentiel.Value.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                    {
                                        Resas.InfosResaAprem.Add(resaInfo);
                                        //Resas.InfosResaMatin.Add(null); // Matin vide
                                    }
                                    else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                                    {
                                        Resas.InfosResaMatin.Add(resaInfo);
                                        Resas.InfosResaAprem.Add(resaInfo);
                                    }
                                }
                                else if ( DateTime.Parse(dateResa.ToShortDateString()) == DateTime.Parse(ess.date_sup_confidentiel.Value.ToShortDateString()) ) // fin
                                {
                                    // Regarder pour définir le créneau
                                    if (ess.date_sup_confidentiel.Value.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                                    {
                                        Resas.InfosResaMatin.Add(resaInfo);
                                        //Resas.InfosResaAprem.Add(null); // Aprèm vide TODO: voir si on peut ajouter un element null!!
                                    }
                                    else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                                    {
                                        Resas.InfosResaMatin.Add(resaInfo);
                                        Resas.InfosResaAprem.Add(resaInfo);
                                    }
                                }
                                else
                                {
                                    // Ajouter cette résa sur le créneau matin et aprèm 
                                    Resas.InfosResaMatin.Add(resaInfo);
                                    Resas.InfosResaAprem.Add(resaInfo);
                                }
                            }
                        }
                        #endregion
                        break;
                }
            }

            #region Gestion nom du jour et couleurs pour l'affichage

        ENDT:
            // Obtenir le nom du jour 
            Resas.JourResa = dateResa; // enregistrer la date en question
            Resas.NomJour = dateResa.ToString("dddd", dateTimeFormats); // Reecrire le nom du jour car lors de l'appel de la méthode ResaConfidentialiteOuverte()
                                                                        // Resas est réinitialisé!
            // TODO: Requete vers la base de données pour obtenir toutes les réservations du type "maintenance" 
            // TODO: Requete vers la base de données pour obtenir toutes les réservations du type "métrologie" 

            if (Resas.NomJour != "samedi" && Resas.NomJour != "dimanche")
            {
                // si plus d'une réservation à ce jour alors conflit entre 2 résas "Restreint"
                if(Resas.InfosResaMatin.Count() > 1) // réservations restreint
                    Resas.CouleurFondMatin = "#ffd191"; // Indiquer un chevauchement des créneaux réservation (orange)

                if(Resas.InfosResaAprem.Count() > 1)
                    Resas.CouleurFondAprem = "#ffd191"; // Indiquer un chevauchement des créneaux réservation (orange)

                if(Resas.InfosResaMatin.Count() == 1)
                {
                    if(Resas.InfosResaMatin[0].StatusEssai == EnumStatusEssai.Validate.ToString())
                        Resas.CouleurFondMatin = "#fdc0be"; // rouge (validée et occupée)
                    else if(Resas.InfosResaMatin[0].StatusEssai == EnumStatusEssai.WaitingValidation.ToString())
                        Resas.CouleurFondMatin = "#fbeed9";  // Couleur beige pour indiquer que la réservation est en attente
                }

                // si pas de chevauchement des résas alors vérifier le status projet
                if (Resas.InfosResaAprem.Count() == 1)
                {
                    if (Resas.InfosResaAprem[0].StatusEssai == EnumStatusEssai.Validate.ToString())
                        Resas.CouleurFondAprem = "#fdc0be"; // rouge (validée et occupée)
                    else if (Resas.InfosResaAprem[0].StatusEssai == EnumStatusEssai.WaitingValidation.ToString())
                        Resas.CouleurFondAprem = "#fbeed9";  // Couleur beige pour indiquer que la réservation est en attente
                }

              
                // CODE COULEUR DISPO SUR: https://encycolorpedia.fr/
                // Definir les couleurs de fond pour indiquer si le créneau est occupé ou pas
                if (Resas.InfosResaMatin.Count() == 0) // si au moins une réservation le matin alors matinée occupée
                    Resas.CouleurFondMatin = "#a2d9d4"; // matin dispo (Vert)
                if (Resas.InfosResaAprem.Count() == 0) // si au moins une réservation l'aprèm alors aprèm occupée
                    Resas.CouleurFondAprem = "#a2d9d4"; // Aprèm libre (Vert)
            }
            else // si jour samedi ou dimanche alors mettre en fond gris
            {
                Resas.CouleurFondMatin = "silver";
                Resas.CouleurFondAprem = "silver";
            }

            #endregion

            return Resas;
        }


        public bool VerifDisponibilitéEquipement(DateTime dateDebut, DateTime dateFin, int idEquipement)
        {
            #region Vérification sur les réservations du type "Ouvert" où il faut juste vérifier par l'ID equipement

            bool estOuvertDisponible        = false;
            bool estRestreintDispo          = false;
            bool estConfidentielDispo       = false;

            // TODO: Vérifier cette requete

            // requete complète pour trouver les réservations où leur essai est "ouvert", l'id equipement est égal a idEquipement et la date souhaitée pour réservation est déjà prise
            var resasOuv = (from essai in context.essai
                         from resa in context.reservation_projet
                         where essai.confidentialite == EnumConfidentialite.Ouvert.ToString() && essai.id == resa.essaiID && resa.equipementID == idEquipement
                         && ( ((dateDebut >= resa.date_debut) || dateFin >= resa.date_debut) 
                         && ((dateDebut <= resa.date_fin) || dateFin <= resa.date_fin))
                         select resa).Distinct().ToList();

            if (resasOuv.Count() == 0) // aucun equipement réservé à ces dates! 
                estOuvertDisponible = true;

            #endregion

            #region Vérification sur les réservations "Restreint" 

            // Récupérer l'id zone pour l'équipement enquêté
            var zon = (from equip in context.equipement
                       where equip.id == idEquipement
                       select equip.zoneID.Value).First();

            // requete pour recuperer les reservation dont il s'agit d'un essai "Restreint" pour cet équipement et où les dates sont déjà réservés
            var resasRest = (from essai in context.essai
                         from resa in context.reservation_projet
                         from equip in context.equipement
                         where essai.confidentialite == EnumConfidentialite.Restreint.ToString() && resa.equipementID == idEquipement
                         && ( ( dateDebut >= resa.date_debut || dateFin >= resa.date_debut) 
                         && ( dateDebut <= resa.date_fin || dateFin <= resa.date_fin ))
                         select resa).Distinct().ToList();

            // TODO: lors de la validation des réservations mettre un conflit si une des 2 résas sont "RESTREINT" et que les équipement
            // sont differents mais dans la même zone (réservations validées ou à valider)
            // Pas de blocage pour réserver un autre équipement dans cette zone! jusqu'à la validation 
            if (resasRest.Count() == 0) // si aucune réservation directe sur l"equipement alors on peut réserver
                estRestreintDispo = true;

            #endregion

            #region Vérification sur les réservations "Confidentiel"

            // TODO: pas de blocage pour les zones HaloirAp7, SalleAp5, SalleAp6, SalleAp8, SalleAp9

            int ApCinq = Convert.ToInt32(EnumZonesPfl.SalleAp5);
            int ApSix = Convert.ToInt32(EnumZonesPfl.SalleAp6);
            int ApSept = Convert.ToInt32(EnumZonesPfl.HaloirAp7);
            int ApHuit = Convert.ToInt32(EnumZonesPfl.SalleAp8);
            int ApNeuf = Convert.ToInt32(EnumZonesPfl.SalleAp9);

            if(context.equipement.First(e=>e.id == idEquipement).zoneID.Value == ApCinq ||
                context.equipement.First(e => e.id == idEquipement).zoneID.Value == ApSix ||
                context.equipement.First(e => e.id == idEquipement).zoneID.Value == ApSept ||
                context.equipement.First(e => e.id == idEquipement).zoneID.Value == ApHuit ||
                context.equipement.First(e => e.id == idEquipement).zoneID.Value == ApNeuf)
            {
                // requete pour trouver les essais "confidentiels" avec les mêmes dates
                var essaiConf = (from essai in context.essai
                                 from equip in context.equipement
                                 from reser in context.reservation_projet
                                 where (essai.confidentialite == EnumConfidentialite.Confidentiel.ToString()
                                 && (reser.equipementID == idEquipement)
                                 && (((dateDebut >= essai.date_inf_confidentiel) || dateFin >= essai.date_inf_confidentiel)
                                 && ((dateDebut <= essai.date_sup_confidentiel) || dateFin <= essai.date_sup_confidentiel)))
                                 select essai).Distinct().ToList();

                if (essaiConf.Count() == 0) // si aucune réservation "confidentiel sur ces dates et hors les zones alimentaires 
                    estConfidentielDispo = true;
            }
            else
            {
                // requete pour trouver les essais "confidentiels" avec les mêmes dates ( sauf pour les zones alimentaires )
                var essaiConfZonesAlim = (from essai in context.essai
                                          from equip in context.equipement
                                          from reser in context.reservation_projet
                                          where (essai.confidentialite == EnumConfidentialite.Confidentiel.ToString() 
                                          && (equip.zoneID != ApCinq && equip.zoneID != ApSix && equip.zoneID != ApSept
                                          && equip.zoneID != ApHuit && equip.zoneID != ApNeuf)
                                          && (((dateDebut >= essai.date_inf_confidentiel) || dateFin >= essai.date_inf_confidentiel)
                                          && ((dateDebut <= essai.date_sup_confidentiel) || dateFin <= essai.date_sup_confidentiel)))
                                          select essai).Distinct().ToList();


                if (essaiConfZonesAlim.Count() == 0) // si aucune réservation "confidentiel sur ces dates et hors les zones alimentaires 
                    estConfidentielDispo = true;
            }
            

            #endregion

            return (estOuvertDisponible && estRestreintDispo && estConfidentielDispo);
        }


        public string ObtenirNomEquipement(int id)
        {
            return context.equipement.First(e => e.id == id).nom;
        }

        #region méthodes externes

        public ReservationsJour ResaConfidentialiteOuverte(essai ess, ReservationInfos resaInfo, int IdEquipement, DateTime dateResa)
        {
            ReservationsJour Resas = new ReservationsJour();

            foreach (var resa in context.reservation_projet.Where(r => r.essaiID == ess.id))
            {
                if ( (resa.equipementID == IdEquipement) && ( DateTime.Parse(dateResa.ToShortDateString()) >= DateTime.Parse(resa.date_debut.ToShortDateString()) )
                    && ( DateTime.Parse(dateResa.ToShortDateString()) <= DateTime.Parse(resa.date_fin.ToShortDateString()) ) )
                //((Convert.ToDateTime(dateResa.ToShortDateString()).Date >= Convert.ToDateTime(resa.date_debut.ToShortDateString()).Date) &&
                //(Convert.ToDateTime(dateResa.ToShortDateString()).Date <= (Convert.ToDateTime(resa.date_fin.ToShortDateString()).Date)))) // Si l'équipement à afficher est impliqué dans l'essai
                {
                    if (DateTime.Parse(dateResa.ToShortDateString()) == DateTime.Parse(resa.date_debut.ToShortDateString())) // si dateResa égal à resa.date_debut
                    {
                        // Regarder pour définir le créneau
                        if (resa.date_debut.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                        {
                            Resas.InfosResaAprem.Add(resaInfo);
                            //Resas.InfosResaMatin.Add(null); // Matin vide
                        }
                        else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                        {
                            Resas.InfosResaMatin.Add(resaInfo);
                            Resas.InfosResaAprem.Add(resaInfo);
                        }
                    }
                    else if (DateTime.Parse(dateResa.ToShortDateString()) == DateTime.Parse(resa.date_fin.ToShortDateString())) // si dateResa égal à resa.date_fin
                    {
                        // Regarder pour définir le créneau
                        if (resa.date_fin.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                        {
                            Resas.InfosResaMatin.Add(resaInfo);
                            //Resas.InfosResaAprem.Add(null); // Aprèm vide TODO: voir si on peut ajouter un element null!!
                        }
                        else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                        {
                            Resas.InfosResaMatin.Add(resaInfo);
                            Resas.InfosResaAprem.Add(resaInfo);
                        }
                    }
                    else // date à l'intérieur du seuil de réservation
                    {
                        // Ajouter cette résa sur le créneau matin et aprèm 
                        Resas.InfosResaMatin.Add(resaInfo);
                        Resas.InfosResaAprem.Add(resaInfo);
                    }

                }
            }

            return Resas;
        }

        #endregion

        
    }
}
