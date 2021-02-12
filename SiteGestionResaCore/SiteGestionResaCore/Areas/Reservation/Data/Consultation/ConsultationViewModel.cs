using SiteGestionResaCore.Areas.Reservation.Data.Validation;
using SiteGestionResaCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data.Consultation
{
    public class ConsultationViewModel
    {
        public IList<InfosResasValid> ResasValid { get; set; }

        public InfosProjet InfosProjet { get; set; }

        public ConsultInfosEssaiChildVM InfosEssai { get; set; }

        public List<InfosReservation> Reservations { get; set; }
    }
}
