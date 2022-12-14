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
        capteur ObtenirCapteur(int id);

        bool SupprimerCapteur(int idCapteur);

        equipement ObtenirEquipement(int idEquipement);

        bool UpdatePeriodicite(capteur Capteur, double periodicite);

        bool UpdateDateProVerif(capteur capt, DateTime dateverif);

        bool UpdateDateDerniereVerif(capteur capt, DateTime dateverif);

        bool UpdateFacteur(capteur capt, double facteur);

        bool UpdateEMT(capteur capt, double emt);

        bool UpdateCodeCapteur(capteur capt, string code);

        bool UpdateNomCapteur(capteur capt, string nom);

        bool UpdateConformite(capteur capt, bool conformite);
        double FacteurCorrectif(capteur capt);
    }
}
