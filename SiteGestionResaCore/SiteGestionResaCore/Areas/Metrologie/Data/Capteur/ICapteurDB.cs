using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.Capteur
{
    public interface ICapteurDB
    {
        List<CapteurXAffichage> ObtenirListCapteurs();

        List<equipement> ObtListEquipements();

        bool AjouterCapteur(AjouterCapteurVM model);
    }
}
