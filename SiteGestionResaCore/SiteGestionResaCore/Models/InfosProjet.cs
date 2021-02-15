using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Models
{
    public class InfosProjet
    {
        public string TitreProjet { get; set; }
        public string NumProjet { get; set; }
        public string TypeProjet { get; set; }
        public string Financement { get; set; }
        public string Organisme { get; set; }
        public string MailRespProj { get; set; }
        public string Provenance { get; set; }
        public string Description { get; set; }
        public DateTime DateCreation { get; set; }
        public string MailUsrSaisie { get; set; }
    }
}
