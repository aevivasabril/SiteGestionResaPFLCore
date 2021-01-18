using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Areas.Reservation.Data;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    public class CalendResaDb: ICalendResaDb
    {
        /// <summary>
        /// accès à la base de données PflStloResaTest
        /// </summary>
        private readonly GestionResaContext resaDB;
        private readonly ILogger<CalendResaDb> logger;

        public CalendResaDb(
            GestionResaContext resaDB,
            ILogger<CalendResaDb> logger)
        {
            this.resaDB = resaDB;
            this.logger = logger;
        }

        /*public List<InfosCalenZone> ObtenirZonesVsEquipements()
        {
            List<InfosCalenZone> listInfos = new List<InfosCalenZone>();
            InfosCalenZone infosTemp = new InfosCalenZone();
            List<string> NomsEquip = new List<string>();

            var zones = resaDB.zone.ToList();

            foreach(var zo in zones)
            {
                infosTemp = new InfosCalenZone()
                {
                    IdZone = zo.id,
                    NomZone = zo.nom_zone,
                    ListEquipements = resaDB.equipement.Where(e => e.zoneID == zo.id).Select(e => e.nom).ToList()
                };
                listInfos.Add(infosTemp);
            }   
            return listInfos;
        }*/

        public List<zone> ListeZones()
        {
            return resaDB.zone.ToList();
        }

        public List<equipement> ListeEquipements(int ZoneID)
        {
            return resaDB.equipement.Where(e=>e.zoneID == ZoneID).Distinct().ToList();
        }

        /*public InfosCalenZone ResasEquipementsParZone(DateTime DateRecup, int NbJours, int ZoneId)
        {
            InfosCalenZone infos = new InfosCalenZone();

            // Obtenir la liste des équipements disponibles dans la zone
            List<equipement> equipements = resaDB.equipement.Where(e => e.zoneID == ZoneId).Distinct().ToList();

            // pour chaque équipement obtenir les réservations sur les dates souhaitées
            foreach(var equip in equipements)
            {

                infos = new InfosCalenZone 
                { 
                    IdZone = ZoneId, NomZone = resaDB.zone.Where(z => z.id == ZoneId).Select(z => z.nom_zone).ToString() 
                };

            }

            return infos;
        }*/

        public ResasEquipParJour ResasEquipementParJour(int IdEquipement, DateTime DateRecup)
        {
            ResasEquipParJour resasEquip = new ResasEquipParJour();
            DateTimeFormatInfo dateTimeFormats = null;
            DateTime dateSeuilInf = new DateTime();                                                 // RESTREINT: Date à comparer sur chaque réservation pour trouver le seuil inferieur
            DateTime dateSeuilSup = new DateTime();
            reservation_projet ResaAGarder = new reservation_projet();                              // On garde une des réservations de côté (peu importe laquelle car on a juste besoin d'accèder aux infos "essai")
            bool IsEquipInZone = false;

            DateTime JourCalendrier;
            string NomJour;

            essai[] SubInfosEssai = new essai[] { };
            List<essai> InfosEssai = new List<essai>();
            equipement Equipement = resaDB.equipement.First(x => x.id == IdEquipement);     // Equipement à enqueter

            // CORRECTION: initialisation des dateTime pour trouver les réservations se chevauchant (4 dates differentes)
            DateTime DatEnqDebMatin = DateRecup; // date debut matin
            DateTime DatEnqDebAprem = new DateTime(DateRecup.Year, DateRecup.Month, DateRecup.Day, 13, 0, 0, DateTimeKind.Local); // date début aprèm
            DateTime DatEnqFinMatin = new DateTime(DateRecup.Year, DateRecup.Month, DateRecup.Day, 12, 0, 0, DateTimeKind.Local); // date fin matin
            DateTime DatEnqFinAprem = new DateTime(DateRecup.Year, DateRecup.Month, DateRecup.Day, 18, 0, 0, DateTimeKind.Local); // date fin aprèm 

            // Si jour égal à samedi ou dimanche pas besoin de appliquer toute la méthode de recherche!
            // Traduire le nom du jour en cours de l'anglais au Français
            dateTimeFormats = new CultureInfo("fr-FR").DateTimeFormat;
            string jourName = DateRecup.ToString("dddd", dateTimeFormats);

            if (jourName == "samedi" || jourName == "dimanche")
                goto ENDT;

            // Récupérer toutes les réservations validés ou en attente valid 
            SubInfosEssai = (from resa in resaDB.reservation_projet
                             from essa in resaDB.essai
                             where resa.essaiID == essa.id &&
                             (essa.status_essai == EnumStatusEssai.Validate.ToString() ||
                             essa.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                             select essa).Distinct().ToArray();

            // Récupérer les essais où la date enquêté est bien dans la plage de déroulement
            foreach (var es in SubInfosEssai)
            {
                var res = resaDB.reservation_projet.Where(r => r.essaiID == es.id).ToList();
                foreach (var resEs in res)
                {
                    if ((DatEnqDebMatin >= resEs.date_debut || DatEnqDebAprem >= resEs.date_debut) &&
                        (DatEnqFinMatin <= resEs.date_fin || DatEnqFinAprem <= resEs.date_fin))
                    {
                        InfosEssai.Add(es);
                        break;
                    }
                }
            }

            foreach (var ess in InfosEssai)
            {
                InfosAffichageResa infosResa = new InfosAffichageResa
                {
                    IdEssai = ess.id,
                    StatusEssai = ess.status_essai,
                    Confidentialite = ess.confidentialite
                };

                switch (ess.confidentialite)
                {
                    case "Ouvert": // Dans ce cas: je regarde si mon équipement est concerné par cet essai et je bloque uniquement l'équipement

                        #region Confidentialité ouverte

                        resasEquip = ResaConfidentialiteOuverte(ess, infosResa, IdEquipement, DateRecup);

                        #endregion

                        break;
                    case "Restreint": // si "Restreint" il faut bloquer toute la zone, alors vérifier si les réservations sont dans la même zone que l'équipement et bloquer l'équipement 

                        #region Confidentialité "Restreint"

                        bool IsFirstSearchOnEssai = true;

                        #region Recherche des dates superieur et inferieur pour chaque essai

                        foreach (var resa in resaDB.reservation_projet.Where(r => r.essaiID == ess.id))
                        {
                            if (resaDB.equipement.Where(e => e.id == resa.equipementID).First().zoneID.Value == Equipement.zoneID) // si l'équipement objet du "planning" est dans la zone concerné
                            {
                                // Cas 1: l'équipement est dans une des zones dont le conflit ne fait pas partie
                                if (Equipement.zoneID.Equals(EnumZonesPfl.HaloirAp7) || Equipement.zoneID.Equals(EnumZonesPfl.SalleAp5) ||
                                    Equipement.zoneID.Equals(EnumZonesPfl.SalleAp6) || Equipement.zoneID.Equals(EnumZonesPfl.SalleAp8) ||
                                    Equipement.zoneID.Equals((int)EnumZonesPfl.SalleAp9) || Equipement.zoneID.Equals((int)EnumZonesPfl.EquipMobiles))
                                {
                                    // Pour ces zones alors faire comment on fait pour les essai du type "Ouvert" blocage uniquement des équipements
                                    resasEquip = ResaConfidentialiteOuverte(ess, infosResa, IdEquipement, DateRecup);
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

                        if ((IsEquipInZone == true) && (DateTime.Parse(DateRecup.ToShortDateString()) >= DateTime.Parse(dateSeuilInf.ToShortDateString()))
                            && (DateTime.Parse(DateRecup.ToShortDateString()) <= DateTime.Parse(dateSeuilSup.ToShortDateString()))) // si l'équipement est dans la zone et que la date enqueté est dans le seuil
                        {
                            if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(dateSeuilInf.ToShortDateString())) // début
                            {
                                // Regarder pour définir le créneau
                                if (dateSeuilInf.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                {
                                    resasEquip.ListResasAprem.Add(infosResa);
                                    //Resas.InfosResaMatin.Add(null); // Matin vide
                                }
                                else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                                {
                                    resasEquip.ListResasMatin.Add(infosResa);
                                    resasEquip.ListResasAprem.Add(infosResa);
                                }
                            }
                            else if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(dateSeuilSup.ToShortDateString())) // fin
                            {
                                // Regarder pour définir le créneau
                                if (dateSeuilSup.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                                {
                                    resasEquip.ListResasMatin.Add(infosResa);
                                    //Resas.InfosResaAprem.Add(null); // Aprèm vide TODO: voir si on peut ajouter un element null!!
                                }
                                else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                                {
                                    resasEquip.ListResasMatin.Add(infosResa);
                                    resasEquip.ListResasAprem.Add(infosResa);
                                }
                            }
                            else
                            {
                                // Ajouter cette résa sur le créneau matin et aprèm 
                                resasEquip.ListResasMatin.Add(infosResa);
                                resasEquip.ListResasAprem.Add(infosResa);
                            }
                            IsEquipInZone = false;
                        }

                        #endregion
                        #endregion

                        break;
                    case "Confidentiel": // Blocage de toute la plateforme sauf pour les salles alimentaires (5 zones) et la zone equipements mobiles

                        #region Confidentialité "confidentiel" 

                        if (Equipement.zoneID.Equals((int)EnumZonesPfl.HaloirAp7) || Equipement.zoneID.Equals((int)EnumZonesPfl.SalleAp5) ||
                            Equipement.zoneID.Equals((int)EnumZonesPfl.SalleAp6) || Equipement.zoneID.Equals((int)EnumZonesPfl.SalleAp8) ||
                            Equipement.zoneID.Equals((int)EnumZonesPfl.SalleAp9)) //|| EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.EquipMobiles)) (TODO:  la zone equipements mobiles devrait être bloqué?)
                        {
                            // Pour ces zones alors faire comment on fait pour les essai du type "Ouvert" blocage uniquement des équipements
                            resasEquip = ResaConfidentialiteOuverte(ess, infosResa, IdEquipement, DateRecup);
                        }
                        else
                        {
                            if ((DateTime.Parse(DateRecup.ToShortDateString()) >= DateTime.Parse(ess.date_inf_confidentiel.Value.ToShortDateString()))   //Bonne manière de comparer les dates converties en shortstring!
                                && (DateTime.Parse(DateRecup.ToShortDateString()) <= DateTime.Parse(ess.date_sup_confidentiel.Value.ToShortDateString())))
                            {
                                // Créer une réservation uniquement pour avoir l'accès à l'essai (A modifier)
                                ResaAGarder = new reservation_projet { equipementID = IdEquipement, essaiID = ess.id, date_debut = ess.date_inf_confidentiel.Value, date_fin = ess.date_sup_confidentiel.Value, essai = ess };

                                if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(ess.date_inf_confidentiel.Value.ToShortDateString())) // début
                                {
                                    // Regarder pour définir le créneau
                                    if (ess.date_inf_confidentiel.Value.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                    {
                                        resasEquip.ListResasAprem.Add(infosResa);
                                    }
                                    else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                                    {
                                        resasEquip.ListResasMatin.Add(infosResa);
                                        resasEquip.ListResasAprem.Add(infosResa);
                                    }
                                }
                                else if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(ess.date_sup_confidentiel.Value.ToShortDateString())) // fin
                                {
                                    // Regarder pour définir le créneau
                                    if (ess.date_sup_confidentiel.Value.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                                    {
                                        resasEquip.ListResasMatin.Add(infosResa);
                                        //Resas.InfosResaAprem.Add(null); // Aprèm vide TODO: voir si on peut ajouter un element null!!
                                    }
                                    else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                                    {
                                        resasEquip.ListResasMatin.Add(infosResa);
                                        resasEquip.ListResasAprem.Add(infosResa);
                                    }
                                }
                                else
                                {
                                    // Ajouter cette résa sur le créneau matin et aprèm 
                                    resasEquip.ListResasMatin.Add(infosResa);
                                    resasEquip.ListResasAprem.Add(infosResa);
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
                JourCalendrier = DateRecup; // enregistrer la date en question
                NomJour = DateRecup.ToString("dddd", dateTimeFormats); // Reecrire le nom du jour car lors de l'appel de la méthode ResaConfidentialiteOuverte()
                // Resas est réinitialisé!
                // TODO: Requete vers la base de données pour obtenir toutes les réservations du type "maintenance" 
                // TODO: Requete vers la base de données pour obtenir toutes les réservations du type "métrologie" 

                if (NomJour != "samedi" && NomJour != "dimanche")
                {
                    // si plus d'une réservation à ce jour alors conflit entre 2 résas "Restreint"
                    if (resasEquip.ListResasMatin.Count() > 1) // réservations restreint
                        resasEquip.CouleurMatin = "#ffd191"; // Indiquer un chevauchement des créneaux réservation (orange)

                    if (resasEquip.ListResasAprem.Count() > 1)
                        resasEquip.CouleurAprem = "#ffd191"; // Indiquer un chevauchement des créneaux réservation (orange)

                    if (resasEquip.ListResasMatin.Count() == 1)
                    {
                        if (resasEquip.ListResasMatin[0].StatusEssai == EnumStatusEssai.Validate.ToString())
                            resasEquip.CouleurMatin = "#fdc0be"; // rouge (validée et occupée)
                        else if (resasEquip.ListResasMatin[0].StatusEssai == EnumStatusEssai.WaitingValidation.ToString())
                            resasEquip.CouleurMatin = "#fbeed9";  // Couleur beige pour indiquer que la réservation est en attente
                    }

                    // si pas de chevauchement des résas alors vérifier le status projet
                    if (resasEquip.ListResasAprem.Count() == 1)
                    {
                        if (resasEquip.ListResasAprem[0].StatusEssai == EnumStatusEssai.Validate.ToString())
                            resasEquip.CouleurAprem = "#fdc0be"; // rouge (validée et occupée)
                        else if (resasEquip.ListResasAprem[0].StatusEssai == EnumStatusEssai.WaitingValidation.ToString())
                            resasEquip.CouleurAprem = "#fbeed9";  // Couleur beige pour indiquer que la réservation est en attente
                    }


                    // CODE COULEUR DISPO SUR: https://encycolorpedia.fr/
                    // Definir les couleurs de fond pour indiquer si le créneau est occupé ou pas
                    if (resasEquip.ListResasMatin.Count() == 0) // si au moins une réservation le matin alors matinée occupée
                        resasEquip.CouleurMatin = "#6cc3bb"; // matin dispo (Vert)
                    if (resasEquip.ListResasAprem.Count() == 0) // si au moins une réservation l'aprèm alors aprèm occupée
                        resasEquip.CouleurAprem = "#6cc3bb"; // Aprèm libre (Vert)
                }
                else // si jour samedi ou dimanche alors mettre en fond gris
                {
                    resasEquip.CouleurMatin = "silver";
                    resasEquip.CouleurAprem = "silver";
                }

            #endregion

            return resasEquip;
        }

        public ResasEquipParJour ResaConfidentialiteOuverte(essai ess, InfosAffichageResa infosResa, int IdEquipement, DateTime DateRecup)
        {
            ResasEquipParJour EquipVsResa = new ResasEquipParJour();

            foreach (var resa in resaDB.reservation_projet.Where(r => r.essaiID == ess.id))
            {
                if ((resa.equipementID == IdEquipement) && (DateTime.Parse(DateRecup.ToShortDateString()) >= DateTime.Parse(resa.date_debut.ToShortDateString()))
                    && (DateTime.Parse(DateRecup.ToShortDateString()) <= DateTime.Parse(resa.date_fin.ToShortDateString()))) // Si l'équipement à afficher est impliqué dans l'essai
                {
                    if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resa.date_debut.ToShortDateString())) // si dateResa égal à resa.date_debut
                    {
                        // Regarder pour définir le créneau
                        if (resa.date_debut.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                        {
                            EquipVsResa.ListResasAprem.Add(infosResa);
                            //Resas.InfosResaMatin.Add(null); // Matin vide
                        }
                        else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                        {
                            EquipVsResa.ListResasMatin.Add(infosResa);
                            EquipVsResa.ListResasAprem.Add(infosResa);
                        }
                    }
                    else if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resa.date_fin.ToShortDateString())) // si dateResa égal à resa.date_fin
                    {
                        // Regarder pour définir le créneau
                        if (resa.date_fin.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                        {
                            EquipVsResa.ListResasMatin.Add(infosResa);
                            //Resas.InfosResaAprem.Add(null); // Aprèm vide TODO: voir si on peut ajouter un element null!!
                        }
                        else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                        {
                            EquipVsResa.ListResasMatin.Add(infosResa);
                            EquipVsResa.ListResasAprem.Add(infosResa);
                        }
                    }
                    else // date à l'intérieur du seuil de réservation
                    {
                        // Ajouter cette résa sur le créneau matin et aprèm 
                        EquipVsResa.ListResasMatin.Add(infosResa);
                        EquipVsResa.ListResasAprem.Add(infosResa);
                    }
                }
            }
            return EquipVsResa;
        }

        public InfosEquipementReserve ObtenirInfosResa(int IdEssai)
        {
            InfosEquipementReserve Infos = new InfosEquipementReserve();
            essai essai = ObtenirEssai(IdEssai);

            projet pr = ObtenirProjetEssai(essai);
            // si essai confidentiel copier uniquement le mail du responsable projet
            if(essai.confidentialite == EnumConfidentialite.Confidentiel.ToString())
            {
                Infos = new InfosEquipementReserve { MailResponsablePj = pr.mailRespProjet, MailAuteurResa = resaDB.Users.Find(Convert.ToInt32(essai.compte_userID)).Email, 
                    Confidentialite = essai.confidentialite, IdEssai = essai.id };
            }
            else
            {
                Infos = new InfosEquipementReserve { IdEssai = essai.id, MailAuteurResa = resaDB.Users.Find(Convert.ToInt32(essai.compte_userID)).Email, 
                    MailResponsablePj = pr.mailRespProjet, NumeroProjet = pr.num_projet, TitreProjet = pr.titre_projet, Confidentialite = essai.confidentialite };
            }

            return Infos;
        }

        public essai ObtenirEssai(int IdEssai)
        {
            return resaDB.essai.First(e => e.id == IdEssai);
        }

        public projet ObtenirProjetEssai(essai essai)
        {
            return resaDB.projet.First(p => p.id == essai.projetID);
        }
    }
}
