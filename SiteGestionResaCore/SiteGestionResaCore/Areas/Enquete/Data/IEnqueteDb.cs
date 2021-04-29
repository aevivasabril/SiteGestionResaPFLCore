using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Data
{
    public interface IEnqueteDb
    {
        public essai ObtenirEssai(int essaiID);

        public projet ObtenirProjet(essai ess);
    }
}
