using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Consultation
{
    public interface IConsultMaintDb
    {
        List<InfosInterventions> ListIntervPFLEnCours();
        List<InfosInterventions> ListIntervSansZonesEnCours();
        List<InfosInterventions> ListIntervPFLFinies();
        List<InfosInterventions> ListIntervSansZoneFinies();
        List<InfosInterventions> ListIntervPFLSupp();
        List<InfosInterventions> ListIntervSansZoneSupp();
    }
}
