﻿using Microsoft.AspNetCore.Mvc.Rendering;
using SiteGestionResaCore.Areas.Equipe.Data;
using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data
{
    public interface IEquipeResaDb
    {
        Task<listAutresUtilisateurs> ObtenirListAutresAsync();

        List<utilisateur> ObtenirListAdmins();

        Task<IList<utilisateur>> ObtenirUsersLogisticAsync();

        utilisateur ObtenirUtilisateur(int id);

        Task ChangeAccesToUser(int id);

        Task ChangeAccesToAdminAsync(int id);

        Task AddAdminToLogisticRoleAsync(int id);

        Task RemoveLogisticRoleAsync(int id);

        void ValidateAccount(int id);

    }
}
