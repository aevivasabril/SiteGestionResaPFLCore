using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data.Consultation
{
    public class InfosResaNonValid
    {       
        public int idEssai { get; set; }
        public DateTime DateValidation { get; set; }
        public string MailRespProj { get; set; }
        public DateTime DateSaisieEssai { get; set; }
        public string Confidentialit { get; set; }
        public string TitreEssai { get; set; }
        public string RaisonSuppression { get; set; }
        public string RaisonRefus { get; set; }

        public string NomProjet { get; set; }
        public string NumProjet { get; set; }
        public int idProj { get; set; }
    }
}
