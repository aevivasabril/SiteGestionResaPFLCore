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

        bool AjouterCapteur(string NomCapteur,string CodeCapteur, int SelectedPiloteID, DateTime DateProchaineVerifInt, DateTime DateProchaineVerifExt,
                    DateTime DateDerniereVerifInt, DateTime DateDerniereVerifExt, double periodInt, double periodExt, bool CapteurConforme, double EmtCapteur, double FacteurCorrectif);
        capteur ObtenirCapteur(int id);

        bool SupprimerCapteur(int idCapteur);

        equipement ObtenirEquipement(int idEquipement);

        bool UpdatePeriodiciteInt(capteur Capteur, double periodicite);

        bool UpdatePeriodiciteExt(capteur Capteur, double periodicite);

        bool UpdateDateProVerifInt(capteur capt, DateTime dateverif);

        bool UpdateDateProVerifExt(capteur capt, DateTime dateverif);

        bool UpdateDateDerniereVerifInt(capteur capt, DateTime dateverif);
        bool UpdateDateDerniereVerifExt(capteur capt, DateTime dateverif);

        bool UpdateFacteur(capteur capt, double facteur);

        bool UpdateEMT(capteur capt, double emt);

        bool UpdateCodeCapteur(capteur capt, string code);

        bool UpdateNomCapteur(capteur capt, string nom);

        bool UpdateConformite(capteur capt, bool conformite);
        double FacteurCorrectif(capteur capt);
    }
}
