using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data.Validation
{
    public interface IResaAValiderDb
    {
        List<AffichageResasAValider> ResasAValider();
    }
}
