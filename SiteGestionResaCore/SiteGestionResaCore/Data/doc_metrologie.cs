using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public class doc_metrologie
    {
        public int id { get; set; }
        public string nom_document { get; set; }
        public byte[] contenu_doc { get; set; }
        public DateTime date_creation { get; set; }
        public string description_doc { get; set; }
    }
}
