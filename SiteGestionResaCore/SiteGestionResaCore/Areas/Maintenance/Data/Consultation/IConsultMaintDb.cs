using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Consultation
{
    public interface IConsultMaintDb
    {
        List<InfosIntervDansPFL> ListIntervPFL();
        List<InfosIntervSansZone> ListIntervSansZones();
    }
}
