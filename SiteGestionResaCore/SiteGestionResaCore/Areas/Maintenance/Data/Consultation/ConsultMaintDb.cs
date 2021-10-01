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

        public List<InfosIntervDansPFL> ListIntervPFL()
        {
            List<InfosIntervDansPFL> ListPFL = new List<InfosIntervDansPFL>();
            string NomIntervExt = "";
            var intervs = context.reservation_maintenance.Where(r => r.date_debut.Year == DateTime.Today.Year || r.date_fin.Year == DateTime.Today.Year);
            foreach (var inter in intervs.OrderByDescending(e => e.date_debut))
            {
                maintenance maint = context.maintenance.First(m => m.id == inter.maintenanceID);
                if (maint.intervenant_externe != false)
                {
                    NomIntervExt = maint.nom_intervenant_ext;
                }
                else
                {
                    NomIntervExt = "";
                }
                InfosIntervDansPFL info = new InfosIntervDansPFL
                {
                    DateDebut = inter.date_debut,
                    DateFin = inter.date_fin,
                    CodeMaint = maint.code_operation,
                    DescriptifMaint = context.maintenance.First(m => m.id == inter.maintenanceID).description_operation,
                    IdResMaint = inter.id,
                    NomEquipement = context.equipement.First(e => e.id == inter.equipementID).nom,
                    OperateurPFL = context.Users.First(u => u.Id == context.maintenance.First(m => m.id == inter.maintenanceID).userID).Email,
                    TypeMaintenance = context.maintenance.First(m => m.id == inter.maintenanceID).type_maintenance,
                    NomIntervExterne = NomIntervExt
                };
                ListPFL.Add(info);
            }
            return ListPFL;
        }

        public List<InfosIntervSansZone> ListIntervSansZones()
        {
            List<InfosIntervSansZone> ListSansZone = new List<InfosIntervSansZone>();
            string NomIntervExt = "";
            // Filtrer les opérations pour l'année en cours! éviter la surcharge du tableau
            var intervs = context.resa_maint_equip_adjacent.Where(r => r.date_debut.Year == DateTime.Today.Year || r.date_fin.Year == DateTime.Today.Year);
            foreach (var inter in intervs.OrderByDescending(e => e.date_debut))
            {
                maintenance maint = context.maintenance.First(m => m.id == inter.maintenanceID);
                if (maint.intervenant_externe != false)
                {
                    NomIntervExt = maint.nom_intervenant_ext;
                }
                else
                {
                    NomIntervExt = "";
                }
                InfosIntervSansZone info = new InfosIntervSansZone
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
            return ListSansZone;
        }
    }
}
