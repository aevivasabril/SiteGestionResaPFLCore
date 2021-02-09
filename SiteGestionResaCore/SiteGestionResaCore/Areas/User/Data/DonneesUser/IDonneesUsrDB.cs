using SiteGestionResaCore.Areas.User.Data.ResasUser;
using SiteGestionResaCore.Models.EquipementsReserves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.User.Data.DonneesUser
{
    public interface IDonneesUsrDB
    {
        List<InfosResa> ObtenirResasUser(int IdUsr);

        ConsultInfosEssaiChilVM ObtenirInfosEssai(int IdEssai);

        List<InfosResasEquipement> ListEquipVsDonnees(int IdEssai);
    }
}
