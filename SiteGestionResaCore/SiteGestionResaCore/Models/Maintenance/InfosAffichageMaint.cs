using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Models.Maintenance
{
    public class InfosAffichageMaint
    {
        public int IdMaint { get; set; }
        public string CodeOperation { get; set; }
        public string TypeMaintenance { get; set; }
        public string MailOperateur { get; set; }
        public string DescriptionOperation { get; set; }       
    }
}
