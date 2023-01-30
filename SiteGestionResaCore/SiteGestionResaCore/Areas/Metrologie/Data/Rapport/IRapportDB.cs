using SiteGestionResaCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Metrologie.Data.Rapport
{
    public interface IRapportDB
    {
        public IList<EquipVsCapteur> GetEquipements();
    }
}
