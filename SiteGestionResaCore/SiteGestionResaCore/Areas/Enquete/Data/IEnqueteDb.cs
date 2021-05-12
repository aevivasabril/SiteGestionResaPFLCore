using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Data
{
    public interface IEnqueteDb
    {
        essai ObtenirEssai(int essaiID);

        projet ObtenirProjet(essai ess);

        //essai ObtenirEssaiXEnquete(int IdEnquete);

        void UpdateEnqueteWithResponse(string reponse, enquete enquete);

        enquete ObtenirEnqueteFromEssai(int IdEssai);
    }
}
