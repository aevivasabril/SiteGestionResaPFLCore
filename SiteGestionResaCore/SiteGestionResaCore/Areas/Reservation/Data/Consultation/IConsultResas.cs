using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data.Consultation
{
    public interface IConsultResasDB
    {
        IList<InfosResasValid> ObtInfEssaiValidees();

        IList<InfosResaNonValid> ObtInfosEssaisRefusees();

        IList<InfosResaNonValid> ObtInfosEssaisSupprimees();
    }
}
