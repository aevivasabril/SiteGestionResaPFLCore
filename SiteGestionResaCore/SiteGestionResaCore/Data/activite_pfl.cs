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
            doc_essai_pgd = new HashSet<doc_essai_pgd>();
        }
        public int id { get; set; }
        public string nom_activite { get; set; }
        public string type_documents { get; set; }

        public virtual ICollection<doc_essai_pgd> doc_essai_pgd { get; set; }
    }
}
