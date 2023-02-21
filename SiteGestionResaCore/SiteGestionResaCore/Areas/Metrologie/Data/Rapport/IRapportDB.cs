using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.Rapport
{
    public interface IRapportDB
    {
        IList<EquipVsCapteur> GetEquipements();

        capteur ObtenirCapteur(int idCapteur);

        Task<IList<utilisateur>> ObtenirAdminUsrs();

        string NomEquipementXCapteur(int idEquipement);

        bool CreerRapportMetrologie(byte[] data, string nomDoc, int idCapteur, DateTime dateVerif, string TypeRapport);

        bool majCapteurxRapport(bool IsCaptConform, double FacteurCorrectif, DateTime DateVerifMetro, int IdCapteur, string typeMetro);

        List<CapteurXRapport> ListRapports();

        rapport_metrologie ObtenirRapport(int idRapport);
    }
}
