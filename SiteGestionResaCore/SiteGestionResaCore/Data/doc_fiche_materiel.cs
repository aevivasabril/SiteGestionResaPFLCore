using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public class doc_fiche_materiel
    {
        public int id { get; set; }
        public byte[] contenu_fiche { get; set; }
        public string nom_document { get; set; }
        public DateTime date_modification { get; set; }
        public int equipementID { get; set; }

        public virtual equipement equipement { get; set; }

    }
}
