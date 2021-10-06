using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Maintenance.Data.Maintenance
{
    public class MaintenanceInfos
    {
        public int IdMaint { get; set; }

        [Display(Name = "Type de maintenance : ")]
        public string TypeMaintenance { get; set; }

        [Display(Name = "Code d'opération: ")]
        public string CodeOperation { get; set; }

        [Display(Name = "Mail auteur: ")]
        public string EmailAuteur { get; set; }

        [Display(Name = "Nom intervenant externe: ")]
        public string NomIntervenantExt { get; set; }

        [Display(Name = "Description opération: ")]
        public string DescriptionOperation { get; set; }
    }
}
