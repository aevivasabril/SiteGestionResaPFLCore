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
    }
}
