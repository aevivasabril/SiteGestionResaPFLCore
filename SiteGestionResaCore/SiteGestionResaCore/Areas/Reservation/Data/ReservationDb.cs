using Microsoft.AspNetCore.Identity;
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
        public reservation_projet CreationReservation(equipement Equip, essai Essai, DateTime dateDebut, DateTime dateFin)
        {
            //string sqlFormattedDateDebut = dateDebut.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //string sqlFormattedDateFin = dateFin.ToString("yyyy-MM-dd HH:mm:ss.fff");
            // Rajouter uniquement les ID's vers les autres tables (clé étrangere)
            reservation_projet resa = new reservation_projet() { equipementID = Equip.id, essaiID = Essai.id, date_debut = dateDebut, date_fin = dateFin };

            // Rajouter la clé étrangere (table equipement) vers 
            // Add this existing equipment to the new reservation_projet's "equipements" collection
            //resa.equipement = Equip; // ERREURRRRR: :( Cette ligne me créé un déuxième équipement dans la table équipement 
            //resa.equipementID = Equip.id;

            // Ajouter la réference vers l'essai
            //resa.essai = Essai;
            // Ajouter dans ma BDD "reservation_projet"
            context.reservation_projet.Add(resa);

            context.SaveChanges();

            // TODO: Temporaire
            /*var query = (from res in resaDB.reservation_projet
                         where (res.equipement.id == 163)
                         select res).ToArray();*/

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
            //string datResa = dateResa.ToShortDateString();
            // Requete vers la base de données pour obtenir tous les essais qui ont lieu ce jour 
            /*InfosEssai = (from resa in resaDB.reservation_projet
                         from essa in resaDB.essai
                         where resa.essaiID == essa.id &&
                         (Convert.ToDateTime(datResa).Date >= Convert.ToDateTime(resa.date_debut.ToShortDateString()).Date &&
                          Convert.ToDateTime(dateResa.ToShortTimeString()).Date <= Convert.ToDateTime(resa.date_fin.ToShortTimeString()).Date)
                          select essa).Distinct().ToArray(); // Je pense il faut mettre distinct pour récupérer chaque réservation unique*/

            //TODO: Affiner cette recherche car on aurait une liste enorme des essais
            //TODO: question pour christophe: Comment faire une recherche en regardant la date aussi??? 
            SubInfosEssai = (from resa in context.reservation_projet
                             from essa in context.essai
                             where resa.essaiID == essa.id
                             select essa).Distinct().ToArray();

            // Résupérer les essais où la date enquêté est bien dans la plage de déroulement
            foreach (var es in SubInfosEssai)
            {
                foreach (var resEs in es.reservation_projet)
                {
                    if ((dateResa.CompareTo(resEs.date_debut) >= 0) && // si dateResa est superieur à resEs.date_debut ou égal 
                        (dateResa.CompareTo(resEs.date_fin) <= 0))  // si dateResa est inferieur à resEs.date_fin ou égal
                    {
                        InfosEssai.Add(es);
                        break;
                    }
                }
            }


            foreach (var ess in InfosEssai)
            {
                switch (ess.confidentialite)
                {
                    case "Ouvert": // Dans ce cas: je regarde si mon équipement est concerné par cet essai et je bloque uniquement l'équipement

                        #region Confidentialité ouverte

                        Resas = ResaConfidentialiteOuverte(ess, IdEquipement, dateResa);

                        #endregion

                        break;
                    case "Restreint": // si "Restreint" il faut bloquer toute la zone, alors vérifier si les réservations sont dans la même zone que l'équipement et bloquer l'équipement 

                        #region Confidentialité "Restreint"

                        bool IsFirstSearchOnEssai = true;

                        #region Recherche des dates superieur et inferieur pour chaque essai

                        foreach (var resa in ess.reservation_projet)
                        {
                            if (resa.equipement.zoneID == EquipementPlanning.zoneID) // si l'équipement objet du "planning" est dans la zone concerné alors il faut le bloquer
                            {
                                if (EquipementPlanning.zoneID.Equals(EnumZonesPfl.HaloirAp7) || EquipementPlanning.zoneID.Equals(EnumZonesPfl.SalleAp5) ||
                                    EquipementPlanning.zoneID.Equals(EnumZonesPfl.SalleAp6) || EquipementPlanning.zoneID.Equals(EnumZonesPfl.SalleAp8) ||
                                    EquipementPlanning.zoneID.Equals(EnumZonesPfl.SalleAp9))
                                {
                                    // Pour ces zones alors faire comment on fait pour les essai du type "Ouvert" blocage uniquement des équipements
                                    Resas = ResaConfidentialiteOuverte(ess, IdEquipement, dateResa);
                                }
                                else // pour toutes les autres zones calculer la date seuil inferieur et superieur parmi toutes les réservations
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
                                        if (resa.date_debut.CompareTo(dateSeuilSup) >= 0)  // resa.date_fin >= dateSeuilSup)
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

                        if ((IsEquipInZone == true) && (dateResa.CompareTo(dateSeuilInf) >= 0) && (dateResa.CompareTo(dateSeuilSup) <= 0)) // si l'équipement est dans la zone
                        {
                            if (dateResa.CompareTo(dateSeuilInf) == 0) // début
                            {
                                // Regarder pour définir le créneau
                                if (dateSeuilInf.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                {
                                    Resas.InfosResaAprem.Add(ResaAGarder);
                                    //Resas.InfosResaMatin.Add(null); // Matin vide
                                }
                                else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                                {
                                    Resas.InfosResaMatin.Add(ResaAGarder);
                                    Resas.InfosResaAprem.Add(ResaAGarder);
                                }
                            }
                            else if (dateResa.CompareTo(dateSeuilSup) == 0) // fin
                            {
                                // Regarder pour définir le créneau
                                if (dateSeuilSup.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                                {
                                    Resas.InfosResaMatin.Add(ResaAGarder);
                                    //Resas.InfosResaAprem.Add(null); // Aprèm vide TODO: voir si on peut ajouter un element null!!
                                }
                                else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                                {
                                    Resas.InfosResaMatin.Add(ResaAGarder);
                                    Resas.InfosResaAprem.Add(ResaAGarder);
                                }
                            }
                            else
                            {
                                // Ajouter cette résa sur le créneau matin et aprèm 
                                Resas.InfosResaMatin.Add(ResaAGarder);
                                Resas.InfosResaAprem.Add(ResaAGarder);
                            }
                            IsEquipInZone = false;
                        }

                        #endregion
                        #endregion

                        break;
                    case "Confidentiel": // Blocage de toute la plateforme sauf pour les salles alimentaires (5 zones)

                        #region Confidentialité "confidentiel" 

                        if (EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.HaloirAp7) || EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp5) ||
                            EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp6) || EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp8) ||
                            EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp9))
                        {
                            // Pour ces zones alors faire comment on fait pour les essai du type "Ouvert" blocage uniquement des équipements
                            Resas = ResaConfidentialiteOuverte(ess, IdEquipement, dateResa);
                        }
                        else
                        {
                            if ((dateResa.CompareTo(ess.date_inf_confidentiel.Value) >= 0) && (dateResa.CompareTo(ess.date_sup_confidentiel.Value) <= 0))
                            {
                                // Créer une réservation uniquement pour avoir l'accès à l'essai (A modifier)
                                ResaAGarder = new reservation_projet { equipementID = IdEquipement, essaiID = ess.id, date_debut = ess.date_inf_confidentiel.Value, date_fin = ess.date_sup_confidentiel.Value, essai = ess };
                                if (dateResa.ToShortDateString().CompareTo(ess.date_inf_confidentiel.Value.ToShortDateString()) == 0) // début
                                {
                                    // Regarder pour définir le créneau
                                    if (ess.date_inf_confidentiel.Value.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                    {
                                        Resas.InfosResaAprem.Add(ResaAGarder);
                                        //Resas.InfosResaMatin.Add(null); // Matin vide
                                    }
                                    else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                                    {
                                        Resas.InfosResaMatin.Add(ResaAGarder);
                                        Resas.InfosResaAprem.Add(ResaAGarder);
                                    }
                                }
                                else if (dateResa.ToShortDateString().CompareTo(ess.date_sup_confidentiel.Value.ToShortDateString()) == 0) // fin
                                {
                                    // Regarder pour définir le créneau
                                    if (ess.date_sup_confidentiel.Value.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                                    {
                                        Resas.InfosResaMatin.Add(ResaAGarder);
                                        //Resas.InfosResaAprem.Add(null); // Aprèm vide TODO: voir si on peut ajouter un element null!!
                                    }
                                    else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                                    {
                                        Resas.InfosResaMatin.Add(ResaAGarder);
                                        Resas.InfosResaAprem.Add(ResaAGarder);
                                    }
                                }
                                else
                                {
                                    // Ajouter cette résa sur le créneau matin et aprèm 
                                    Resas.InfosResaMatin.Add(ResaAGarder);
                                    Resas.InfosResaAprem.Add(ResaAGarder);
                                }
                            }
                        }
                        #endregion
                        break;
                }
            }

            #region Gestion nom du jour et couleurs pour l'affichage

            // Obtenir le nom du jour 
            Resas.JourResa = dateResa; // enregistrer la date en question
                                       // Traduire le nom du jour en cours de l'anglais au Français
            dateTimeFormats = new CultureInfo("fr-FR").DateTimeFormat;
            Resas.NomJour = dateResa.ToString("dddd", dateTimeFormats);
            // TODO: Requete vers la base de données pour obtenir toutes les réservations du type "maintenance" 
            // TODO: Requete vers la base de données pour obtenir toutes les réservations du type "métrologie" 

            if (Resas.NomJour != "samedi" && Resas.NomJour != "dimanche")
            {
                // CODE COULEUR DISPO SUR: https://encycolorpedia.fr/
                // Definir les couleurs de fond pour indiquer si le créneau est occupé ou pas
                if (Resas.InfosResaMatin.Count() > 0) // si au moins une réservation le matin alors matinée occupée
                    Resas.CouleurFondMatin = "#fdc0be"; // rouge
                else
                    Resas.CouleurFondMatin = "#a2d9d4"; // matin dispo (Vert)

                if (Resas.InfosResaAprem.Count() > 0) // si au moins une réservation l'aprèm alors aprèm occupée
                    Resas.CouleurFondAprem = "#fdc0be"; // rouge
                else
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



        #region méthodes externes

        public ReservationsJour ResaConfidentialiteOuverte(essai ess, int IdEquipement, DateTime dateResa)
        {
            ReservationsJour Resas = new ReservationsJour();

            foreach (var resa in ess.reservation_projet)
            {
                if ((resa.equipementID == IdEquipement) && ((dateResa.CompareTo(resa.date_debut) >= 0) && (dateResa.CompareTo(resa.date_fin) <= 0)))
                //((Convert.ToDateTime(dateResa.ToShortDateString()).Date >= Convert.ToDateTime(resa.date_debut.ToShortDateString()).Date) &&
                //(Convert.ToDateTime(dateResa.ToShortDateString()).Date <= (Convert.ToDateTime(resa.date_fin.ToShortDateString()).Date)))) // Si l'équipement à afficher est impliqué dans l'essai
                {
                    if (dateResa.ToShortDateString().CompareTo(resa.date_debut.ToShortDateString()) == 0) // si dateResa égal à resa.date_debut
                    {
                        // Regarder pour définir le créneau
                        if (resa.date_debut.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                        {
                            Resas.InfosResaAprem.Add(resa);
                            //Resas.InfosResaMatin.Add(null); // Matin vide
                        }
                        else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                        {
                            Resas.InfosResaMatin.Add(resa);
                            Resas.InfosResaAprem.Add(resa);
                        }
                    }
                    else if (dateResa.ToShortDateString().CompareTo(resa.date_fin.ToShortDateString()) == 0) // si dateResa égal à resa.date_fin
                    {
                        // Regarder pour définir le créneau
                        if (resa.date_fin.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                        {
                            Resas.InfosResaMatin.Add(resa);
                            //Resas.InfosResaAprem.Add(null); // Aprèm vide TODO: voir si on peut ajouter un element null!!
                        }
                        else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                        {
                            Resas.InfosResaMatin.Add(resa);
                            Resas.InfosResaAprem.Add(resa);
                        }
                    }
                    else // date à l'intérieur du seuil de réservation
                    {
                        // Ajouter cette résa sur le créneau matin et aprèm 
                        Resas.InfosResaMatin.Add(resa);
                        Resas.InfosResaAprem.Add(resa);
                    }

                }
            }

            return Resas;
        }

        #endregion

        
    }
}
