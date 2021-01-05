using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    public interface ICalendResaDb
    {
        IList<zone> GetAllZones();
    }
}
