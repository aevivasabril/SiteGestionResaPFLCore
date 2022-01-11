using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public class maintenance
    {
        public maintenance()
        {
            reservation_maintenance = new HashSet<reservation_maintenance>();
            resa_maint_equip_adjacent = new HashSet<resa_maint_equip_adjacent>();
        }

        public int id { get; set; }
        public string type_maintenance { get; set; }
        public string code_operation { get; set; }
        public int userID { get; set; }
        public bool intervenant_externe { get; set; }
        public string? nom_intervenant_ext { get; set; }
        public string description_operation { get; set; }
        public bool? maintenance_supprime { get; set; }
        public bool? maintenance_finie { get; set; }
        public DateTime? date_suppression { get; set; }

        public string? raison_suppression { get; set; }
        public DateTime? date_saisie { get; set; }

        public virtual utilisateur utilisateur { get; set; }

        public virtual ICollection<reservation_maintenance> reservation_maintenance { get; set; }
        public virtual ICollection<resa_maint_equip_adjacent> resa_maint_equip_adjacent { get; set; }


    }
}
