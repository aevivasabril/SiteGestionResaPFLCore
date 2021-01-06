using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    public class InfosCalenZone
    {
        public string NomZone { get; set; }

        public int IdZone { get; set; }

        public List<string> ListEquipements { get; set; }
    }
}
