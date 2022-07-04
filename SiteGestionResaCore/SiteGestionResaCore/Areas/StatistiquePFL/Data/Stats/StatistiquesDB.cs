using Microsoft.AspNetCore.Identity;
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
                                 select equipe).FirstOrDefault();

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
                    if(equipeStlo == null)
                    {
                        info = new InfosReservations
                        {
                            IdEssai = ess.id,
                            DateCreation = ess.date_creation,
                            TitreEssai = ess.titreEssai,
                            NomEquipe = null,
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
                    }
                    else
                    {
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
                    }                    
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
                            else if (diff.Hours >= 5 && diff.Days >= 1) // réservation qui peut se prolonger sur plusieurs jours et pouvant aller même le weekeend
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

        /// <summary>
        /// Liste des projets où il y a au moins une réservation valide (non supprimé ou non refusée)
        /// </summary>
        /// <returns></returns>
        public List<projet> ObtenirListProjet()
        {
            return  (from pr in resaDB.projet
                        from ess in resaDB.essai
                        where pr.id == ess.projetID && ess.resa_supprime != true && ess.resa_refuse != true
                        select pr).Distinct().OrderByDescending(p => p.date_creation).ToList();
            //return resaDB.projet.Distinct().OrderByDescending(p => p.date_creation).ToList();
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
                              select equipe).FirstOrDefault();

            foreach (var x in essaisXProjet)
            {
                var reservs = resaDB.reservation_projet.Where(r => r.essaiID == x.id);
                foreach(var res in reservs)
                {
                    var equipement = resaDB.equipement.First(e => e.id == res.equipementID);

                    if(equipeStlo == null)
                    {
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
                            NomEquipe = null
                        };
                    }
                    else
                    {
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
                    }                   
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
                                  select equipe).FirstOrDefault();

                var organisme = resaDB.organisme.First(o => o.id == projet.organismeID);
                var equipement = resaDB.equipement.First(e => e.id == r.equipementID);

                if(equipeStlo == null)
                {
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
                        NomEquipe = null
                    };
                }
                else
                {
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
                }
               
                infos.Add(info);
            }
            return infos;
        }

        public organisme ObtenirOrganisme(int IdOrg)
        {
            return resaDB.organisme.First(o => o.id == IdOrg);
        }

        public List<CategorieXProj> ListProjXProvenance(int IdProv)
        {
            List<CategorieXProj> list = new List<CategorieXProj>();
           
            var proj = (from proje in resaDB.projet
                        from prov in resaDB.ld_provenance
                        where proje.provenance == prov.nom_provenance
                        && prov.id == IdProv && proje.date_creation >= DateTime.Now.AddYears(-3)
                        select proje).Distinct().ToList();

            foreach(var p in proj)
            {
                CategorieXProj prov = new CategorieXProj
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

        public List<CategorieXProj> ListProjXNonProv()
        {
            List<CategorieXProj> list = new List<CategorieXProj>();

            var proj = (from proje in resaDB.projet
                        where proje.provenance == null && proje.date_creation >= DateTime.Now.AddYears(-3)
                        select proje).Distinct().ToList();

            foreach (var p in proj)
            {
                CategorieXProj prov = new CategorieXProj
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

        public int LaitAnneeEnCours()
        {
            // Filtrer pour le type de produit LAIT
            var Today = DateTime.Now;
            int compteur = 0;
            var essais = (from e in resaDB.essai
                           from r in resaDB.reservation_projet
                           where r.date_fin <= Today && r.date_fin.Year == Today.Year && r.date_debut.Year == Today.Year
                           && e.resa_supprime != true && e.resa_refuse != true && e.id == r.essaiID
                           && e.type_produit_entrant == "Lait"
                           select e).Distinct().ToList();
            // calculer la quantité de lait
            foreach (var ess in essais)
            {
                if(ess.quantite_produit != null)
                    compteur += ess.quantite_produit.Value;
            }
            return compteur;
        }

        public int LaitXDates(DateTime dateDebut, DateTime dateFin)
        {
            var Today = DateTime.Now;
            int compteur = 0;
            var essais = (from e in resaDB.essai
                          from r in resaDB.reservation_projet
                          where r.date_fin <= dateFin && r.date_debut >= dateDebut
                          && e.resa_supprime != true && e.resa_refuse != true && e.id == r.essaiID
                          select e).Distinct().ToList();
            // calculer la quantité de lait
            foreach (var ess in essais)
            {
                if (ess.quantite_produit != null)
                    compteur = compteur + ess.quantite_produit.Value;
            }
            return compteur;
        }

        public List<MaintenanceInfos> ListMaintenances(DateTime dateDu, DateTime dateAu)
        {
            List<MaintenanceInfos> list = new List<MaintenanceInfos>();

            #region Pour les maintenances des équipements réservables 

            var resas = (from m in resaDB.maintenance
                          from resaM in resaDB.reservation_maintenance
                          where resaM.date_debut >= dateDu && resaM.date_fin <= dateAu && resaM.maintenanceID == m.id
                          select resaM).Distinct().ToList();

            foreach(var m in resas)
            {
                var maint = resaDB.maintenance.First(ma => ma.id == m.maintenanceID);
                var user = resaDB.Users.First(u => u.Id == maint.userID);
                var equip = resaDB.equipement.First(e => e.id == m.equipementID);
                
                MaintenanceInfos info = new MaintenanceInfos
                {
                    CodeMaintenance = maint.code_operation,
                    TypeMaintenance = maint.type_maintenance,
                    MailOperateur = user.Email,
                    IntervenantExt = maint.intervenant_externe,
                    NomIntervExt = maint.nom_intervenant_ext,
                    DescripOperation = maint.description_operation,
                    MaintSupprimee = maint.maintenance_supprime.HasValue,
                    DateSuppression = maint.date_suppression,
                    RaisonSuppression = maint.raison_suppression,
                    MaintTerminee = maint.maintenance_finie.HasValue,
                    NomEquipement = equip.nom,
                    DateDebut = m.date_debut,
                    DateFin = m.date_fin,
                    ZoneAffectee = resaDB.zone.First(z => z.id == equip.zoneID).nom_zone
                };

                list.Add(info);
            }

            #endregion

            #region Pour les maintenances des équipements communs
            var maintscomm = (from m in resaDB.maintenance
                          from resaM in resaDB.resa_maint_equip_adjacent
                          where resaM.date_debut >= dateDu && resaM.date_fin <= dateAu && resaM.maintenanceID == m.id
                          select resaM).Distinct().ToList();

            foreach(var m in maintscomm)
            {
                var maint = resaDB.maintenance.First(ma => ma.id == m.maintenanceID);
                var user = resaDB.Users.First(u => u.Id == maint.userID);
                MaintenanceInfos info = new MaintenanceInfos
                {
                    CodeMaintenance = maint.code_operation,
                    TypeMaintenance = maint.type_maintenance,
                    MailOperateur = user.Email,
                    IntervenantExt = maint.intervenant_externe,
                    NomIntervExt = maint.nom_intervenant_ext,
                    DescripOperation = maint.description_operation,
                    MaintSupprimee = maint.maintenance_supprime.HasValue,
                    DateSuppression = maint.date_suppression,
                    RaisonSuppression = maint.raison_suppression,
                    MaintTerminee = maint.maintenance_finie.HasValue,
                    NomEquipement = m.nom_equipement,
                    DateDebut = m.date_debut,
                    DateFin = m.date_fin,
                    ZoneAffectee = m.zone_affectee
                };
                list.Add(info);
            }
            #endregion

            return list;

        }

        public List<ld_type_projet> ListTypeProjet()
        {
            return resaDB.ld_type_projet.ToList();
        }
        
        public List<CategorieXProj> ListProjetXType(int IdType)
        {
            List<CategorieXProj> list = new List<CategorieXProj>();

            var proj = (from proje in resaDB.projet
                        from typP in resaDB.ld_type_projet
                        where proje.type_projet == typP.nom_type_projet
                        && typP.id == IdType && proje.date_creation >= DateTime.Now.AddYears(-3)
                        select proje).Distinct().ToList();

            foreach (var p in proj)
            {
                CategorieXProj prov = new CategorieXProj
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

        public ld_type_projet NomTypeProj(int Id)
        {
            return resaDB.ld_type_projet.First(t => t.id == Id);
        }

        public List<CategorieXProj> ListProjsSansType()
        {
            List<CategorieXProj> list = new List<CategorieXProj>();

            var proj = (from proje in resaDB.projet
                        where proje.type_projet == null
                        && proje.date_creation >= DateTime.Now.AddYears(-3)
                        select proje).Distinct().ToList();

            foreach (var p in proj)
            {
                CategorieXProj prov = new CategorieXProj
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

        public List<ld_produit_in> ListProdsEntree()
        {
            return resaDB.ld_produit_in.ToList();
        }

        public List<EssaiXprod> ListEssaisXprod(int idprod)
        {
            List<EssaiXprod> list = new List<EssaiXprod>();

            var essais = (from ess in resaDB.essai
                        from typP in resaDB.ld_produit_in
                        from pr in resaDB.projet
                        where ess.type_produit_entrant == typP.nom_produit_in && typP.id == idprod
                        && pr.date_creation >= DateTime.Now.AddYears(-3) && ess.resa_refuse != true && ess.resa_supprime != true
                        select ess).Distinct().ToList();

            foreach (var p in essais)
            {
                var projet = resaDB.projet.First(pr => pr.id == p.projetID);
                EssaiXprod prov = new EssaiXprod
                {
                    TitreEssai = p.titreEssai,
                    AuteurEssai = resaDB.Users.First(u=>u.Id == p.compte_userID).Email,
                    IdEssai = p.id,
                    PrecisionProd = p.precision_produit,
                    QuantiteProd = p.quantite_produit.GetValueOrDefault(),
                    DestProdFinal = p.destination_produit,
                    DateCreationEssai = p.date_creation,
                    NumProjet = projet.num_projet,
                    TitreProjet = projet.titre_projet
                };
                list.Add(prov);
            }
            return list;
        }

        public string NomTypeProd(int idprod)
        {
            return resaDB.ld_produit_in.First(p => p.id == idprod).nom_produit_in;
        }

        public List<EssaiXprod> ListEssaisSansprod()
        {
            List<EssaiXprod> list = new List<EssaiXprod>();

            var essais = (from ess in resaDB.essai
                          from pr in resaDB.projet
                          where ess.type_produit_entrant == null 
                          && pr.date_creation >= DateTime.Now.AddYears(-3) && ess.resa_refuse != true && ess.resa_supprime != true
                          select ess).Distinct().ToList();

            foreach (var p in essais)
            {
                try
                {
                    var projet = resaDB.projet.First(pr => pr.id == p.projetID);
                    EssaiXprod prov = new EssaiXprod
                    {
                        TitreEssai = p.titreEssai,
                        AuteurEssai = resaDB.Users.First(u => u.Id == p.compte_userID).Email,
                        IdEssai = p.id,
                        PrecisionProd = p.precision_produit,
                        QuantiteProd = p.quantite_produit.GetValueOrDefault(),
                        DestProdFinal = p.destination_produit,
                        DateCreationEssai = p.date_creation,
                        NumProjet = projet.num_projet,
                        TitreProjet = projet.titre_projet
                    };
                    list.Add(prov);
                }
                catch (Exception e)
                {
                    logger.LogError("", "Problème pour initialiser un des objets EssaiXProd");
                }               
            }
            return list;
        }

    }
}
