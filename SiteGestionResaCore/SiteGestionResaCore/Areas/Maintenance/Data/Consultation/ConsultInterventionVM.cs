using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Consultation
{
    public class ConsultInterventionVM
    {
        public List<InfosInterventions> ListIntervDansPFL { get; set; }

        public List<InfosInterventions> ListIntervSansZone { get; set; }
    }
}
