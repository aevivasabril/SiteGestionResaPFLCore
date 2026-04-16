using SiteGestionResaCore.Areas.User.Data.DataPcVue;
using SiteGestionResaCore.Areas.User.Data.ResasUser;
using SiteGestionResaCore.Data;
using SiteGestionResaCore.Models;
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

        ConsultInfosEssaiChildVM ObtenirInfosEssai(int IdEssai);

        List<InfosResasEquipement> ListEquipVsDonnees(int IdEssai);

        AllDataPcVue ObtenirDonneesPcVue(int idResa);

        AllDataPcVue ObtDataConsoElectPFL(DateTime dateDu, DateTime dateAu);

        List<InfosCompteursXEquipResa> ListCompteursXEquipResa(List<InfosResasEquipement> ListResasEquipement);

    }
}
