using Microsoft.AspNetCore.Identity;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using SiteGestionResaCore.Models.Maintenance;
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
            #region Variables pour les essais
            // Variables
            ReservationsJour Resas = new ReservationsJour();
            ReservationsJour ResasTemp = new ReservationsJour();

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

            #endregion

            #region Variables pour les Interventions

            List<maintenance> InfosInterv = new List<maintenance>();

            #endregion

            //TODO: question pour christophe: Comment faire une recherche en regardant la date aussi??? 
            SubInfosEssai = (from resa in context.reservation_projet
                            from essa in context.essai
                            where resa.essaiID == essa.id && 
                            (essa.status_essai == EnumStatusEssai.Validate.ToString() || 
                            essa.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                            select essa).Distinct().ToArray();

            // Récupérer les essais où la date enquêté est bien dans la plage de déroulement
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

                        ResasTemp = ResaConfidentialiteOuverte(ess, resaInfo, IdEquipement, dateResa);

                        #endregion

                        break;
                    case "Restreint": // si "Restreint" il faut bloquer toute la zone, alors vérifier si les réservations sont dans la même zone que l'équipement et bloquer l'équipement 

                        #region Confidentialité "Restreint"

                        // Traiter comment une réservation ouverte car on bloque uniquement l'équipement en mode restreint
                        // Comme ça l'utilisateur saura que malgré que la zone est en restreint il peut réserver des autres équipements

                        ResasTemp = ResaConfidentialiteOuverte(ess, resaInfo, IdEquipement, dateResa);     
                        
                        #endregion

                        break;
                    case "Confidentiel": // Blocage de toute la plateforme sauf pour les salles alimentaires (5 zones) et la zone equipements mobiles

                        #region Confidentialité "confidentiel" 

                        if (EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp7A) || EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp7B) ||
                            EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp7C) || EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp5) ||
                            EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp6) || EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp8) ||
                            EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.SalleAp9)) //|| EquipementPlanning.zoneID.Equals((int)EnumZonesPfl.EquipMobiles)) (TODO:  la zone equipements mobiles devrait être bloqué?)
                        {
                            // Pour ces zones alors faire comment on fait pour les essai du type "Ouvert" blocage uniquement des équipements
                            ResasTemp = ResaConfidentialiteRestreint(ess, resaInfo, EquipementPlanning, dateResa);
                        }
                        else
                        {
                            ResasTemp = ObtenirResasJourEssConfidentielPFL(ess, resaInfo, EquipementPlanning, dateResa);
                            
                        }
                        #endregion
                        break;
                }

                // Stocker les valeurs rétrouves pour cet essai 
                foreach (var res in ResasTemp.InfosResaMatin)
                {
                    Resas.InfosResaMatin.Add(res);
                }
                // Stocker les valeurs rétrouves pour cet essai 
                foreach (var res in ResasTemp.InfosResaAprem)
                {
                    Resas.InfosResaAprem.Add(res);
                }
            }

            #region Informations interventions maintenance (bloquer toute la zone pour tous les types d'interventions)

            var maints = (from interMaint in context.reservation_maintenance
                          from maint in context.maintenance
                          where interMaint.maintenanceID == maint.id &&
                          ((DatEnqDebMatin >= interMaint.date_debut || DatEnqDebAprem >= interMaint.date_debut)
                          && (DatEnqFinMatin <= interMaint.date_fin || DatEnqFinAprem <= interMaint.date_fin))
                          select maint).Distinct().ToArray();


            // Récupérer les essais où la date enquêté est bien dans la plage de déroulement
            foreach (var maint in maints)
            {
                var resaMaint = context.reservation_maintenance.Where(r => r.maintenanceID == maint.id).ToList();
                foreach (var resEs in resaMaint)
                {
                    if ((DatEnqDebMatin >= resEs.date_debut || DatEnqDebAprem >= resEs.date_debut) &&
                        (DatEnqFinMatin <= resEs.date_fin || DatEnqFinAprem <= resEs.date_fin))
                    {
                        InfosInterv.Add(maint);
                        break;
                    }
                }
            }

            foreach (var m in InfosInterv)
            {
                InfosAffichageMaint affichageMaint = new InfosAffichageMaint
                {
                    IdMaint = m.id,
                    CodeOperation = m.code_operation,
                    DescriptionOperation = m.description_operation,
                    TypeMaintenance = m.type_maintenance,
                    MailOperateur = context.Users.First(u=>u.Id == m.userID).Email
                };

                #region Determiner les créneaux des interventions

                ResasTemp = IntervEquipParJour(m, affichageMaint, EquipementPlanning, dateResa);

                // Stocker les valeurs rétrouves pour cet essai 
                foreach (var intMatin in ResasTemp.InfosIntervMatin)
                {
                    Resas.InfosIntervMatin.Add(intMatin);
                }
                // Stocker les valeurs rétrouves pour cet essai 
                foreach (var intAprem in ResasTemp.InfosIntervAprem)
                {
                    Resas.InfosIntervAprem.Add(intAprem);
                }
                #endregion
            }

            #endregion

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


                /// Définition des couleurs maintenance (regne sur les essais)  
                if (Resas.InfosIntervMatin.Count() == 1)
                {
                    Resas.CouleurFondMatin = "#70cff0"; // blue opération maintenance en cours
                }
                if (Resas.InfosIntervAprem.Count() == 1)
                {
                    Resas.CouleurFondAprem = "#70cff0"; // bleu opération maintenance en cours
                }

                // CODE COULEUR DISPO SUR: https://encycolorpedia.fr/
                // Definir les couleurs de fond pour indiquer si le créneau est occupé ou pas
                if (Resas.InfosResaMatin.Count() == 0 && Resas.InfosIntervMatin.Count() == 0) // si au moins une réservation le matin alors matinée occupée
                    Resas.CouleurFondMatin = "#c2e6e2"; // matin dispo (Vert)
                if (Resas.InfosResaAprem.Count() == 0 && Resas.InfosIntervAprem.Count() == 0) // si au moins une réservation l'aprèm alors aprèm occupée
                    Resas.CouleurFondAprem = "#c2e6e2"; // Aprèm libre (Vert)
            }
            else // si jour samedi ou dimanche alors mettre en fond gris
            {
                Resas.CouleurFondMatin = "silver";
                Resas.CouleurFondAprem = "silver";
            }

            #endregion

            return Resas;
        }

        /// <summary>
        /// Methode permettant de vérifier la disponibilité des équipements dans les cas des réservations standard
        /// mais aussi pour l'ajout des équipements sur une réservation "ouvert" (modification des réservations)
        /// même méthode car un essai ouvert ne peut pas être ni restreint ni confidentiel
        /// </summary>
        /// <param name="dateDebut"></param>
        /// <param name="dateFin"></param>
        /// <param name="idEquipement"></param>
        /// <returns></returns>
        public bool VerifDisponibilitéEquipement(DateTime dateDebut, DateTime dateFin, int idEquipement, string confidentialite)
        {
            bool estOuvertDisponible = false;
            bool estRestreintDispo = false;
            bool estConfidentielDispo = false;
            bool estInterventionDispo = false;

            // Récupérer l'id zone pour l'équipement enquêté
            var zon = (from equip in context.equipement
                       where equip.id == idEquipement
                       select equip.zoneID.Value).First();

            #region Vérification sur les réservations du type "Ouvert" où il faut juste vérifier par l'ID equipement

            // requete complète pour trouver les réservations où leur essai est "ouvert", l'id equipement est égal a idEquipement et la date souhaitée pour réservation est déjà prise
            var resasOuv = (from essai in context.essai
                         from resa in context.reservation_projet
                         where essai.confidentialite == EnumConfidentialite.Ouvert.ToString() && essai.id == resa.essaiID && resa.equipementID == idEquipement
                         && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                             essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                         && ( ((dateDebut >= resa.date_debut) || dateFin >= resa.date_debut) 
                         && ((dateDebut <= resa.date_fin) || dateFin <= resa.date_fin))
                         select resa).Distinct().ToList();

            if (resasOuv.Count() == 0) // aucun equipement réservé à ces dates! 
                estOuvertDisponible = true;

            #endregion

            #region Vérification sur les réservations "Restreint" 
            // TODO:  Conflit
            // requete pour recuperer les reservation dont il s'agit d'un essai "Restreint" pour cet équipement et où les dates sont déjà réservés
            var resasRest = (from essai in context.essai
                         from resa in context.reservation_projet
                         from equip in context.equipement
                         where essai.confidentialite == EnumConfidentialite.Restreint.ToString() && essai.id == resa.essaiID && resa.equipementID == idEquipement
                         && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                             essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                         && ( ( dateDebut >= resa.date_debut || dateFin >= resa.date_debut) 
                         && ( dateDebut <= resa.date_fin || dateFin <= resa.date_fin ))
                         select resa).Distinct().ToList();

            // lors de la validation des réservations mettre un conflit si une des 2 résas sont "RESTREINT" et que les équipement
            // sont differents mais dans la même zone (réservations validées ou à valider)
            // Pas de blocage pour réserver un autre équipement dans cette zone! jusqu'à la validation 
            if (resasRest.Count() == 0) // si aucune réservation directe sur l'equipement alors on peut réserver
                estRestreintDispo = true;

            #endregion

            #region Vérification sur les réservations "Confidentiel"

            int ApCinq = Convert.ToInt32(EnumZonesPfl.SalleAp5);
            int ApSix = Convert.ToInt32(EnumZonesPfl.SalleAp6);
            int ApSeptA = Convert.ToInt32(EnumZonesPfl.SalleAp7A);
            int ApSeptB = Convert.ToInt32(EnumZonesPfl.SalleAp7B);
            int ApSeptC = Convert.ToInt32(EnumZonesPfl.SalleAp7C);
            int ApHuit = Convert.ToInt32(EnumZonesPfl.SalleAp8);
            int ApNeuf = Convert.ToInt32(EnumZonesPfl.SalleAp9);
         
            // Si l'équipement que l'on souhaite réserver est dans la zone des salles alimentaires alors on doit bloquer les réservation s'une des 
            // réservations est dans la même zone, au mêmes dates et en mode confidentiel
            if(zon == ApCinq || zon == ApSix || zon == ApSeptA || zon == ApSeptB || zon == ApSeptC || zon == ApHuit || zon == ApNeuf)
            {
                // requete pour trouver les essais "confidentiels" avec les mêmes dates (Zones alimentaires)
                // l'équipement pour réservation est dans la zone des salles alimentaires (même traitement que sur une réservation restreint
                var essaiConfZonesAlim = (from essai in context.essai
                                          from equip in context.equipement
                                          from reser in context.reservation_projet
                                          where (essai.confidentialite == EnumConfidentialite.Confidentiel.ToString() && essai.id == reser.essaiID
                                          && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                             essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                                          && (reser.equipement.zoneID == ApCinq || reser.equipement.zoneID == ApSix || reser.equipement.zoneID == ApSeptA ||
                                          reser.equipement.zoneID == ApSeptB || reser.equipement.zoneID == ApSeptC || reser.equipement.zoneID == ApHuit || reser.equipement.zoneID == ApNeuf)
                                          && (reser.equipement.zoneID == zon)
                                          && (((dateDebut >= reser.date_debut) || dateFin >= reser.date_debut)
                                          && ((dateDebut <= reser.date_fin) || dateFin <= reser.date_fin)))
                                          select essai).Distinct().ToList();

                if (essaiConfZonesAlim.Count() == 0) // si aucune réservation "confidentiel sur ces dates et hors les zones alimentaires 
                    estConfidentielDispo = true;
            }
            else
            {
                // requete pour trouver les essais "confidentiels" avec les mêmes dates ( PFL )
                // s'une des réservations est sur la PFL, que l'équipement que l'on souhaite réserver est sur la PFL aussi et au mêmes dates
                // alors on bloque la réservation!
                var essaiConfPFL = (from essai in context.essai
                                    from equip in context.equipement
                                    from reser in context.reservation_projet
                                    where ((essai.confidentialite == EnumConfidentialite.Confidentiel.ToString() && essai.id == reser.essaiID)
                                    && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                        essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                                    && (reser.equipement.zoneID != ApCinq && reser.equipement.zoneID != ApSix && reser.equipement.zoneID != ApSeptA
                                    && reser.equipement.zoneID != ApSeptB && reser.equipement.zoneID != ApSeptC
                                    && reser.equipement.zoneID != ApHuit && reser.equipement.zoneID != ApNeuf)
                                    && (((dateDebut >= reser.date_debut) || dateFin >= reser.date_debut)
                                    && ((dateDebut <= reser.date_fin) || dateFin <= reser.date_fin)))
                                    select essai).Distinct().ToList();

                if (essaiConfPFL.Count() == 0) // si aucune réservation "confidentiel sur ces dates et hors les zones alimentaires 
                    estConfidentielDispo = true;        
            }


            #region Vérification sur les opérations de maintenance

            if (zon == ApCinq || zon == ApSix || zon == ApSeptA || zon == ApSeptB || zon == ApSeptC || zon == ApHuit || zon == ApNeuf)
            {            

                #region Vérification sur les opérations de maintenance salles alimentaires
                // si l'équipement que l'on rajoute dans un essai est dans la même zone d'une intervention aux mêmes dates alors il faut le bloquer
                var IntervZon = (from maint in context.maintenance
                                 from resaMaint in context.reservation_maintenance
                                 from equip in context.equipement
                                 where maint.id == resaMaint.maintenanceID
                                 && (resaMaint.equipement.zoneID == zon)
                                 && (resaMaint.equipement.zoneID == ApCinq || resaMaint.equipement.zoneID == ApSix || resaMaint.equipement.zoneID == ApSeptA
                                 || resaMaint.equipement.zoneID == ApSeptB || resaMaint.equipement.zoneID == ApSeptC || resaMaint.equipement.zoneID == ApHuit
                                 || resaMaint.equipement.zoneID == ApNeuf)
                                 && (((dateDebut >= resaMaint.date_debut) || dateFin >= resaMaint.date_debut)
                                 && ((dateDebut <= resaMaint.date_fin) || dateFin <= resaMaint.date_fin))
                                 select maint).Distinct().ToList();

                if (IntervZon.Count() == 0)
                    estInterventionDispo = true;

                #endregion
            }
            else
            {        
                #region Vérification sur les opérations de maintenance ZONE PFL
                // si l'équipement que l'on rajoute dans un essai est dans la même zone d'une intervention aux mêmes dates alors il faut le bloquer
                if(confidentialite == EnumConfidentialite.Confidentiel.ToString())
                {
                    var IntervZon = (from maint in context.maintenance
                                     from resaMaint in context.reservation_maintenance
                                     from equip in context.equipement
                                     where maint.id == resaMaint.maintenanceID
                                     && (resaMaint.equipement.zoneID != ApCinq && resaMaint.equipement.zoneID != ApSix && resaMaint.equipement.zoneID != ApSeptA
                                     && resaMaint.equipement.zoneID != ApSeptB && resaMaint.equipement.zoneID != ApSeptC && resaMaint.equipement.zoneID != ApHuit
                                     && resaMaint.equipement.zoneID != ApNeuf)
                                     && (((dateDebut >= resaMaint.date_debut) || dateFin >= resaMaint.date_debut)
                                     && ((dateDebut <= resaMaint.date_fin) || dateFin <= resaMaint.date_fin))
                                     select maint).Distinct().ToList();

                    if (IntervZon.Count() == 0)
                        estInterventionDispo = true;
                }
                else
                { // pour les réservatins ouvertes et restreint vérifier que l'equipement n'est pas dans la zone d'intervention
                    var IntervZon = (from maint in context.maintenance
                                     from resaMaint in context.reservation_maintenance
                                     from equip in context.equipement
                                     where maint.id == resaMaint.maintenanceID
                                     && (resaMaint.equipement.zoneID == zon)
                                     && (resaMaint.equipement.zoneID != ApCinq && resaMaint.equipement.zoneID != ApSix && resaMaint.equipement.zoneID != ApSeptA
                                     && resaMaint.equipement.zoneID != ApSeptB && resaMaint.equipement.zoneID != ApSeptC && resaMaint.equipement.zoneID != ApHuit
                                     && resaMaint.equipement.zoneID != ApNeuf)
                                     && (((dateDebut >= resaMaint.date_debut) || dateFin >= resaMaint.date_debut)
                                     && ((dateDebut <= resaMaint.date_fin) || dateFin <= resaMaint.date_fin))
                                     select maint).Distinct().ToList();

                    if (IntervZon.Count() == 0)
                        estInterventionDispo = true;
                }
                
                #endregion
            }
            #endregion

            #endregion

            return (estOuvertDisponible && estRestreintDispo && estConfidentielDispo && estInterventionDispo); // OK
        }


        public string ObtenirNomEquipement(int id)
        {
            return context.equipement.First(e => e.id == id).nom;
        }

        public string ObtenirNomZoneImpacte(int IdEquipement)
        {
            return context.zone.First(e => e.id == context.equipement.First(e => e.id == IdEquipement).zoneID).nom_zone;
        }

        public string ObtenirNomTypeMaintenance(int id)
        {
            return context.ld_type_maintenance.First(l => l.id == id).nom_type_maintenance;
        }

        /// <summary>
        /// méthode qui vérifie les essais utilisant le même équipement pour intervention où la même zone
        /// pas besoin de vérifier la confidentialité car dès moment où la zone soit bloqué c'est OK
        /// </summary>
        /// <param name="debutToSave"></param>
        /// <param name="finToSave"></param>
        /// <param name="idEquipement"></param>
        /// <returns></returns>
        public List<essai> ObtenirListEssaiXAnnulation(DateTime debutToSave, DateTime finToSave, int idEquipement)
        {
            List<essai> ListEssais = new List<essai>();

            int ApCinq = Convert.ToInt32(EnumZonesPfl.SalleAp5);
            int ApSix = Convert.ToInt32(EnumZonesPfl.SalleAp6);
            int ApSeptA = Convert.ToInt32(EnumZonesPfl.SalleAp7A);
            int ApSeptB = Convert.ToInt32(EnumZonesPfl.SalleAp7B);
            int ApSeptC = Convert.ToInt32(EnumZonesPfl.SalleAp7C);
            int ApHuit = Convert.ToInt32(EnumZonesPfl.SalleAp8);
            int ApNeuf = Convert.ToInt32(EnumZonesPfl.SalleAp9);

            // Récupérer l'id zone pour l'équipement enquêté
            //TODO: vérifier! !
            int zon = context.zone.First(z => z.id == context.equipement.First(e => e.id == idEquipement).zoneID).id;

            #region disponibilité sur les essais

            // Si l'équipement où l'on intervient est dans la PFL alors il faut vérifier qu'il n'y a pas des essais confidentiels aux mêmes dates
            // Vérifier qu'il y a pas autre maintenance sur ces dates et sur la même zone
            if (zon == ApCinq || zon == ApSix || zon == ApSeptA || zon == ApSeptB || zon == ApSeptC || zon == ApHuit || zon == ApNeuf)
            {
                // Vérifier la disponibilité sur les essais qui ont au moins un materiel dans la même zone
                // TODO: Vérifier
                // pour les zones alimentaires annuler uniquement les essais qui utilisent le même équipement car
                // les utilisateurs peuvent continuer à travailler très probablement sur les autres équipements de la zone
                ListEssais = (from essai in context.essai
                              from equip in context.equipement
                              from reser in context.reservation_projet
                              where (essai.id == reser.essaiID
                              && (reser.equipementID == idEquipement)
                              && (reser.equipement.zoneID == ApCinq || reser.equipement.zoneID == ApSix || reser.equipement.zoneID == ApSeptA
                                || reser.equipement.zoneID == ApSeptB || reser.equipement.zoneID == ApSeptC
                                || reser.equipement.zoneID == ApHuit || reser.equipement.zoneID == ApNeuf)
                              && (((debutToSave >= reser.date_debut) || finToSave >= reser.date_debut)
                              && ((debutToSave <= reser.date_fin) || finToSave <= reser.date_fin)))
                              select essai).Distinct().ToList();

            }
            else
            { // Equipements dans la Zone PFL
                ListEssais = (from essai in context.essai
                              from equip in context.equipement
                              from reser in context.reservation_projet
                              where (essai.id == reser.essaiID
                              && ((reser.equipement.id == idEquipement) || (reser.equipement.zoneID == zon)
                              || essai.confidentialite == EnumConfidentialite.Confidentiel.ToString())
                              && (((debutToSave >= reser.date_debut) || finToSave >= reser.date_debut)
                              && ((debutToSave <= reser.date_fin) || finToSave <= reser.date_fin)))
                              select essai).Distinct().ToList();
            }

            #endregion           

            return ListEssais;
        }


        public bool DispoEssaiRestreintPourAjout(DateTime dateDebut, DateTime dateFin, int idEquipement, int IdEssai)
        {
            //bool isGoodToAdd = false;

            bool estOuvertDisponible = false;
            bool estRestreintDispo = false;
            bool estConfidentielDispo = false;
            bool estInterventionDispo = false;

            // Récupérer l'id zone pour l'équipement enquêté
            var zon = (from equip in context.equipement
                       where equip.id == idEquipement
                       select equip.zoneID.Value).First();

            // Variable pour vérifier la dispo des equipements (sur les operations de maintenance
            var confidentialite = context.essai.First(e => e.id == IdEssai).confidentialite;

            #region Vérification sur les réservations du type "Ouvert" où il faut juste vérifier par l'ID equipement
            // TODO: Conflit
            // requete complète pour trouver les réservations où leur essai est "ouvert", l'id equipement est égal a idEquipement et la date souhaitée pour réservation est déjà prise
            var resasOuv = (from essai in context.essai
                            from resa in context.reservation_projet
                            where essai.confidentialite == EnumConfidentialite.Ouvert.ToString() && essai.id == resa.essaiID && resa.equipementID == idEquipement
                            && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                             essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                            && (((dateDebut >= resa.date_debut) || dateFin >= resa.date_debut)
                            && ((dateDebut <= resa.date_fin) || dateFin <= resa.date_fin))
                            select resa).Distinct().ToList();

            if (resasOuv.Count() == 0) // aucun equipement réservé à ces dates! 
                estOuvertDisponible = true;

            #endregion

            #region Vérification sur les réservations "Restreint" 
            // TODO: Conflit
            // requete pour recuperer les reservations dont il s'agit d'un essai "Restreint" pour cet équipement et où les dates sont déjà réservés
            var resasRest = (from essai in context.essai
                             from resa in context.reservation_projet
                             from equip in context.equipement
                             where essai.confidentialite == EnumConfidentialite.Restreint.ToString() && essai.id == resa.essaiID && resa.equipementID == idEquipement
                             && (essai.status_essai == EnumStatusEssai.Validate.ToString() || essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                             && essai.id != IdEssai
                             && ((dateDebut >= resa.date_debut || dateFin >= resa.date_debut)
                             && (dateDebut <= resa.date_fin || dateFin <= resa.date_fin))
                             select resa).Distinct().ToList();

            // lors de la validation des réservations mettre un conflit si une des 2 résas sont "RESTREINT" et que les équipement
            // sont differents mais dans la même zone (réservations validées ou à valider)
            // Pas de blocage pour réserver un autre équipement dans cette zone! jusqu'à la validation 
            if (resasRest.Count() == 0) // si aucune réservation directe sur l"equipement alors on peut réserver
                estRestreintDispo = true;

            #endregion

            #region Vérification sur les réservations "Confidentiel"

            int ApCinq = Convert.ToInt32(EnumZonesPfl.SalleAp5);
            int ApSix = Convert.ToInt32(EnumZonesPfl.SalleAp6);
            int ApSeptA = Convert.ToInt32(EnumZonesPfl.SalleAp7A);
            int ApSeptB = Convert.ToInt32(EnumZonesPfl.SalleAp7B);
            int ApSeptC = Convert.ToInt32(EnumZonesPfl.SalleAp7C);
            int ApHuit = Convert.ToInt32(EnumZonesPfl.SalleAp8);
            int ApNeuf = Convert.ToInt32(EnumZonesPfl.SalleAp9);

            // Si l'équipement que l'on souhaite réserver est dans la zone des salles alimentaires alors on doit bloquer les réservation s'une des 
            // réservations est dans la même zone, aux mêmes dates et en mode confidentiel
            if (zon == ApCinq || zon == ApSix || zon == ApSeptA || zon == ApSeptB || zon == ApSeptC || zon == ApHuit || zon == ApNeuf)
            {
                // requete pour trouver les essais "confidentiels" avec les mêmes dates
                var essaiConfZonesAlim = (from essai in context.essai
                                          from equip in context.equipement
                                          from reser in context.reservation_projet
                                          where (essai.confidentialite == EnumConfidentialite.Confidentiel.ToString() && (essai.id == reser.essaiID)
                                          && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                             essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                                          && (reser.equipement.zoneID == ApCinq || reser.equipement.zoneID == ApSix || reser.equipement.zoneID == ApSeptA
                                             || reser.equipement.zoneID == ApSeptB || reser.equipement.zoneID == ApSeptC
                                             || reser.equipement.zoneID == ApHuit || reser.equipement.zoneID == ApNeuf)
                                          && (reser.equipement.zoneID == zon)
                                          && (((dateDebut >= reser.date_debut) || dateFin >= reser.date_debut)
                                          && ((dateDebut <= reser.date_fin) || dateFin <= reser.date_fin)))
                                          select essai).Distinct().ToList();

                if (essaiConfZonesAlim.Count() == 0) // si aucune réservation "confidentiel sur ces dates et hors les zones alimentaires 
                    estConfidentielDispo = true;
            }
            else
            {
                // requete pour trouver les essais "confidentiels" avec les mêmes dates ( PFL )
                // s'une des réservations est sur la PFL, que l'équipement que l'on souhaite réserver est sur la PFL aussi et au mêmes dates
                // alors on bloque la réservation!
                var essaiConfPFL = (from essai in context.essai
                                    from equip in context.equipement
                                    from reser in context.reservation_projet
                                    where ((essai.confidentialite == EnumConfidentialite.Confidentiel.ToString() && essai.id == reser.essaiID)
                                    && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                        essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                                    && (reser.equipement.zoneID != ApCinq && reser.equipement.zoneID != ApSix && reser.equipement.zoneID != ApSeptA
                                    && reser.equipement.zoneID != ApSeptB && reser.equipement.zoneID != ApSeptC
                                    && reser.equipement.zoneID != ApHuit && reser.equipement.zoneID != ApNeuf)
                                    && (((dateDebut >= reser.date_debut) || dateFin >= reser.date_debut)
                                    && ((dateDebut <= reser.date_fin) || dateFin <= reser.date_fin)))
                                    select essai).Distinct().ToList();

                if (essaiConfPFL.Count() == 0) // si aucune réservation "confidentiel sur ces dates et hors les zones alimentaires 
                    estConfidentielDispo = true;
            }
            #endregion

            #region Vérification sur les opérations de maintenance

            // si l'équipement que l'on rajoute dans un essai RESTREINT est dans la même zone d'une intervention aux mêmes dates alors il faut le bloquer
            var IntervZon = (from maint in context.maintenance
                                from resaMaint in context.reservation_maintenance
                                from equip in context.equipement
                                where maint.id == resaMaint.maintenanceID
                                && (resaMaint.equipement.zoneID == zon)
                                && (((dateDebut >= resaMaint.date_debut) || dateFin >= resaMaint.date_debut)
                                && ((dateDebut <= resaMaint.date_fin) || dateFin <= resaMaint.date_fin))
                                select maint).Distinct().ToList();

            if (IntervZon.Count() == 0)
                estInterventionDispo = true;                  
            
            #endregion

            return (estOuvertDisponible && estRestreintDispo && estConfidentielDispo && estInterventionDispo);
        }

        public bool DispoEssaiConfidentielPourAjout(DateTime dateDebut, DateTime dateFin, int idEquipement, int IdEssai)
        {
            bool estOuvertDisponible = false;
            bool estRestreintDispo = false;
            bool estConfidentielDispo = false;
            bool estInterventionDispo = false;

            int ApCinq = Convert.ToInt32(EnumZonesPfl.SalleAp5);
            int ApSix = Convert.ToInt32(EnumZonesPfl.SalleAp6);
            int ApSeptA = Convert.ToInt32(EnumZonesPfl.SalleAp7A);
            int ApSeptB = Convert.ToInt32(EnumZonesPfl.SalleAp7B);
            int ApSeptC = Convert.ToInt32(EnumZonesPfl.SalleAp7C);
            int ApHuit = Convert.ToInt32(EnumZonesPfl.SalleAp8);
            int ApNeuf = Convert.ToInt32(EnumZonesPfl.SalleAp9);

            // Récupérer l'id zone pour l'équipement enquêté
            var zon = (from equip in context.equipement
                       where equip.id == idEquipement
                       select equip.zoneID.Value).First();

            #region Vérification sur les réservations du type "Ouvert" où il faut juste vérifier par l'ID equipement

            if(zon == ApCinq || zon == ApSix || zon == ApSeptA || zon == ApSeptB || zon == ApSeptC || zon == ApHuit || zon == ApNeuf)
            {
                // requete complète pour trouver les réservations où leur essai est "ouvert", l'id equipement est égal a idEquipement et la date souhaitée pour réservation est déjà prise
                var resasOuvZon = (from essai in context.essai
                                   from resa in context.reservation_projet
                                   from equip in context.equipement
                                   where essai.confidentialite == EnumConfidentialite.Ouvert.ToString() && essai.id == resa.essaiID
                                   && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                       essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                                   && (resa.equipement.zoneID == ApCinq || resa.equipement.zoneID == ApSix || resa.equipement.zoneID == ApSeptA
                                   || resa.equipement.zoneID == ApSeptB || resa.equipement.zoneID == ApSeptC
                                   || resa.equipement.zoneID == ApHuit || resa.equipement.zoneID == ApNeuf)
                                   && (resa.equipement.zoneID == zon)
                                   && (((dateDebut >= resa.date_debut) || dateFin >= resa.date_debut)
                                   && ((dateDebut <= resa.date_fin) || dateFin <= resa.date_fin))
                                   select resa).Distinct().ToList();

                // lors de la validation des réservations mettre un conflit si une des 2 résas sont "RESTREINT" et que les équipement
                // sont differents mais dans la même zone (réservations validées ou à valider)
                // Pas de blocage pour réserver un autre équipement dans cette zone! jusqu'à la validation 
                if (resasOuvZon.Count() == 0) // si aucune réservation directe sur l"equipement alors on peut réserver
                    estOuvertDisponible = true;
            }
            else
            {
                // comme il s'agit d'une réservation confidentiel il faut que la zone PFL soit libre
                var resasOuvPFL = (from essai in context.essai
                                   from resa in context.reservation_projet
                                   from equip in context.equipement
                                   where essai.confidentialite == EnumConfidentialite.Ouvert.ToString() && essai.id == resa.essaiID
                                   && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                       essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                                   && (resa.equipement.zoneID != ApCinq && resa.equipement.zoneID != ApSix && resa.equipement.zoneID != ApSeptA
                                   && resa.equipement.zoneID != ApSeptB && resa.equipement.zoneID != ApSeptC
                                   && resa.equipement.zoneID != ApHuit && resa.equipement.zoneID != ApNeuf)
                                   && (((dateDebut >= resa.date_debut) || dateFin >= resa.date_debut)
                                   && ((dateDebut <= resa.date_fin) || dateFin <= resa.date_fin))
                                   select resa).Distinct().ToList();

                // lors de la validation des réservations mettre un conflit si une des 2 résas sont "RESTREINT" et que les équipement
                // sont differents mais dans la même zone (réservations validées ou à valider)
                // Pas de blocage pour réserver un autre équipement dans cette zone! jusqu'à la validation 
                if (resasOuvPFL.Count() == 0) // si aucune réservation directe sur l"equipement alors on peut réserver
                    estOuvertDisponible = true;

            }

            #endregion

            #region Vérification sur les réservations "Restreint" 

            if (zon == ApCinq || zon == ApSix || zon == ApSeptA || zon == ApSeptB || zon == ApSeptC || zon == ApHuit || zon == ApNeuf)
            {
                // requete pour recuperer les reservation dont il s'agit d'un essai "Restreint" pour cet équipement et où les dates sont déjà réservés
                var resasRestZone = (from essai in context.essai
                                     from resa in context.reservation_projet
                                     from equip in context.equipement
                                     where essai.confidentialite == EnumConfidentialite.Restreint.ToString() && essai.id == resa.essaiID
                                     && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                     essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                                     && (resa.equipement.zoneID == ApCinq || resa.equipement.zoneID == ApSix || resa.equipement.zoneID == ApSeptA
                                     || resa.equipement.zoneID == ApSeptB || resa.equipement.zoneID == ApSeptC
                                     || resa.equipement.zoneID == ApHuit || resa.equipement.zoneID == ApNeuf)
                                     && (resa.equipement.zoneID == zon)
                                     && ((dateDebut >= resa.date_debut || dateFin >= resa.date_debut)
                                     && (dateDebut <= resa.date_fin || dateFin <= resa.date_fin))
                                     select resa).Distinct().ToList();

                // lors de la validation des réservations mettre un conflit si une des 2 résas sont "RESTREINT" et que les équipement
                // sont differents mais dans la même zone (réservations validées ou à valider)
                // Pas de blocage pour réserver un autre équipement dans cette zone! jusqu'à la validation 
                if (resasRestZone.Count() == 0) // si aucune réservation directe sur l"equipement alors on peut réserver
                    estRestreintDispo = true;
            }
            else
            {
                // requete pour recuperer les reservation dont il s'agit d'un essai "Restreint" pour cet équipement et où les dates sont déjà réservés
                var resasRestPFL = (from essai in context.essai
                                    from resa in context.reservation_projet
                                    from equip in context.equipement
                                    where essai.confidentialite == EnumConfidentialite.Restreint.ToString() && essai.id == resa.essaiID
                                    && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                    essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                                    && (resa.equipement.zoneID != ApCinq && resa.equipement.zoneID != ApSix && resa.equipement.zoneID != ApSeptA
                                    && resa.equipement.zoneID != ApSeptB && resa.equipement.zoneID != ApSeptC
                                    && resa.equipement.zoneID != ApHuit && resa.equipement.zoneID != ApNeuf)
                                    && ((dateDebut >= resa.date_debut || dateFin >= resa.date_debut)
                                    && (dateDebut <= resa.date_fin || dateFin <= resa.date_fin))
                                    select resa).Distinct().ToList();

                // lors de la validation des réservations mettre un conflit si une des 2 résas sont "RESTREINT" et que les équipement
                // sont differents mais dans la même zone (réservations validées ou à valider)
                // Pas de blocage pour réserver un autre équipement dans cette zone! jusqu'à la validation 
                if (resasRestPFL.Count() == 0) // si aucune réservation directe sur l"equipement alors on peut réserver
                    estRestreintDispo = true;
            }
  
            #endregion

            #region Vérification sur les réservations "Confidentiel"

            // Si l'équipement que l'on souhaite réserver est dans la zone des salles alimentaires alors on doit bloquer les réservation s'une des 
            // réservations est dans la même zone, au mêmes dates et en mode confidentiel
            if (zon == ApCinq || zon == ApSix || zon == ApSeptA || zon == ApSeptB || zon == ApSeptC || zon == ApHuit || zon == ApNeuf)
            {
                // requete pour trouver les essais "confidentiels" avec les mêmes dates
                var essaiConfZone = (from essai in context.essai
                                     from equip in context.equipement
                                     from reser in context.reservation_projet
                                     where (essai.confidentialite == EnumConfidentialite.Confidentiel.ToString() && essai.id == reser.essaiID
                                     && (essai.id != IdEssai)
                                     && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                         essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                                     && (reser.equipement.zoneID == ApCinq || reser.equipement.zoneID == ApSix || reser.equipement.zoneID == ApSeptA
                                     || reser.equipement.zoneID == ApSeptB || reser.equipement.zoneID == ApSeptC
                                     || reser.equipement.zoneID == ApHuit || reser.equipement.zoneID == ApNeuf)
                                     && (reser.equipement.zoneID == zon)
                                     && (((dateDebut >= reser.date_debut) || dateFin >= reser.date_debut)
                                     && ((dateDebut <= reser.date_fin) || dateFin <= reser.date_fin)))
                                     select essai).Distinct().ToList();

                if (essaiConfZone.Count() == 0) // si aucune réservation "confidentiel sur ces dates et hors les zones alimentaires 
                    estConfidentielDispo = true;
            }
            else
            {
                // requete pour trouver les essais "confidentiels" avec les mêmes dates ( PFL )
                // s'une des réservations est sur la PFL, que l'équipement que l'on souhaite réserver est sur la PFL aussi et au mêmes dates
                // alors on bloque la réservation!
                var essaiConfPFL = (from essai in context.essai
                                    from equip in context.equipement
                                    from reser in context.reservation_projet
                                    where ((essai.confidentialite == EnumConfidentialite.Confidentiel.ToString() && essai.id == reser.essaiID)
                                    && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                        essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                                    && (essai.id != IdEssai)
                                    && (reser.equipement.zoneID != ApCinq && reser.equipement.zoneID != ApSix && reser.equipement.zoneID != ApSeptA
                                    && reser.equipement.zoneID != ApSeptB && reser.equipement.zoneID != ApSeptC
                                    && reser.equipement.zoneID != ApHuit && reser.equipement.zoneID != ApNeuf)
                                    && (((dateDebut >= reser.date_debut) || dateFin >= reser.date_debut)
                                    && ((dateDebut <= reser.date_fin) || dateFin <= reser.date_fin)))
                                    select essai).Distinct().ToList();

                if (essaiConfPFL.Count() == 0) // si aucune réservation "confidentiel sur ces dates et hors les zones alimentaires 
                    estConfidentielDispo = true;
            }
            #endregion

            #region Vérification sur les opérations de maintenance

            if (zon == ApCinq || zon == ApSix || zon == ApSeptA || zon == ApSeptB || zon == ApSeptC || zon == ApHuit || zon == ApNeuf)
            {
                // si l'équipement que l'on rajoute dans un essai est dans la même zone d'une intervention aux mêmes dates alors il faut le bloquer
                var IntervZonAlim = (from maint in context.maintenance
                                    from resaMaint in context.reservation_maintenance
                                    from equip in context.equipement
                                    where maint.id == resaMaint.maintenanceID
                                    && (resaMaint.equipement.zoneID == ApCinq || resaMaint.equipement.zoneID == ApSix || resaMaint.equipement.zoneID == ApSeptA
                                    || resaMaint.equipement.zoneID == ApSeptB || resaMaint.equipement.zoneID == ApSeptC
                                    || resaMaint.equipement.zoneID == ApHuit || resaMaint.equipement.zoneID == ApNeuf)
                                    && (resaMaint.equipement.zoneID == zon)
                                    && (((dateDebut >= resaMaint.date_debut) || dateFin >= resaMaint.date_debut)
                                    && ((dateDebut <= resaMaint.date_fin) || dateFin <= resaMaint.date_fin))
                                    select maint).Distinct().ToList();
                if (IntervZonAlim.Count() == 0)
                    estInterventionDispo = true;
            }
            else
            { // si l'équipement est dans la zone PFL alors il doit pas y avoir aucune maintenance sur la PFL
                // si l'équipement que l'on rajoute dans un essai est dans la même zone d'une intervention aux mêmes dates alors il faut le bloquer
                var IntervZonPFL = (from maint in context.maintenance
                                    from resaMaint in context.reservation_maintenance
                                    from equip in context.equipement
                                    where maint.id == resaMaint.maintenanceID
                                    && (resaMaint.equipement.zoneID != ApCinq && resaMaint.equipement.zoneID != ApSix && resaMaint.equipement.zoneID != ApSeptA
                                    && resaMaint.equipement.zoneID != ApSeptB && resaMaint.equipement.zoneID != ApSeptC
                                    && resaMaint.equipement.zoneID != ApHuit && resaMaint.equipement.zoneID != ApNeuf)
                                    && (((dateDebut >= resaMaint.date_debut) || dateFin >= resaMaint.date_debut)
                                    && ((dateDebut <= resaMaint.date_fin) || dateFin <= resaMaint.date_fin))
                                    select maint).Distinct().ToList();

                if (IntervZonPFL.Count() == 0)
                    estInterventionDispo = true;           
            }


            #endregion

            return (estOuvertDisponible && estRestreintDispo && estConfidentielDispo && estInterventionDispo);
        }



        /// <summary>
        /// méthode pour déterminer la disponibilité d'une zone pour intervention
        /// Vérification des essais et des opérations de maintenance
        /// </summary>
        /// <param name="dateDebut"></param>
        /// <param name="dateFin"></param>
        /// <param name="idEquipement"></param>
        /// <returns></returns>
        public bool ZoneDisponibleXIntervention(DateTime dateDebut, DateTime dateFin, int idEquipement)
        {
            bool resaOk = false;
            bool interOk = false;

            int ApCinq = Convert.ToInt32(EnumZonesPfl.SalleAp5);
            int ApSix = Convert.ToInt32(EnumZonesPfl.SalleAp6);
            int ApSeptA = Convert.ToInt32(EnumZonesPfl.SalleAp7A);
            int ApSeptB = Convert.ToInt32(EnumZonesPfl.SalleAp7B);
            int ApSeptC = Convert.ToInt32(EnumZonesPfl.SalleAp7C);
            int ApHuit = Convert.ToInt32(EnumZonesPfl.SalleAp8);
            int ApNeuf = Convert.ToInt32(EnumZonesPfl.SalleAp9);

            // Récupérer l'id zone pour l'équipement enquêté
            var zon = (from equip in context.equipement
                       where equip.id == idEquipement
                       select equip.zoneID.Value).First();

            #region disponibilité sur les essais

            // Si l'équipement où l'on intervient est dans la PFL alors il faut vérifier qu'il n'y a pas des essais confidentiels aux mêmes dates
            // Vérifier qu'il y a pas autre maintenance sur ces dates et sur la même zone
            if (zon != ApCinq && zon != ApSix && zon != ApSeptA && zon != ApSeptB && zon != ApSeptC && zon != ApHuit && zon != ApNeuf)
            {
                // Vérifier la disponibilité sur les essais qu'ont au moins un materiel dans la même zone
                // J'ai enlevé la condition confidentiel car l'essai peut se derouler sauf si ça touche la même zone de mon intervention ou le même équipement
                var resasZon = (from essai in context.essai
                                from resa in context.reservation_projet
                                from equip in context.equipement
                                where essai.id == resa.essaiID
                                && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                    essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                                && (resa.equipement.zoneID == zon || resa.equipementID == idEquipement) //|| essai.confidentialite == EnumConfidentialite.Confidentiel.ToString())
                                && (resa.equipement.zoneID != ApCinq && resa.equipement.zoneID != ApSix && resa.equipement.zoneID != ApSeptA
                                && resa.equipement.zoneID != ApSeptB && resa.equipement.zoneID != ApSeptC
                                && resa.equipement.zoneID != ApHuit && resa.equipement.zoneID != ApNeuf)
                                && (((dateDebut >= resa.date_debut) || dateFin >= resa.date_debut)
                                && ((dateDebut <= resa.date_fin) || dateFin <= resa.date_fin))
                                select essai).Distinct().ToList();

                if (resasZon.Count() == 0)
                    resaOk = true;
            }
            else
            { // dans le cas des zones alimentaires alors on vérifie qu'il n'y a pas des essais dans la même zone de l'intervention
                var resasZon = (from essai in context.essai
                                from resa in context.reservation_projet
                                from equip in context.equipement
                                where essai.id == resa.essaiID
                                && (essai.status_essai == EnumStatusEssai.Validate.ToString() ||
                                    essai.status_essai == EnumStatusEssai.WaitingValidation.ToString())
                                && (resa.equipement.zoneID == zon)
                                && (resa.equipement.zoneID == ApCinq || resa.equipement.zoneID == ApSix || resa.equipement.zoneID == ApSeptA
                                || resa.equipement.zoneID == ApSeptB || resa.equipement.zoneID == ApSeptC
                                || resa.equipement.zoneID == ApHuit || resa.equipement.zoneID == ApNeuf)
                                && (((dateDebut >= resa.date_debut) || dateFin >= resa.date_debut)
                                && ((dateDebut <= resa.date_fin) || dateFin <= resa.date_fin))
                                select essai).Distinct().ToList();

                if (resasZon.Count() == 0)
                    resaOk = true;
            }

            #endregion

            #region disponibilité sur les interventions

            // Vérifier qu'il y a pas autre maintenance sur ces dates et sur la même zone
            var IntervZon = (from maint in context.maintenance
                             from resaMaint in context.reservation_maintenance
                             from equip in context.equipement
                             where maint.id == resaMaint.maintenanceID
                             && (resaMaint.equipement.zoneID == zon)
                             && (((dateDebut >= resaMaint.date_debut) || dateFin >= resaMaint.date_debut)
                             && ((dateDebut <= resaMaint.date_fin) || dateFin <= resaMaint.date_fin))
                             select maint).Distinct().ToList();

            if (IntervZon.Count() == 0)
                interOk = true;

            #endregion

            return (resaOk && interOk);
        }

        #region méthodes externes

        public ReservationsJour ResaConfidentialiteOuverte(essai ess, ReservationInfos resaInfo, int IdEquipement, DateTime dateResa)
        {
            ReservationsJour Resas = new ReservationsJour();

            foreach (var resa in context.reservation_projet.Where(r => r.essaiID == ess.id))
            {
                if ( (resa.equipementID == IdEquipement) && ( DateTime.Parse(dateResa.ToShortDateString()) >= DateTime.Parse(resa.date_debut.ToShortDateString()) )
                    && ( DateTime.Parse(dateResa.ToShortDateString()) <= DateTime.Parse(resa.date_fin.ToShortDateString()) ) ) // Si l'équipement à afficher est impliqué dans l'essai
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
                            // Vérifier si il s'agit d'une demi journée (juste l'aprèm)
                            if (resa.date_fin.Hour.Equals(12) && (resa.date_fin.ToShortDateString() == dateResa.ToShortDateString())) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                            {
                                Resas.InfosResaMatin.Add(resaInfo);
                                //Resas.InfosResaMatin.Add(null); // Matin vide
                            }
                            else  // si l'heure de fin est 18h alors on rajoute sur les 2
                            {
                                Resas.InfosResaMatin.Add(resaInfo);
                                Resas.InfosResaAprem.Add(resaInfo);
                            }
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

        ReservationsJour ObtenirResasJourEssConfidentielPFL(essai ess, ReservationInfos resaInfo, equipement Equipement, DateTime dateResa)
        {
            ReservationsJour Resas = new ReservationsJour();

            int ApCinq = Convert.ToInt32(EnumZonesPfl.SalleAp5);
            int ApSix = Convert.ToInt32(EnumZonesPfl.SalleAp6);
            int ApSeptA = Convert.ToInt32(EnumZonesPfl.SalleAp7A);
            int ApSeptB = Convert.ToInt32(EnumZonesPfl.SalleAp7B);
            int ApSeptC = Convert.ToInt32(EnumZonesPfl.SalleAp7C);
            int ApHuit = Convert.ToInt32(EnumZonesPfl.SalleAp8);
            int ApNeuf = Convert.ToInt32(EnumZonesPfl.SalleAp9);

            var resas = context.reservation_projet.Where(r => r.essaiID == ess.id);

            foreach (var resa in resas)
            {
                var equip = context.equipement.First(e => e.id == resa.equipementID);
                if (!equip.zoneID.Equals((int)EnumZonesPfl.SalleAp7A) && !equip.zoneID.Equals((int)EnumZonesPfl.SalleAp7B) && !equip.zoneID.Equals((int)EnumZonesPfl.SalleAp7C) &&
                    !equip.zoneID.Equals((int)EnumZonesPfl.SalleAp5) && !equip.zoneID.Equals((int)EnumZonesPfl.SalleAp6) && !equip.zoneID.Equals((int)EnumZonesPfl.SalleAp8) &&
                    !equip.zoneID.Equals((int)EnumZonesPfl.SalleAp9))
                {
                    if (DateTime.Parse(dateResa.ToShortDateString()) >= DateTime.Parse(resa.date_debut.ToShortDateString())
                    && DateTime.Parse(dateResa.ToShortDateString()) <= DateTime.Parse(resa.date_fin.ToShortDateString()))
                    {
                        // vérifier si l'essai n'est pas déjà dans la liste Matin
                        var EssaiDejaAjouteMatin = Resas.InfosResaMatin.Any(e => e.numEssai == ess.id);
                        var EssaiDejaAjouteAprem = Resas.InfosResaAprem.Any(e => e.numEssai == ess.id);

                        if (DateTime.Parse(dateResa.ToShortDateString()) == DateTime.Parse(resa.date_debut.ToShortDateString())) // début
                        {
                            // Regarder pour définir le créneau
                            if (resa.date_debut.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                            {
                                if (!EssaiDejaAjouteAprem)
                                {
                                    Resas.InfosResaAprem.Add(resaInfo);
                                    //Resas.InfosResaMatin.Add(null); // Matin vide
                                }
                            }
                            else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                // Regarder pour définir le créneau
                                if (resa.date_fin.Hour.Equals(12) && (resa.date_fin.ToShortDateString() == dateResa.ToShortDateString())) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                {
                                    if (!EssaiDejaAjouteMatin)
                                    {
                                        Resas.InfosResaMatin.Add(resaInfo);
                                    }
                                    //Resas.InfosResaMatin.Add(null); // Matin vide
                                }
                                else // si l'heure de fin est 18h alors on rajoute sur les 2
                                {
                                    if (!EssaiDejaAjouteMatin)
                                    {
                                        Resas.InfosResaMatin.Add(resaInfo);
                                    }
                                    if (!EssaiDejaAjouteAprem)
                                    {
                                        Resas.InfosResaAprem.Add(resaInfo);
                                    }
                                }
                            }
                        }
                        else if (DateTime.Parse(dateResa.ToShortDateString()) == DateTime.Parse(resa.date_fin.ToShortDateString())) // fin
                        {
                            // Regarder pour définir le créneau
                            if (resa.date_fin.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                            {
                                if (!EssaiDejaAjouteMatin)
                                {
                                    Resas.InfosResaMatin.Add(resaInfo);
                                }
                            }
                            else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                if (!EssaiDejaAjouteMatin)
                                {
                                    Resas.InfosResaMatin.Add(resaInfo);
                                }
                                if (!EssaiDejaAjouteAprem)
                                {
                                    Resas.InfosResaAprem.Add(resaInfo);
                                }
                            }
                        }
                        else
                        {
                            // Ajouter cette résa sur le créneau matin et aprèm 
                            if (!EssaiDejaAjouteMatin)
                            {
                                Resas.InfosResaMatin.Add(resaInfo);
                            }
                            if (!EssaiDejaAjouteAprem)
                            {
                                Resas.InfosResaAprem.Add(resaInfo);
                            }
                        }
                    }
                }
            }

            return Resas;
        }

        public ReservationsJour IntervEquipParJour(maintenance maint, InfosAffichageMaint infosAffichage, equipement Equipement, DateTime DateRecup)
        {
            ReservationsJour EquipVsResa = new ReservationsJour();

            foreach (var resaInter in context.reservation_maintenance.Where(r => r.maintenanceID == maint.id))
            {
                if (context.equipement.Where(e => e.id == resaInter.equipementID).First().zoneID.Value == Equipement.zoneID) // si l'équipement objet du "planning" est dans la zone d'une réservation
                {
                    if ((DateTime.Parse(DateRecup.ToShortDateString()) >= DateTime.Parse(resaInter.date_debut.ToShortDateString()))
                        && (DateTime.Parse(DateRecup.ToShortDateString()) <= DateTime.Parse(resaInter.date_fin.ToShortDateString()))) // Si l'équipement à afficher est impliqué dans l'essai
                    {
                        if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resaInter.date_debut.ToShortDateString())) // si dateResa égal à resa.date_debut
                        {
                            // Regarder pour définir le créneau
                            if (resaInter.date_debut.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                            {
                                EquipVsResa.InfosIntervAprem.Add(infosAffichage);
                                //Resas.InfosResaMatin.Add(null); // Matin vide
                            }
                            else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                // Vérifier si il s'agit d'une demi journée (juste l'aprèm)
                                if (resaInter.date_fin.Hour.Equals(12) && (resaInter.date_fin.ToShortDateString() == DateRecup.ToShortDateString())) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                {
                                    EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                                    //Resas.InfosResaMatin.Add(null); // Matin vide
                                }
                                else  // si l'heure de fin est 18h alors on rajoute sur les 2
                                {
                                    EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                                    EquipVsResa.InfosIntervAprem.Add(infosAffichage);
                                }
                            }
                        }
                        else if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resaInter.date_fin.ToShortDateString())) // si dateResa égal à resa.date_fin
                        {
                            // Regarder pour définir le créneau
                            if (resaInter.date_fin.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                            {
                                EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                            }
                            else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                                EquipVsResa.InfosIntervAprem.Add(infosAffichage);
                            }
                        }
                        else // date à l'intérieur du seuil de réservation
                        {
                            // Ajouter cette résa sur le créneau matin et aprèm 
                            EquipVsResa.InfosIntervMatin.Add(infosAffichage);
                            EquipVsResa.InfosIntervAprem.Add(infosAffichage);
                        }
                    }
                }
            }
            return EquipVsResa;
        }

        ReservationsJour ResaConfidentialiteRestreint(essai ess, ReservationInfos infosResa, equipement Equipement, DateTime DateRecup)
        {
            ReservationsJour EquipVsResa = new ReservationsJour();

            var resas = context.reservation_projet.Where(r => r.essaiID == ess.id);

            foreach (var resa in resas)
            {
                if (context.equipement.Where(e => e.id == resa.equipementID).First().zoneID.Value == Equipement.zoneID) // si l'équipement objet du "planning" est dans la zone d'une réservation
                {
                    if (DateTime.Parse(DateRecup.ToShortDateString()) >= DateTime.Parse(resa.date_debut.ToShortDateString())
                        && DateTime.Parse(DateRecup.ToShortDateString()) <= DateTime.Parse(resa.date_fin.ToShortDateString()))
                    {
                        #region vérifier si l'essai n'est pas déjà dans la liste Matin
                        // vérifier si l'essai n'est pas déjà dans la liste Matin
                        var EssaiDejaAjouteMatin = EquipVsResa.InfosResaMatin.Any(e => e.numEssai == ess.id);
                        var EssaiDejaAjouteAprem = EquipVsResa.InfosResaAprem.Any(e => e.numEssai == ess.id);

                        if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resa.date_debut.ToShortDateString())) // début
                        {
                            // Regarder pour définir le créneau
                            if (resa.date_debut.Hour.Equals(13)) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                            {
                                if (!EssaiDejaAjouteAprem)
                                {
                                    EquipVsResa.InfosResaAprem.Add(infosResa);
                                }
                            }
                            else // si l'heure de debut est 7h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                if (resa.date_fin.Hour.Equals(12) && (resa.date_fin.ToShortDateString() == DateRecup.ToShortDateString())) // si l'heure de debut de réservation est l'aprèm alors rajouter cette résa dans le créneau aprèm
                                {
                                    if (!EssaiDejaAjouteMatin)
                                    {
                                        EquipVsResa.InfosResaMatin.Add(infosResa);
                                    }
                                    //Resas.InfosResaMatin.Add(null); // Matin vide
                                }
                                else // si l'heure de fin est 18h alors on rajoute sur les 2
                                {
                                    if (!EssaiDejaAjouteMatin)
                                    {
                                        EquipVsResa.InfosResaMatin.Add(infosResa);
                                    }
                                    if (!EssaiDejaAjouteAprem)
                                    {
                                        EquipVsResa.InfosResaAprem.Add(infosResa);
                                    }
                                }
                            }
                        }
                        else if (DateTime.Parse(DateRecup.ToShortDateString()) == DateTime.Parse(resa.date_fin.ToShortDateString())) // fin
                        {
                            // Regarder pour définir le créneau
                            if (resa.date_fin.Hour.Equals(12)) // si l'heure de fin de réservation est midi alors rajouter cette résa dans le créneau du matin
                            {
                                if (!EssaiDejaAjouteMatin)
                                {
                                    EquipVsResa.InfosResaMatin.Add(infosResa);
                                }
                            }
                            else // si l'heure de fin est 18h alors on rajoute dans les 2 créneau les infos réservation
                            {
                                if (!EssaiDejaAjouteMatin)
                                {
                                    EquipVsResa.InfosResaMatin.Add(infosResa);
                                }
                                if (!EssaiDejaAjouteAprem)
                                {
                                    EquipVsResa.InfosResaAprem.Add(infosResa);
                                }
                            }
                        }
                        else
                        {
                            // Ajouter cette résa sur le créneau matin et aprèm 
                            if (!EssaiDejaAjouteMatin)
                            {
                                EquipVsResa.InfosResaMatin.Add(infosResa);
                            }
                            if (!EssaiDejaAjouteAprem)
                            {
                                EquipVsResa.InfosResaAprem.Add(infosResa);
                            }
                        }
                        #endregion
                    }
                }
            }
            return EquipVsResa;
        }

        #endregion


    }
}
