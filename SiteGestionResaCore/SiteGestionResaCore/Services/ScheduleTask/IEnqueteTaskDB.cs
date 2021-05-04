using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Services.ScheduleTask
{
    public interface IEnqueteTaskDB
    {
        Task<IList<utilisateur>> GetUtilisateurSuperAdminAsync();
    }
}
