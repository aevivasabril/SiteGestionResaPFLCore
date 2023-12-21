using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Consultation
{
    public class ConsultMaintDb: IConsultMaintDb
    {
        private readonly GestionResaContext context;
        private readonly ILogger<ConsultMaintDb> logger;
        private readonly UserManager<utilisateur> userManager;

        public ConsultMaintDb(
            GestionResaContext context,
            ILogger<ConsultMaintDb> logger,
            UserManager<utilisateur> userManager)
        {
            this.context = context;
            this.logger = logger;
            this.userManager = userManager;
        }

        public List<InfosInterventions> ListIntervPFLEnCours()
        {
            List<InfosInterventions> ListPFL = new List<InfosInterventions>();
            string NomIntervExt = "";

            // récupérer uniquement les interventions des 12 derniers mois
            DateTime DateSeuil = DateTime.Now.AddYears(-1);

            var intervs = context.reservation_maintenance.Where(r => (r.date_debut >= DateSeuil || r.date_fin >= DateSeuil));
            foreach (var inter in intervs.OrderByDescending(e => e.date_debut))
            {
                maintenance maint = context.maintenance.First(m => m.id == inter.maintenanceID);

                if(maint.maintenance_supprime != true && inter.interv_fini != true)
                {
                    if (maint.intervenant_externe != false)
                    {
                        NomIntervExt = maint.nom_intervenant_ext;
                    }
                    else
                    {
                        NomIntervExt = "";
                    }
                    InfosInterventions info = new InfosInterventions
                    {
                        DateDebut = inter.date_debut,
                        DateFin = inter.date_fin,
                        CodeMaint = maint.code_operation,
                        DescriptifMaint = context.maintenance.First(m => m.id == inter.maintenanceID).description_operation,
                        IdResMaint = inter.id,
                        NomEquipement = context.equipement.First(e => e.id == inter.equipementID).nom,
                        OperateurPFL = context.Users.First(u => u.Id == context.maintenance.First(m => m.id == inter.maintenanceID).userID).Email,
                        TypeMaintenance = context.maintenance.First(m => m.id == inter.maintenanceID).type_maintenance,
                        NomIntervExterne = NomIntervExt,
                        NumGMAO = context.equipement.First(e => e.id == inter.equipementID).numGmao
                    };
                    ListPFL.Add(info);
                }            
            }
            return ListPFL;
        }

        public List<InfosInterventions> ListIntervSansZonesEnCours()
        {
            List<InfosInterventions> ListSansZone = new List<InfosInterventions>();
            string NomIntervExt = "";

            // récupérer uniquement les interventions des 12 derniers mois
            DateTime DateSeuil = DateTime.Now.AddYears(-1);

            // Filtrer les opérations pour l'année en cours! éviter la surcharge du tableau
            var intervs = context.resa_maint_equip_adjacent.Where(r => (r.date_debut >= DateSeuil|| r.date_fin >= DateSeuil));

            foreach (var inter in intervs.OrderByDescending(e => e.date_debut))
            {
                maintenance maint = context.maintenance.First(m => m.id == inter.maintenanceID);

                if(maint.maintenance_supprime != true && inter.interv_fini != true)
                {
                    if (maint.intervenant_externe != false)
                    {
                        NomIntervExt = maint.nom_intervenant_ext;
                    }
                    else
                    {
                        NomIntervExt = "";
                    }
                    InfosInterventions info = new InfosInterventions
                    {
                        DateDebut = inter.date_debut,
                        DateFin = inter.date_fin,
                        DescriptifMaint = context.maintenance.First(m => m.id == inter.maintenanceID).description_operation,
                        IdResMaint = inter.id,
                        CodeMaint = maint.code_operation,
                        NomEquipement = inter.nom_equipement,
                        ZoneAffecte = inter.zone_affectee,
                        OperateurPFL = context.Users.First(u => u.Id == context.maintenance.First(m => m.id == inter.maintenanceID).userID).Email,
                        TypeMaintenance = context.maintenance.First(m => m.id == inter.maintenanceID).type_maintenance,
                        NomIntervExterne = NomIntervExt
                    };

                    ListSansZone.Add(info);
                }                
            }
            return ListSansZone;
        }
        public List<InfosInterventions> ListIntervPFLFinies()
        {
            List<InfosInterventions> ListPFLFinies = new List<InfosInterventions>();
            string NomIntervExt = "";

            // récupérer uniquement les interventions des 12 derniers mois
            DateTime DateSeuil = DateTime.Now.AddYears(-1);

            var intervs = context.reservation_maintenance.Where(r => (r.date_debut >= DateSeuil || r.date_fin >= DateSeuil));
            foreach (var inter in intervs.OrderByDescending(e => e.date_debut))
            {
                maintenance maint = context.maintenance.First(m => m.id == inter.maintenanceID);

                if (maint.maintenance_supprime != true && inter.interv_fini == true)
                {
                    if (maint.intervenant_externe != false)
                    {
                        NomIntervExt = maint.nom_intervenant_ext;
                    }
                    else
                    {
                        NomIntervExt = "";
                    }
                    InfosInterventions info = new InfosInterventions
                    {
                        DateDebut = inter.date_debut,
                        DateFin = inter.date_fin,
                        CodeMaint = maint.code_operation,
                        DescriptifMaint = context.maintenance.First(m => m.id == inter.maintenanceID).description_operation,
                        IdResMaint = inter.id,
                        NomEquipement = context.equipement.First(e => e.id == inter.equipementID).nom,
                        OperateurPFL = context.Users.First(u => u.Id == context.maintenance.First(m => m.id == inter.maintenanceID).userID).Email,
                        TypeMaintenance = context.maintenance.First(m => m.id == inter.maintenanceID).type_maintenance,
                        NomIntervExterne = NomIntervExt,
                        NumGMAO = context.equipement.First(e => e.id == inter.equipementID).numGmao
                    };
                    ListPFLFinies.Add(info);
                }
            }
            return ListPFLFinies;
        }
        public List<InfosInterventions> ListIntervSansZoneFinies()
        {
            List<InfosInterventions> ListSansZoneFinies = new List<InfosInterventions>();
            string NomIntervExt = "";

            // récupérer uniquement les interventions des 12 derniers mois
            DateTime DateSeuil = DateTime.Now.AddYears(-1);

            // Filtrer les opérations pour l'année en cours! éviter la surcharge du tableau
            var intervs = context.resa_maint_equip_adjacent.Where(r => (r.date_debut >= DateSeuil || r.date_fin >= DateSeuil));

            foreach (var inter in intervs.OrderByDescending(e => e.date_debut))
            {
                maintenance maint = context.maintenance.First(m => m.id == inter.maintenanceID);

                if (maint.maintenance_supprime != true && inter.interv_fini == true)
                {
                    if (maint.intervenant_externe != false)
                    {
                        NomIntervExt = maint.nom_intervenant_ext;
                    }
                    else
                    {
                        NomIntervExt = "";
                    }
                    InfosInterventions info = new InfosInterventions
                    {
                        DateDebut = inter.date_debut,
                        DateFin = inter.date_fin,
                        DescriptifMaint = context.maintenance.First(m => m.id == inter.maintenanceID).description_operation,
                        IdResMaint = inter.id,
                        CodeMaint = maint.code_operation,
                        NomEquipement = inter.nom_equipement,
                        ZoneAffecte = inter.zone_affectee,
                        OperateurPFL = context.Users.First(u => u.Id == context.maintenance.First(m => m.id == inter.maintenanceID).userID).Email,
                        TypeMaintenance = context.maintenance.First(m => m.id == inter.maintenanceID).type_maintenance,
                        NomIntervExterne = NomIntervExt
                    };

                    ListSansZoneFinies.Add(info);
                }
            }
            return ListSansZoneFinies;
        }

        public List<InfosInterventions> ListIntervPFLSupp()
        {
            List<InfosInterventions> ListPFLSupp = new List<InfosInterventions>();
            string NomIntervExt = "";

            // récupérer uniquement les interventions des 12 derniers mois
            DateTime DateSeuil = DateTime.Now.AddYears(-1);

            var intervs = context.reservation_maintenance.Where(r => (r.date_debut >= DateSeuil || r.date_fin >= DateSeuil));
            foreach (var inter in intervs.OrderByDescending(e => e.date_debut))
            {
                maintenance maint = context.maintenance.First(m => m.id == inter.maintenanceID);

                if (maint.maintenance_supprime == true && inter.interv_fini != true)
                {
                    if (maint.intervenant_externe != false)
                    {
                        NomIntervExt = maint.nom_intervenant_ext;
                    }
                    else
                    {
                        NomIntervExt = "";
                    }
                    InfosInterventions info = new InfosInterventions
                    {
                        DateDebut = inter.date_debut,
                        DateFin = inter.date_fin,
                        CodeMaint = maint.code_operation,
                        DescriptifMaint = context.maintenance.First(m => m.id == inter.maintenanceID).description_operation,
                        IdResMaint = inter.id,
                        NomEquipement = context.equipement.First(e => e.id == inter.equipementID).nom,
                        OperateurPFL = context.Users.First(u => u.Id == context.maintenance.First(m => m.id == inter.maintenanceID).userID).Email,
                        TypeMaintenance = context.maintenance.First(m => m.id == inter.maintenanceID).type_maintenance,
                        NomIntervExterne = NomIntervExt,
                        NumGMAO = context.equipement.First(e => e.id == inter.equipementID).numGmao,
                        RaisonSupp = maint.raison_suppression,
                        DateSuppression = maint.date_suppression.Value
                    };
                    ListPFLSupp.Add(info);
                }
            }
            return ListPFLSupp;
        }

        public List<InfosInterventions> ListIntervSansZoneSupp()
        {
            List<InfosInterventions> ListSansZoneSupp = new List<InfosInterventions>();
            string NomIntervExt = "";

            // récupérer uniquement les interventions des 12 derniers mois
            DateTime DateSeuil = DateTime.Now.AddYears(-1);

            // Filtrer les opérations pour l'année en cours! éviter la surcharge du tableau
            var intervs = context.resa_maint_equip_adjacent.Where(r => (r.date_debut >= DateSeuil || r.date_fin >= DateSeuil));

            foreach (var inter in intervs.OrderByDescending(e => e.date_debut))
            {
                maintenance maint = context.maintenance.First(m => m.id == inter.maintenanceID);

                if (maint.maintenance_supprime == true && inter.interv_fini != true)
                {
                    if (maint.intervenant_externe != false)
                    {
                        NomIntervExt = maint.nom_intervenant_ext;
                    }
                    else
                    {
                        NomIntervExt = "";
                    }
                    InfosInterventions info = new InfosInterventions
                    {
                        DateDebut = inter.date_debut,
                        DateFin = inter.date_fin,
                        DescriptifMaint = context.maintenance.First(m => m.id == inter.maintenanceID).description_operation,
                        IdResMaint = inter.id,
                        CodeMaint = maint.code_operation,
                        NomEquipement = inter.nom_equipement,
                        ZoneAffecte = inter.zone_affectee,
                        OperateurPFL = context.Users.First(u => u.Id == context.maintenance.First(m => m.id == inter.maintenanceID).userID).Email,
                        TypeMaintenance = context.maintenance.First(m => m.id == inter.maintenanceID).type_maintenance,
                        RaisonSupp = maint.raison_suppression,
                        DateSuppression = maint.date_suppression.Value
                    };

                    ListSansZoneSupp.Add(info);
                }
            }
            return ListSansZoneSupp;
        }
    }
}
