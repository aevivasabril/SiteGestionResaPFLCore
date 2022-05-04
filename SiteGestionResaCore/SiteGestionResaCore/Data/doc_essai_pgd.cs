using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public class doc_essai_pgd
    {
        public int id { get; set; }
        public byte[] contenu_document { get; set; }
        public string nom_document { get; set; }
        public int? equipementID { get; set; }
        public int essaiID { get; set; }
        public int type_documentID { get; set; }
        public DateTime date_creation { get; set; }
        public int type_activiteID { get; set; }
        public double taille_ko { get; set; }

        public virtual equipement equipement { get; set; }
        public virtual essai essai { get; set; }
        public virtual type_document type_document { get; set; }
        public virtual activite_pfl activite_pfl { get; set; }
    }
}
