using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Enquete.Data.PostEnquete
{
    public class EnquetesSansReponse
    {
        public int IdEnquete { get; set; }
        public string TitreEssai { get; set; }
        public string AuteurEssai { get; set; }
        public int IdEssai { get; set; }
    }
}
