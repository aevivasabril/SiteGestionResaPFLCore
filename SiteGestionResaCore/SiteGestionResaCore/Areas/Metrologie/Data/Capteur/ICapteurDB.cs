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

        bool AjouterCapteur(string NomCapteur,string CodeCapteur, int SelectedPiloteID, DateTime DateProchaineVerif,
                    DateTime DateDernierVerif, double period, bool CapteurConforme, double EmtCapteur, double FacteurCorrectif);
    }
}
