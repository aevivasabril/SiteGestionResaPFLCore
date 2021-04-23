using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Models
{
    public class ConsultInfosEssaiChildVM
    {
        [DisplayName("N° Essai")]
        public int id { get; set; }
        [DisplayName("Date Création")]
        public DateTime DateCreation { get; set; }
        [DisplayName("Email manipulateur essai")]
        public string MailManipulateur { get; set; }
        [DisplayName("Type de produit entrant")]
        public string TypeProduitEntrant { get; set; }
        [DisplayName("Demande saisie par")]
        public string MailUser { get; set; }
        [DisplayName("Descriptif essai")]
        public string TitreEssai { get; set; }
        [DisplayName("Confidentialité essai")]
        public string Confidentialite { get; set; }
        [DisplayName("Transport assuré par")]
        public bool TransportStlo { get; set; }
        [DisplayName("Précision produit")]
        public string PrecisionProd { get; set; }
        [DisplayName("Quantité produit")]
        public int? QuantiteProd { get; set; }
        [DisplayName("Provenance produit")]
        public string ProveProd { get; set; }
        [DisplayName("Destination produit")]
        public string DestProd { get; set; }
    }
}
