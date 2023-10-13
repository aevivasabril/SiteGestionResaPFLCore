using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Services.ScheduleTask
{
    public interface IEnqueteTaskDB
    {
        Task<IList<utilisateur>> GetUtilisateurSuperAdminAsync();

        List<enquete> GetEnquetesXFirstTime();

        bool AreEnquetesCreated();

        essai GetEssaiParEnquete(int id);

        projet GetProjetParEnquete(int projetID);

        List<enquete> GetEnquetesPourRelance();

        void UpdateDateEnvoiEnquete(enquete enquete);

        string GetEmailCreatorEssai(int userID);

        void UpdateDateEnvoiEnqueteManuel(enquete enquete);

    }
}
