using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Areas.Reservation.Data.Validation
{
    public class InfosEssai
    {
        public int id { get; set; }
        public DateTime DateCreation { get; set; }
        public string MailManipulateur { get; set; }
        public string TypeProduitEntrant { get; set; }
        public string MailUser { get; set; }
        public string Commentaire { get; set; }
        public string Confidentialite { get; set; }
        public bool TransportStlo { get; set; }
        public string PrecisionProd { get; set; }
        public string QuantiteProd { get; set; }
        public string ProveProd { get; set; }
        public string DestProd { get; set; }
    }
}
