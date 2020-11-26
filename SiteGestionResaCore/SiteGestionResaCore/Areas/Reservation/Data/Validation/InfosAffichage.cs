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
        /*public InfosEssai()
        {
            Reservations  = new List<InfosReservation>();
            InfosConflits = new List<InfosConflit>();
        }*/
        /// <summary>
        /// id essai
        /// </summary>
        public int idEssai { get; set; }
        //public int NumEssai { get; set; }
        public DateTime DateCreation { get; set; }
        //public string MailManipulateur { get; set; }
        public string MailUser { get; set; }
        public string Commentaire { get; set; }
        //public string Confidentialite { get; set; }
        //public string TypeProduitEntrant { get; set; }
        //public bool TransportStlo { get; set; }

        public bool ConflitExist { get; set; }

        //public List<InfosReservation> Reservations { get; set; }

        public string NomProjet { get; set; }
        public string NumProjet { get; set; }
        public int idProj { get; set; }

        //public List<InfosConflit> InfosConflits { get; set; }
    }
}
