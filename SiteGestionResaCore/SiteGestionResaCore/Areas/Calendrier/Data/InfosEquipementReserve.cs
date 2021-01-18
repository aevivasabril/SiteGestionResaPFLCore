using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Calendrier.Data
{
    public class InfosEquipementReserve
    {
        public string TitreProjet { get; set; }

        public string NumeroProjet { get; set; }

        public string MailResponsablePj { get; set; }

        public string MailAuteurResa { get; set; }

        public int IdEssai { get; set; }

        public string Confidentialite { get; set; }
    }
}
