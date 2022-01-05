using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public partial class doc_qualite
    {
        public int id { get; set; }
        public string? nom_document { get; set; }
        public string? chemin_document { get; set; }
        public string description_doc { get; set; }
    }
}
