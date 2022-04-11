using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public partial class doc_qualite
    {
        public int id { get; set; }
        public byte[] contenu_doc_qualite { get; set; }
        public string nom_document { get; set; }
        public string nom_rubrique_doc { get; set; }
        public string? description_doc_qualite { get; set; }
        public DateTime? date_modif_doc { get; set; }
    }
}
