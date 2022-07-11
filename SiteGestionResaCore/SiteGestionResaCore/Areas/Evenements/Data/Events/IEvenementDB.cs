using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Evenements.Data
{
    public interface IEvenementDB
    {
        List<evenement> ListEvenements();

        bool AjoutEvent(string message);

        bool SupprimerEvenement(int idEvent);
    }
}
