using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data.Validation
{
    /// <summary>
    /// Classe répresentant certaines infos sur un essai
    /// </summary>
    public class InfosAffichage
    {
        /// <summary>
        /// id essai
        /// </summary>
        public int idEssai { get; set; }
        public DateTime DateCreation { get; set; }
        public string MailUser { get; set; }
        public string Commentaire { get; set; }

        public bool ConflitExist { get; set; }

        public string NomProjet { get; set; }
        public string NumProjet { get; set; }
        public int idProj { get; set; }
    }
}
