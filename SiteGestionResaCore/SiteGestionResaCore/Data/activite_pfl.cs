using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public class activite_pfl
    {
        public activite_pfl()
        {
            equipement = new HashSet<equipement>();
        }
        public int id { get; set; }
        public string nom_activite { get; set; }
        public string type_documents { get; set; }

        public virtual ICollection<equipement> equipement { get; set; }
    }
}
