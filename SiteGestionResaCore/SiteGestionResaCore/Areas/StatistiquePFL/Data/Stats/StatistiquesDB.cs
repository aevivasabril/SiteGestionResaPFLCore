﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Areas.StatistiquePFL.Data.Stats;
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
                        ZoneEquipement = resaDB.zone.First(z => z.id == equipement.zoneID).nom_zone,
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
                var listResasXEquip = reservations.Where(r => r.Key == equip.id).ToList(); // Reagrouper les réservations pour cet équipement
                if (listResasXEquip.Count() != 0) // Si existe au moins une réservation pour cet équipement cet année
                {                    
                    foreach(var obje in listResasXEquip)
                    {
                        foreach(var resa in obje)
                        {
                            #region Calcul des jours pour une reservation

                            var diff = resa.date_fin - resa.date_debut;
                            if (diff.Hours == 5 && diff.Days == 0) // une demi journée
                            {
                                nbJours = nbJours + diff.Days + 0.5;
                            }
                            else if (diff.Hours >= 5 && diff.Hours <= 11 && diff.Days == 0) // réservation commençant l'aprèm et finissant l'aprèm
                            {
                                nbJours = nbJours + diff.Days + 1;
                            }
                            else if (diff.Hours >= 5 && diff.Days >1) // réservation qui peut se prolonger sur plusieurs jours et pouvant aller même le weekeend
                            {
                                var dateBegin = resa.date_debut;
                                for(int i=0; i <= diff.Days; i++)
                                {
                                    if(dateBegin.Date.DayOfWeek.ToString() != "Saturday" && dateBegin.Date.DayOfWeek.ToString() != "Sunday")
                                    {
                                        var diffW = resa.date_fin - dateBegin;
                                        if (diffW.Days == 0 && diffW.Hours == 5)
                                        {
                                            // démi journée
                                            nbJours = nbJours + 0.5;
                                        }
                                        if (diffW.Days == 0 && diffW.Hours == 11)
                                        {
                                            // une journée
                                            nbJours = nbJours + 1;
                                        }
                                        else if (diffW.Hours >= 5 && diffW.Hours <= 11 && diffW.Days >=1) // réservation commençant l'aprèm et finissant l'aprèm
                                        {
                                            nbJours = nbJours + 1;
                                        }
                                    }
                                    dateBegin = dateBegin.AddDays(1);
                                }
                            }
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
                        ZoneEquipement = resaDB.zone.First(z => z.id == equipement.zoneID).nom_zone,
                        NomEquipe = equipeStlo.nom_equipe
                    };
                    infos.Add(info);
                }
            }

            return infos;
        }

        public projet ObtenirProjet(int IdProjet)
        {
            return resaDB.projet.First(p => p.id == IdProjet);
        }

        public List<ld_equipes_stlo> ObtenirListEquip()
        {
            return resaDB.ld_equipes_stlo.Distinct().ToList();
        }

        public List<InfosReservations> ObtRecapitulatifXEquipe(int IdEquipeStlo, DateTime datedebut, DateTime datefin)
        {
            List<InfosReservations> infos = new List<InfosReservations>();
            InfosReservations info = new InfosReservations();

            var equipeStlo = resaDB.ld_equipes_stlo.First(e => e.id == IdEquipeStlo);
            var requete = (from resa in resaDB.reservation_projet
                           from equip in resaDB.ld_equipes_stlo
                           from essa in resaDB.essai
                           from user in resaDB.Users
                           where resa.date_debut >= datedebut && resa.date_fin <= datefin
                           && (resa.essaiID == essa.id && essa.compte_userID == user.Id && user.equipeID == IdEquipeStlo
                           && essa.resa_supprime != true && essa.resa_refuse != true)
                           select resa).Distinct().ToList();
            
            foreach(var r in requete)
            {
                var essai = resaDB.essai.First(e => e.id == r.essaiID);
                var projet = resaDB.projet.First(p=>p.id == essai.projetID);
                var organisme = resaDB.organisme.First(o => o.id == projet.organismeID);
                var equipement = resaDB.equipement.First(e => e.id == r.equipementID);

                info = new InfosReservations
                {
                    NumProjet = projet.num_projet,
                    TypeProjet = projet.type_projet,
                    RespProjet = projet.mailRespProjet,
                    TitreProjet = projet.titre_projet,
                    TitreEssai = essai.titreEssai,
                    DateCreation = essai.date_creation,
                    NomOrganisme = organisme.nom_organisme,
                    IdEssai = essai.id,
                    DateDebutResa = r.date_debut,
                    DateFinResa = r.date_fin,
                    NomEquipement = equipement.nom,
                    ZoneEquipement = resaDB.zone.First(z => z.id == equipement.zoneID).nom_zone,
                    NomEquipe = equipeStlo.nom_equipe
                };
                infos.Add(info);
            }
            return infos;
        }

        public ld_equipes_stlo ObtInfosEquipe(int IdEquipe)
        {
            return resaDB.ld_equipes_stlo.First(e => e.id == IdEquipe);
        }

        public List<organisme> ListeOrganismes()
        {
            return resaDB.organisme.ToList();
        }

        public List<InfosReservations> ObtRecapitulatifXOrg(int IdOrg, DateTime datedebut, DateTime datefin)
        {
            List<InfosReservations> infos = new List<InfosReservations>();
            InfosReservations info = new InfosReservations();

            var org = resaDB.organisme.First(e => e.id == IdOrg);
            var requete = (from resa in resaDB.reservation_projet
                           from orga in resaDB.organisme
                           from essa in resaDB.essai
                           from user in resaDB.Users
                           where resa.date_debut >= datedebut && resa.date_fin <= datefin
                           && (resa.essaiID == essa.id && essa.compte_userID == user.Id && user.organismeID == IdOrg
                           && essa.resa_supprime != true && essa.resa_refuse != true)
                           select resa).Distinct().ToList();

            foreach (var r in requete)
            {
                var essai = resaDB.essai.First(e => e.id == r.essaiID);
                var projet = resaDB.projet.First(p => p.id == essai.projetID);

                var equipeStlo = (from user in resaDB.Users
                                  from equipe in resaDB.ld_equipes_stlo
                                  where user.equipeID == equipe.id && user.Id == projet.compte_userID
                                  select equipe).First();

                var organisme = resaDB.organisme.First(o => o.id == projet.organismeID);
                var equipement = resaDB.equipement.First(e => e.id == r.equipementID);

                info = new InfosReservations
                {
                    NumProjet = projet.num_projet,
                    TypeProjet = projet.type_projet,
                    RespProjet = projet.mailRespProjet,
                    TitreProjet = projet.titre_projet,
                    TitreEssai = essai.titreEssai,
                    DateCreation = essai.date_creation,
                    NomOrganisme = organisme.nom_organisme,
                    IdEssai = essai.id,
                    DateDebutResa = r.date_debut,
                    DateFinResa = r.date_fin,
                    NomEquipement = equipement.nom,
                    ZoneEquipement = resaDB.zone.First(z => z.id == equipement.zoneID).nom_zone,
                    NomEquipe = equipeStlo.nom_equipe
                };
                infos.Add(info);
            }
            return infos;
        }

        public organisme ObtenirOrganisme(int IdOrg)
        {
            return resaDB.organisme.First(o => o.id == IdOrg);
        }

        public List<ProvenanceXProj> ListProjXProvenance(int IdProv)
        {
            List<ProvenanceXProj> list = new List<ProvenanceXProj>();
           
            var proj = (from proje in resaDB.projet
                        from prov in resaDB.ld_provenance
                        where proje.provenance == prov.nom_provenance
                        && prov.id == IdProv && proje.date_creation >= DateTime.Now.AddYears(-3)
                        select proje).Distinct().ToList();

            foreach(var p in proj)
            {
                ProvenanceXProj prov = new ProvenanceXProj
                {
                    DateCreation = p.date_creation.ToString(),
                    DescriptProj = p.description_projet,
                    Financement = p.financement,
                    NumProjet = p.num_projet,
                    Organisme = resaDB.organisme.First(o => o.id == p.organismeID).nom_organisme,
                    Provenance = p.provenance,
                    RespProjet = p.mailRespProjet,
                    TitreProj = p.titre_projet,
                    TypeProj = p.type_projet
                };
                list.Add(prov);
            }
            return list;
        }

        public ld_provenance NomProvenance(int IdProvenance)
        {
            return resaDB.ld_provenance.First(p => p.id == IdProvenance);
        }

        public List<ld_provenance> ListeProvenances()
        {
            return resaDB.ld_provenance.ToList();
        }

        public List<ProvenanceXProj> ListProjXNonProv()
        {
            List<ProvenanceXProj> list = new List<ProvenanceXProj>();

            var proj = (from proje in resaDB.projet
                        where proje.provenance == null && proje.date_creation >= DateTime.Now.AddYears(-3)
                        select proje).Distinct().ToList();

            foreach (var p in proj)
            {
                ProvenanceXProj prov = new ProvenanceXProj
                {
                    DateCreation = p.date_creation.ToString(),
                    DescriptProj = p.description_projet,
                    Financement = p.financement,
                    NumProjet = p.num_projet,
                    Organisme = resaDB.organisme.First(o => o.id == p.organismeID).nom_organisme,
                    Provenance = p.provenance,
                    RespProjet = p.mailRespProjet,
                    TitreProj = p.titre_projet,
                    TypeProj = p.type_projet
                };
                list.Add(prov);
            }
            return list;
        }
    }
}
