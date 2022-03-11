using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.DonneesPGD.Data
{
    public class InfosResasSansEntrepot
    {
        public int idEssai { get; set; }
        public string MailRespProj { get; set; }
        public DateTime DateSaisieEssai { get; set; }
        public string TitreEssai { get; set; }

        public string NomProjet { get; set; }
        public string NumProjet { get; set; }
        public int idProj { get; set; }
    }
}
