using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Services.ScheduleTask
{
    public interface IEntrepotTaskDB
    {
        public List<projet> GetProjetsXNotification();
        public List<projet> GetProjetsXSuppression();
        public bool SupprimerEntrepotXProj(projet pr); 
    }
}
