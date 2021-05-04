using Microsoft.AspNetCore.Identity;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Services.ScheduleTask
{
    public class EnqueteTaskDB: IEnqueteTaskDB
    {
        private readonly UserManager<utilisateur> userManager;

        public EnqueteTaskDB(
            UserManager<utilisateur> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IList<utilisateur>> GetUtilisateurSuperAdminAsync()
        {
            return await userManager.GetUsersInRoleAsync("MainAdmin");
        }
    }
}
