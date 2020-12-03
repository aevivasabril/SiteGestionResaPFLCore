using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.ResasUser
{
    public interface IResasUserDB
    {
        List<InfosResasUser> ObtenirResasUser(int IdUsr, string openPartial, int IdEssai);

        essai ObtenirEssaiPourModif(int IdEssai);
    }
}
