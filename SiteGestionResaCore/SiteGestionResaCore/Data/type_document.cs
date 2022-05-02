using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Data
{
    public class type_document
    {
        public type_document()
        {
            doc_essai_pgd = new HashSet<doc_essai_pgd>();
        }

        public int id { get; set; }
        public string nom_document { get; set; }
        public string identificateur { get; set; }

        public virtual ICollection<doc_essai_pgd> doc_essai_pgd { get; set; }
    }
}
