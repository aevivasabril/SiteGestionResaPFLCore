using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data.Validation
{
    public class ResasPourValidationViewModel
    {
        public IList<InfosAffichage> resasAValider { get; set; }

        public InfosEssai infosEssai { get; set; }

    }
}
