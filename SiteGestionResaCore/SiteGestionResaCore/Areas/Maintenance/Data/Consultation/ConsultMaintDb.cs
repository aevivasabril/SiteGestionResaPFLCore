/*
 * website developped to manage the dairy platform STLO operations  
 * Code by Anny VIVAS, inspired from the operationnal functioning of the ancien website developped by Bruno PERRET  
 * July 2021
 * website includes code from:
 *  DotNetZip library for dealing with zip, bsip and zlib from .net 
 *  Created by: Henrik/Dino Chiesa
 * 
 *  MailKit open source library for .NET mail-client 
 *  Created by:  Jeffrey Stedfast
 * 
 *  Microsoft.AspNetCore.Identity.EntityFrameworkCore, ASP.NET Core Identity provider that uses Entity Framework Core
 *  Created by: Microsoft
 *  
 *  Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation, Runtime compilation support for Razor views and Razor pages in ASP.NET Core MVC
 *  Created by: Microsoft
 *
 *  Microsoft.EntityFrameworkCore.Design, Shared design-time components for Entity Framework Core tools
 *  Created by: Microsoft
 *
 *  Microsoft.EntityFrameworkCore.SqlServer, Microsoft SQL Server database provider for Entity Framework Core
 *  Created by: Microsoft
 *
 *  Ncrontab, NCrontab is crontab for all .NET runtimes supported by .NET Standard 1.0. It provides parsing and formatting of crontab expressions as well as calculation of occurrences of time based on a schedule expressed in the crontab format
 *  Created by: Atif Aziz
 *   
 * This projet is released under the terms of the GNU general public license GPL version 3 or later:
 * availaible here: https://www.gnu.org/licenses/gpl-3.0-standalone.html
 * 
 * Copyright (c) 2021-2024 Anny Vivas
 */

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

            // Bout de code pour mettre les operations "resa_maint_equip_adjacent" et "reservation_maintenance" en coherence avec la variable 'intervention_finie' du tableau "maintenance"
            // maintenant chaque operation par équipement est finalisée ou pas donc independante de l'état général de la maintenance
            // A EXECUTER UNE SEUL FOIS! 
            /*List<maintenance> maintenances = context.maintenance.Where(m => m.maintenance_finie == true).Distinct().ToList();
            foreach ( var maint in maintenances)
            {
                // obtenir les maintenances sur des équipements PFL et les mettre à jour
                var interventions = context.reservation_maintenance.Where(m => m.maintenanceID == maint.id).Distinct().ToList();
                foreach ( var oper in interventions)
                {
                    oper.interv_fini = true;
                    oper.actions_realisees = "Auto: intervention fini";
                    context.SaveChanges();
                }

                var interv_adjacentes = context.resa_maint_equip_adjacent.Where(r => r.maintenanceID == maint.id).Distinct().ToList();
                foreach (var oper in interv_adjacentes)
                {
                    oper.interv_fini = true;
                    oper.actions_realisees = "Auto: intervention fini";
                    context.SaveChanges();
                }
            }*/
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
                        NumGMAO = context.equipement.First(e => e.id == inter.equipementID).numGmao,
                        ActionMainte = inter.actions_realisees
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
                        NomIntervExterne = NomIntervExt,
                        ActionMainte = inter.actions_realisees
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

                if (maint.maintenance_supprime == true)
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

                if (maint.maintenance_supprime == true)
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
