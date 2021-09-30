using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Consultation
{
    public class ConsultInterventionVM
    {
        public List<InfosIntervDansPFL> ListIntervDansPFL { get; set; }

        public List<InfosIntervSansZone> ListIntervSansZone { get; set; }
    }
}
