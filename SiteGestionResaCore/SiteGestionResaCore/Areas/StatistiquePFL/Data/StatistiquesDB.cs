using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.StatistiquePFL.Data
{
    public class StatistiquesDB: IStatistiquesDB
    {
        private readonly GestionResaContext resaDB;
        private readonly ILogger<StatistiquesDB> logger;
        private readonly UserManager<utilisateur> userManager;

        public StatistiquesDB(
            GestionResaContext resaDB,
            ILogger<StatistiquesDB> logger,
            UserManager<utilisateur> userManager)
        {
            this.resaDB = resaDB;
            this.logger = logger;
            this.userManager = userManager;
        }

        public List<InfosReservations> ObtenirResasDuAu(DateTime datedu, DateTime dateau)
        {
            List<InfosReservations> infos = new List<InfosReservations>();
            InfosReservations info = new InfosReservations();
            
            var listEssai = (from resa in resaDB.reservation_projet
                             from essa in resaDB.essai
                             where (resa.essaiID == essa.id)
                             && (essa.resa_refuse.Value != true && essa.resa_supprime.Value != true)
                             && ((resa.date_debut >= datedu && resa.date_fin <= dateau))
                             select essa).Distinct().ToList();

            foreach(var ess in listEssai)
            {
                var projet = resaDB.projet.First(p => p.id == ess.projetID);
                var organisme = resaDB.organisme.First(o => o.id == projet.organismeID);
                //var user = resaDB.Users.First(u=>u.Id == projet.compte_userID)
                var equipeStlo = (from user in resaDB.Users
                                 from equipe in resaDB.ld_equipes_stlo
                                 where user.equipeID == equipe.id && user.Id == projet.compte_userID
                                 select equipe).First();

                var ListResas = (from resa in resaDB.reservation_projet
                                from essai in resaDB.essai
                                where resa.essaiID == ess.id
                                && ((resa.date_debut >= datedu || resa.date_fin <= dateau))
                                 select resa).Distinct().ToList();
                
                foreach(var resa in ListResas)
                {
                    double nbJour = 0.00;
                    var equipement = resaDB.equipement.First(e => e.id == resa.equipementID);

                    #region Calcul des jours pour une reservation

                    var diff = resa.date_fin - resa.date_debut;

                    if (diff.Hours == 5) // démi journée
                        nbJour = diff.Days + 0.5;
                    else if (diff.Hours >= 5) // réservation commençant l'aprèm et finissant le jour suivant le matin 
                        nbJour = diff.Days + 1; 

                    #endregion

                    info = new InfosReservations
                    {
                        IdEssai = ess.id,
                        DateCreation = ess.date_creation,
                        TitreEssai = ess.titreEssai,
                        NomEquipe = equipeStlo.nom_equipe,
                        NomEquipement = equipement.nom,
                        NomOrganisme = organisme.nom_organisme,
                        NumProjet = projet.num_projet,
                        RespProjet = projet.mailRespProjet,
                        TitreProjet = projet.titre_projet,
                        TypeProjet = projet.type_projet,
                        DateDebutResa = resa.date_debut,
                        DateFinResa = resa.date_fin,
                        NbJours = nbJour
                    };
                    infos.Add(info);
                }
            }

            return infos;
        }

        public List<ZoneStats> ObtenirListZones()
        {
            List<ZoneStats> Liste = new List<ZoneStats>();

            var zons = resaDB.zone.Distinct().ToList();
            foreach(var zon in zons)
            {
                ZoneStats zone = new ZoneStats { IdZone = zon.id, NomZone = zon.nom_zone };
                Liste.Add(zone);
            }

            return Liste;
        }

        public List<InfosEquipVsJours> ObtListEquipsVsJours(int idZone, int Annee)
        {
            List<InfosEquipVsJours> list = new List<InfosEquipVsJours>();

            // TODO: Améliorer car je prends en compte tous les essais même supprimés
            var reservations = (from resa in resaDB.reservation_projet
                                from equip in resaDB.equipement
                                from essa in resaDB.essai
                                where resa.date_debut.Year == Annee && resa.date_fin.Year == Annee
                                && equip.zoneID == idZone && essa.resa_refuse != true && essa.resa_supprime != true && resa.essaiID == essa.id
                                select resa).Distinct().ToList().GroupBy(r=>r.equipementID);

            var equipements = resaDB.equipement.Where(e=>e.zoneID == idZone).Distinct().ToList();

            foreach(var equip in equipements)
            {
                double nbJours = 0;
                if(reservations.Where(r=>r.Key == equip.id).Count() != 0) // Si existe au moins une réservation pour cet équipement cet année
                {
                    var listResasXEquip = reservations.Where(r => r.Key == equip.id).ToList(); // Reagrouper les réservations pour cet équipement
                    foreach(var obje in listResasXEquip)
                    {
                        foreach(var resa in obje)
                        {
                            #region Calcul des jours pour une reservation

                            var diff = resa.date_fin - resa.date_debut;
                            if (diff.Hours == 5) // démi journée
                                nbJours = nbJours + diff.Days + 0.5;
                            else if (diff.Hours >= 5) // réservation commençant l'aprèm et finissant le jour suivant le matin 
                                nbJours = nbJours + diff.Days + 1;
                            #endregion
                        }
                    }
                }
                else // Pas de réservation pour cet équipement donc 0 jours
                {
                    nbJours = 0;
                }

                InfosEquipVsJours infos = new InfosEquipVsJours { 
                    IdEquipement = equip.id,
                    NomEquipement = equip.nom,
                    NbJours = nbJours
                };

                list.Add(infos);
            }

            return list;
        }

        public zone ObtNomZone(int id)
        {
            return resaDB.zone.First(z => z.id == id);
        }

        public List<projet> ObtenirListProjet()
        {
            return resaDB.projet.Distinct().OrderByDescending(p => p.date_creation).ToList();
        }

        public List<InfosReservations> ObtRecapitulatifXProjet(int IdProjet)
        {
            List<InfosReservations> infos = new List<InfosReservations>();
            InfosReservations info = new InfosReservations();

            var projet = resaDB.projet.First(p => p.id == IdProjet);
            var organisme = resaDB.organisme.First(o => o.id == projet.organismeID);

            var essaisXProjet = (from ess in resaDB.essai
                                 where ess.projetID == IdProjet && ess.resa_refuse != true && ess.resa_supprime != true
                                 select ess).Distinct().ToList();

            var equipeStlo = (from user in resaDB.Users
                              from equipe in resaDB.ld_equipes_stlo
                              where user.equipeID == equipe.id && user.Id == projet.compte_userID
                              select equipe).First();

            foreach (var x in essaisXProjet)
            {
                var reservs = resaDB.reservation_projet.Where(r => r.essaiID == x.id);
                foreach(var res in reservs)
                {
                    var equipement = resaDB.equipement.First(e => e.id == res.equipementID);

                    info = new InfosReservations
                    {
                        NumProjet = projet.num_projet,
                        TypeProjet = projet.type_projet,
                        RespProjet = projet.mailRespProjet,
                        TitreProjet = projet.titre_projet,
                        TitreEssai = x.titreEssai,
                        DateCreation = x.date_creation,
                        NomOrganisme = organisme.nom_organisme,
                        IdEssai = x.id,
                        DateDebutResa = res.date_debut,
                        DateFinResa = res.date_fin,
                        NomEquipement = equipement.nom,
                        NomEquipe = equipeStlo.nom_equipe
                    };
                    infos.Add(info);
                }
            }

            return infos;
        }
    }
}
