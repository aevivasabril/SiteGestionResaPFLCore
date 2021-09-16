using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Maintenance
{
    public class EquipementDansZone
    {
        public List<essai> EssaisXAnnulation { get; set; }

        public int IdEquipementXIntervention { get; set; }

        public DateTime DateDebutInterv { get; set; }

        public DateTime DateFinInterv { get; set; }

        //public int ZoneEquipIntervId { get; set; }
    }
}
